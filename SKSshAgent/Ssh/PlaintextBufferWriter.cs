// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class PlaintextBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private const int _arrayMaxLength = 0x7FEFFFFF;

        private byte[] _buffer;

        private int _length;

        public PlaintextBufferWriter()
        {
            _buffer = Array.Empty<byte>();
        }

        public int WrittenLength => _length;

        public Span<byte> WrittenSpan => _buffer.AsSpan(0, _length);

        public void Dispose()
        {
            CryptographicOperations.ZeroMemory(_buffer.AsSpan(0, _length));
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public void Advance(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (count > _buffer.Length - _length)
                throw new InvalidOperationException("Attempted to advance beyond the end of the buffer.");

            _length += count;
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            EnsureCapacity(sizeHint);

            return _buffer.AsMemory(_length);
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            EnsureCapacity(sizeHint);

            return _buffer.AsSpan(_length);
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        private void EnsureCapacity(int sizeHint)
        {
            if (sizeHint < 0)
                throw new ArgumentOutOfRangeException(nameof(sizeHint));

            uint minCapacity = (uint)_buffer.Length + (uint)sizeHint;
            uint newCapacity = Math.Max(minCapacity, (uint)_buffer.Length * 2);
            if (newCapacity > _arrayMaxLength)
                newCapacity = Math.Min(minCapacity, _arrayMaxLength);

            byte[] newBuffer = GC.AllocateUninitializedArray<byte>((int)newCapacity, pinned: true);
            Array.Copy(_buffer, newBuffer, _length);
            Array.Clear(newBuffer, _length, newBuffer.Length - _length);
            CryptographicOperations.ZeroMemory(_buffer.AsSpan(0, _length));
            _buffer = newBuffer;
        }
    }
}
