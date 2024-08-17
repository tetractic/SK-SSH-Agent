// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace SKSshAgent.Ssh;

// https://datatracker.ietf.org/doc/html/rfc4251#section-5
internal ref struct SshWireWriter
{
    private readonly IBufferWriter<byte>? _buffer;

    private Span<byte> _span;

    private int _bufferedLength;

    public SshWireWriter(Span<byte> destination)
    {
        _buffer = null;
        _span = destination;
        _bufferedLength = 0;
    }

    /// <exception cref="ArgumentNullException"/>
    public SshWireWriter(IBufferWriter<byte> destination)
    {
        ArgumentNullException.ThrowIfNull(destination);

        _buffer = destination;
        _span = destination.GetSpan();
        _bufferedLength = 0;
    }

    public readonly int BufferedLength => _bufferedLength;

    public void Flush()
    {
        if (_buffer != null)
        {
            if (_bufferedLength > 0)
            {
                _buffer.Advance(_bufferedLength);
                _bufferedLength = 0;
            }
            _span = Span<byte>.Empty;
        }
    }

    /// <exception cref="InvalidOperationException"/>
    public void WriteBoolean(bool value) => WriteByte((byte)(value ? 1 : 0));

    /// <exception cref="InvalidOperationException"/>
    public void WriteByte(byte value)
    {
        EnsureCapacity(1);

        _span[0] = value;
        Advance(1);
    }

    /// <exception cref="InvalidOperationException"/>
    public void WriteBytes(ReadOnlySpan<byte> bytes)
    {
        int length = bytes.Length;

        EnsureCapacity(length);

        bytes.CopyTo(_span);
        Advance(length);
    }

    /// <exception cref="InvalidOperationException"/>
    public void WriteUInt32(uint value)
    {
        EnsureCapacity(4);

        BinaryPrimitives.WriteUInt32BigEndian(_span, value);
        Advance(4);
    }

    /// <exception cref="InvalidOperationException"/>
    public void WriteByteString(ReadOnlySpan<byte> value)
    {
        EnsureCapacity(4 + value.Length);

        BinaryPrimitives.WriteUInt32BigEndian(_span, (uint)value.Length);

        value.CopyTo(_span.Slice(4));
        Advance(4 + value.Length);
    }

    /// <exception cref="InvalidOperationException"/>
    public void WriteString(string value)
    {
        int length = Encoding.UTF8.GetByteCount(value);

        EnsureCapacity(4 + length);

        BinaryPrimitives.WriteUInt32BigEndian(_span, (uint)length);

        int writtenLength = Encoding.UTF8.GetBytes(value, _span.Slice(4));
        Debug.Assert(length == writtenLength);
        Advance(4 + length);
    }

    /// <exception cref="InvalidOperationException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int pendingLength)
    {
        if (_span.Length < pendingLength)
            FlushAndEnsureCapacity(pendingLength);
    }

    /// <exception cref="InvalidOperationException"/>
    private void FlushAndEnsureCapacity(int pendingLength)
    {
        if (_buffer != null)
        {
            if (_bufferedLength > 0)
            {
                _buffer.Advance(_bufferedLength);
                _bufferedLength = 0;
            }
            _span = _buffer.GetSpan(pendingLength);
        }

        if (_span.Length < pendingLength)
            throw new InvalidOperationException("Reached the end of the destination buffer while attempting to write.");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance(int length)
    {
        _span = _span.Slice(length);
        _bufferedLength += length;
    }
}
