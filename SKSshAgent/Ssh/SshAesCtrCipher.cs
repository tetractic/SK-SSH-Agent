// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class SshAesCtrCipher : IDisposable
    {
        public const int BlockLength = 16;

        private readonly ICryptoTransform _encryptor;

        private readonly ulong[] _counter;

        private readonly byte[] _block;

        /// <exception cref="ArgumentException"/>
        public SshAesCtrCipher(ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv)
        {
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
                throw new ArgumentException("Invalid length.", nameof(key));
            if (iv.Length != BlockLength)
                throw new ArgumentException("Invalid length.", nameof(iv));

            byte[] keyArray = GC.AllocateUninitializedArray<byte>(key.Length, pinned: true);
            try
            {
                key.CopyTo(keyArray);

                // Aes wipes its copy of array containing the key when disposed, but it does not
                // pin the array, so the GC may relocate that array, leaving a copy of the key
                // in memory.  Let's dispose it as quickly as possible.
                using (var aes = Aes.Create())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.None;
                    aes.Key = keyArray;

                    _encryptor = aes.CreateEncryptor();
                }
            }
            finally
            {
                CryptographicOperations.ZeroMemory(keyArray);
            }

            _counter = GC.AllocateArray<ulong>(2, pinned: true);
            _counter[0] = BinaryPrimitives.ReadUInt64BigEndian(iv);
            _counter[1] = BinaryPrimitives.ReadUInt64BigEndian(iv.Slice(8));
            _block = GC.AllocateArray<byte>(BlockLength, pinned: true);
        }

        public void Dispose()
        {
            _encryptor.Dispose();
            CryptographicOperations.ZeroMemory(MemoryMarshal.AsBytes(_counter.AsSpan()));
            CryptographicOperations.ZeroMemory(_block);
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="CryptographicException"/>
        public void Encrypt(ReadOnlySpan<byte> plaintext, Span<byte> ciphertext)
        {
            if (plaintext.Length % BlockLength != 0)
                throw new ArgumentException("Invalid length.", nameof(plaintext));
            if (ciphertext.Length != plaintext.Length)
                throw new ArgumentException("Ciphertext length must match plaintext length.");

            Cipher(plaintext, ciphertext);
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="CryptographicException"/>
        public void Decrypt(ReadOnlySpan<byte> ciphertext, Span<byte> plaintext)
        {
            if (ciphertext.Length % BlockLength != 0)
                throw new ArgumentException("Invalid length.", nameof(ciphertext));
            if (plaintext.Length != ciphertext.Length)
                throw new ArgumentException("Plaintext length must match ciphertext length.");

            Cipher(ciphertext, plaintext);
        }

        /// <exception cref="CryptographicException"/>
        private void Cipher(ReadOnlySpan<byte> source, Span<byte> destination)
        {
            ref ulong counterHi = ref _counter[0];
            ref ulong counterLo = ref _counter[1];
            var block64 = MemoryMarshal.Cast<byte, ulong>(_block);
            ref ulong blockHi = ref block64[0];
            ref ulong blockLo = ref block64[1];
            var src64 = MemoryMarshal.Cast<byte, ulong>(source);
            var dst64 = MemoryMarshal.Cast<byte, ulong>(destination);

            while (src64.Length > 1)
            {
                if (BitConverter.IsLittleEndian)
                {
                    blockHi = BinaryPrimitives.ReverseEndianness(counterHi);
                    blockLo = BinaryPrimitives.ReverseEndianness(counterLo);
                }
                else
                {
                    blockHi = counterHi;
                    blockLo = counterLo;
                }

                int n = _encryptor.TransformBlock(_block, 0, _block.Length, _block, 0);
                Debug.Assert(n == _block.Length);

                dst64[0] = src64[0] ^ blockHi;
                dst64 = dst64.Slice(1);
                src64 = src64.Slice(1);
                dst64[0] = src64[0] ^ blockLo;
                dst64 = dst64.Slice(1);
                src64 = src64.Slice(1);

                // Increment counter.
                ulong temp = counterLo + 1;
                counterLo = temp;
                if (temp == 0)
                    counterHi += 1;
            }

            Debug.Assert(src64.Length == 0);
            Debug.Assert(dst64.Length == 0);
        }
    }
}
