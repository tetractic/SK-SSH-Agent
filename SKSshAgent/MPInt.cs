// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Numerics;

namespace SKSshAgent
{
    internal static class MPInt
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
    }
}
