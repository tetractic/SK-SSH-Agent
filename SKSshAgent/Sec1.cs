// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SKSshAgent
{
    /// <seealso href="https://www.secg.org/sec1-v2.pdf"/>
    internal static class Sec1
    {
        // Gets the number of bits in an unsigned big-endian big integer.
        internal static long GetBitLength(ReadOnlySpan<byte> bytes)
        {
            for (int i = 0; i < bytes.Length; ++i)
                if (bytes[i] != 0)
                    return (long)(bytes.Length - i) * 8 - BitOperations.LeadingZeroCount(bytes[i]);

            return 0;
        }

        internal static int SizeBitsToLength(int sizeBits) => (int)(((long)sizeBits + 7) / 8);

        /// <exception cref="InvalidDataException"/>
        internal static bool TryReadFieldElementBytes(ReadOnlySpan<byte> bytes, int fieldSizeBits, out BigInteger fieldElement, out int bytesConsumed)
        {
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.5

            int fieldElementLength = SizeBitsToLength(fieldSizeBits);

            if (bytes.Length < fieldElementLength)
            {
                fieldElement = default;
                bytesConsumed = 0;
                return false;
            }

            bytes = bytes.Slice(0, fieldElementLength);

            fieldElement = ReadFieldElementCore(bytes, fieldSizeBits);
            bytesConsumed = fieldElementLength;
            return true;
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static bool TryWriteFieldElementBytes(BigInteger fieldElement, int fieldSizeBits, Span<byte> destination, out int bytesWritten)
        {
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.5

            if (fieldElement < 0 || fieldElement.GetBitLength() > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(fieldElement));

            int fieldElementLength = SizeBitsToLength(fieldSizeBits);

            if (destination.Length < fieldElementLength)
            {
                bytesWritten = 0;
                return false;
            }
            else
            {
                WriteFieldElementBytesCore(fieldElement, fieldElementLength, destination);
                bytesWritten = fieldElementLength;
                return true;
            }
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        internal static BigInteger BytesToFieldElement(ReadOnlySpan<byte> bytes, int fieldSizeBits)
        {
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.5

            int fieldElementLength = SizeBitsToLength(fieldSizeBits);

            if (bytes.Length != fieldElementLength)
                throw new ArgumentException("Invalid size for EC field element.", nameof(bytes));

            return ReadFieldElementCore(bytes, fieldSizeBits);
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static byte[] FieldElementToBytes(BigInteger fieldElement, int fieldSizeBits)
        {
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.5

            if (fieldElement < 0 || fieldElement.GetBitLength() > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(fieldElement));

            int fieldElementLength = SizeBitsToLength(fieldSizeBits);

            byte[] bytes = new byte[fieldElementLength];
            WriteFieldElementBytesCore(fieldElement, fieldElementLength, bytes);
            return bytes;
        }

        /// <exception cref="ArgumentOutOfRangeException"/>
        internal static ImmutableArray<byte> FieldElementToImmutableBytes(BigInteger fieldElement, int fieldSizeBits)
        {
            byte[] bytes = FieldElementToBytes(fieldElement, fieldSizeBits);
            return Unsafe.As<byte[], ImmutableArray<byte>>(ref bytes);
        }

        /// <exception cref="InvalidDataException"/>
        private static BigInteger ReadFieldElementCore(ReadOnlySpan<byte> bytes, int fieldSizeBits)
        {
            if (GetBitLength(bytes) > fieldSizeBits)
                throw new InvalidDataException("Invalid EC field element.");

            return new BigInteger(bytes, isUnsigned: true, isBigEndian: true);
        }

        private static void WriteFieldElementBytesCore(BigInteger fieldElement, int fieldSizeBytes, Span<byte> destination)
        {
            int offset = fieldSizeBytes - fieldElement.GetByteCount(isUnsigned: true);
            destination.Slice(0, offset).Clear();
            bool completelyWritten = fieldElement.TryWriteBytes(destination.Slice(offset), out int bytesWritten, isUnsigned: true, isBigEndian: true);
            Debug.Assert(offset + bytesWritten == fieldSizeBytes || !completelyWritten);
        }
    }
}
