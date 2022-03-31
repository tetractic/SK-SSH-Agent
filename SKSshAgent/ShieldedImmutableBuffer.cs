// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static Windows.Win32.PInvoke;

namespace SKSshAgent
{
    internal readonly struct ShieldedImmutableBuffer : IEquatable<ShieldedImmutableBuffer>
    {
        public static readonly ShieldedImmutableBuffer Empty = default;

        private readonly byte[]? _encryptedBuffer;

        private ShieldedImmutableBuffer(byte[] encryptedBuffer)
        {
            _encryptedBuffer = encryptedBuffer;
        }

        public bool IsEmpty => _encryptedBuffer == null;

        public ReadOnlyMemory<byte> ShieldedMemory => _encryptedBuffer.AsMemory();

        public ReadOnlySpan<byte> ShieldedSpan => _encryptedBuffer.AsSpan();

        public static bool operator ==(ShieldedImmutableBuffer left, ShieldedImmutableBuffer right) => left.Equals(right);

        public static bool operator ==(ShieldedImmutableBuffer? left, ShieldedImmutableBuffer? right) => left.GetValueOrDefault().Equals(right.GetValueOrDefault());

        public static bool operator !=(ShieldedImmutableBuffer left, ShieldedImmutableBuffer right) => !(left == right);

        public static bool operator !=(ShieldedImmutableBuffer? left, ShieldedImmutableBuffer? right) => !(left == right);

        /// <exception cref="CryptographicException"/>
        public static ShieldedImmutableBuffer Create(ReadOnlySpan<byte> unencryptedBuffer)
        {
            return Create(unencryptedBuffer.Length, unencryptedBuffer, (source, buffer) => source.CopyTo(buffer));
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CryptographicException"/>
        public static ShieldedImmutableBuffer Create<T>(int length, ReadOnlySpan<T> source, CreateFromSourceAction<T> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            if (length == 0)
            {
                action(source, Span<byte>.Empty);

                return Empty;
            }

            uint encryptedBufferLength = (uint)length + 4;
            const uint blockSize = CRYPTPROTECTMEMORY_BLOCK_SIZE;
            encryptedBufferLength += blockSize - 1;
            encryptedBufferLength -= encryptedBufferLength % blockSize;

            byte[] encryptedBuffer = new byte[encryptedBufferLength];

            unsafe
            {
                fixed (byte* encryptedBufferPtr = encryptedBuffer)
                {
                    try
                    {
                        BinaryPrimitives.WriteInt32LittleEndian(encryptedBuffer.AsSpan(encryptedBuffer.Length - 4), length);

                        action(source, encryptedBuffer.AsSpan(0, length));

                        if (!CryptProtectMemory(encryptedBufferPtr, (uint)encryptedBuffer.Length, CRYPTPROTECTMEMORY_SAME_PROCESS))
                            throw new CryptographicException(Marshal.GetLastPInvokeError());
                    }
                    catch
                    {
                        CryptographicOperations.ZeroMemory(encryptedBuffer);

                        throw;
                    }
                }
            }

            return new ShieldedImmutableBuffer(encryptedBuffer);
        }

        /// <exception cref="CryptographicException"/>
        public UnshieldScope Unshield() => new(_encryptedBuffer ?? Array.Empty<byte>());

        public bool Equals(ShieldedImmutableBuffer other)
        {
            return _encryptedBuffer == other._encryptedBuffer;
        }

        public override bool Equals(object? obj)
        {
            return obj is ShieldedImmutableBuffer &&
                   Equals((ShieldedImmutableBuffer)obj);
        }

        public override int GetHashCode()
        {
            return _encryptedBuffer?.GetHashCode() ?? 0;
        }

        public delegate void CreateFromSourceAction<T>(ReadOnlySpan<T> source, Span<byte> buffer);

        public struct UnshieldScope : IDisposable
        {
            private readonly byte[] _decryptedBuffer;

            private int _length;

            /// <exception cref="CryptographicException"/>
            internal UnshieldScope(byte[] encryptedBuffer)
            {
                _decryptedBuffer = GC.AllocateUninitializedArray<byte>(encryptedBuffer.Length, pinned: true);

                Array.Copy(encryptedBuffer, _decryptedBuffer, encryptedBuffer.Length);

                try
                {
                    unsafe
                    {
                        fixed (byte* decryptedBufferPtr = _decryptedBuffer)
                            if (!CryptUnprotectMemory(decryptedBufferPtr, (uint)_decryptedBuffer.Length, CRYPTPROTECTMEMORY_SAME_PROCESS))
                                throw new CryptographicException(Marshal.GetLastPInvokeError());
                    }

                    _length = BinaryPrimitives.ReadInt32LittleEndian(_decryptedBuffer.AsSpan(_decryptedBuffer.Length - 4));
                }
                catch
                {
                    CryptographicOperations.ZeroMemory(_decryptedBuffer);

                    throw;
                }
            }

            public void Dispose()
            {
                CryptographicOperations.ZeroMemory(_decryptedBuffer);

                _length = 0;
            }

            public ReadOnlyMemory<byte> UnshieldedMemory => _decryptedBuffer.AsMemory(0, _length);

            public ReadOnlySpan<byte> UnshieldedSpan => _decryptedBuffer.AsSpan(0, _length);

            public int UnshieldedLength => _length;
        }
    }
}
