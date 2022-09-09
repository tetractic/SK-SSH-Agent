// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;

namespace SKSshAgent.Ssh
{
    internal static class SshEC
    {
        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        internal static (ImmutableArray<byte> X, ImmutableArray<byte> Y) ReadECPoint(ref SshWireReader reader, int fieldSizeBits)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshbuf-getput-crypto.c#L74
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.4 (simplification thereof)

            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);

            var ecPointBuffer = reader.ReadByteString();

            int ecPointLength = 1 + 2 * fieldElementLength;

            if (ecPointBuffer.Length != ecPointLength || ecPointBuffer[0] != 4)
                throw new InvalidDataException("Invalid EC point data.");

            var x = ecPointBuffer.Slice(1, fieldElementLength);
            var y = ecPointBuffer.Slice(1 + fieldElementLength, fieldElementLength);

            if (MPInt.GetBitLength(x) > fieldSizeBits || MPInt.GetBitLength(y) > fieldSizeBits)
                throw new InvalidDataException("Invalid EC point data.");

            return (x.ToImmutableArray(), y.ToImmutableArray());
        }

        internal static void WriteECPoint(ref SshWireWriter writer, int fieldSizeBits, ImmutableArray<byte> x, ImmutableArray<byte> y)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshbuf-getput-crypto.c#L154
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.3 (simplification thereof)

            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);

            Debug.Assert(x.Length == fieldElementLength && MPInt.GetBitLength(x.AsSpan()) <= fieldSizeBits);
            Debug.Assert(y.Length == fieldElementLength && MPInt.GetBitLength(y.AsSpan()) <= fieldSizeBits);

            int ecPointLength = 1 + 2 * fieldElementLength;

            byte[] ecPointBuffer = new byte[ecPointLength];

            ecPointBuffer[0] = 4;
            x.CopyTo(ecPointBuffer, 1);
            y.CopyTo(ecPointBuffer, 1 + fieldElementLength);

            writer.WriteByteString(ecPointBuffer);
        }

        /// <exception cref="InvalidDataException"/>
        internal static void SshWireMPIntToFieldElementBytes(ReadOnlySpan<byte> bytes, Span<byte> fieldElementBytes)
        {
            if (bytes.Length > 0 && (bytes[0] & 0x80) != 0)
                throw new InvalidDataException("Invalid EC field element.");

            while (bytes.Length > 0 && bytes[0] == 0)
                bytes = bytes.Slice(1);

            if (bytes.Length > fieldElementBytes.Length)
                throw new InvalidDataException("Invalid EC field element.");

            int offset = fieldElementBytes.Length - bytes.Length;
            fieldElementBytes.Slice(0, offset).Clear();
            bytes.CopyTo(fieldElementBytes.Slice(offset));
        }

        internal static void WriteFieldElementAsMPInt(ref SshWireWriter writer, ReadOnlySpan<byte> fieldElementBytes)
        {
            while (fieldElementBytes.Length > 0 && fieldElementBytes[0] == 0)
                fieldElementBytes = fieldElementBytes.Slice(1);

            if (fieldElementBytes.Length > 0 && (fieldElementBytes[0] & 0x80) != 0)
            {
                writer.WriteUInt32((uint)fieldElementBytes.Length + 1);
                writer.WriteByte(0);
            }
            else
            {
                writer.WriteUInt32((uint)fieldElementBytes.Length);
            }
            writer.WriteBytes(fieldElementBytes);
        }
    }
}
