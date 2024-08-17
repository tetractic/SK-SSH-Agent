// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh;

internal sealed class SshAesCbcCipher : IDisposable
{
    public const int BlockLength = 16;

    private readonly ICryptoTransform _encryptor;

    private readonly ICryptoTransform _decryptor;

    private readonly byte[] _block;

    /// <exception cref="ArgumentException"/>
    public SshAesCbcCipher(ReadOnlySpan<byte> key, ReadOnlySpan<byte> iv)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Invalid length.", nameof(key));
        if (iv.Length != BlockLength)
            throw new ArgumentException("Invalid length.", nameof(iv));

        byte[] keyArray = GC.AllocateUninitializedArray<byte>(key.Length, pinned: true);
        byte[] ivArray = GC.AllocateUninitializedArray<byte>(iv.Length, pinned: true);
        try
        {
            key.CopyTo(keyArray);
            iv.CopyTo(ivArray);

            // Aes wipes its copy of array containing the key when disposed, but it does not
            // pin the array, so the GC may relocate that array, leaving a copy of the key
            // in memory.  Let's dispose it as quickly as possible.
            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;
                aes.Key = keyArray;
                aes.IV = ivArray;

                _encryptor = aes.CreateEncryptor();
                _decryptor = aes.CreateDecryptor();
            }
        }
        finally
        {
            CryptographicOperations.ZeroMemory(keyArray);
            CryptographicOperations.ZeroMemory(ivArray);
        }

        _block = GC.AllocateArray<byte>(BlockLength, pinned: true);
    }

    public void Dispose()
    {
        _encryptor.Dispose();
        _decryptor.Dispose();
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

        while (plaintext.Length > 0)
        {
            plaintext.Slice(0, _block.Length).CopyTo(_block);

            int n = _encryptor.TransformBlock(_block, 0, _block.Length, _block, 0);
            Debug.Assert(n == _block.Length);

            _block.AsSpan().CopyTo(ciphertext);

            plaintext = plaintext.Slice(_block.Length);
            ciphertext = ciphertext.Slice(_block.Length);
        }
    }

    /// <exception cref="ArgumentException"/>
    /// <exception cref="CryptographicException"/>
    public void Decrypt(ReadOnlySpan<byte> ciphertext, Span<byte> plaintext)
    {
        if (ciphertext.Length % BlockLength != 0)
            throw new ArgumentException("Invalid length.", nameof(ciphertext));
        if (plaintext.Length != ciphertext.Length)
            throw new ArgumentException("Plaintext length must match ciphertext length.");

        while (ciphertext.Length > 0)
        {
            ciphertext.Slice(0, _block.Length).CopyTo(_block);

            int n = _decryptor.TransformBlock(_block, 0, _block.Length, _block, 0);
            Debug.Assert(n == _block.Length);

            _block.AsSpan().CopyTo(plaintext);

            ciphertext = ciphertext.Slice(_block.Length);
            plaintext = plaintext.Slice(_block.Length);
        }
    }
}
