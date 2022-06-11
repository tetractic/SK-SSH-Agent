// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

// All of the NaCl software is in the public domain. [1]
// https://nacl.cr.yp.to/features.html

using System;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace nacl.crypto_verify
{
    internal static partial class crypto
    {
        /*
        return  0 if x[i] == y[i] for all i in {0,1,2,...,31}
        return -1 if x[i] != y[i] for any i in {0,1,2,...,31}
        */

        internal static int crypto_verify_32(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
        {
            int differentbits = 0;
            differentbits |= x[0] ^ y[0];
            differentbits |= x[1] ^ y[1];
            differentbits |= x[2] ^ y[2];
            differentbits |= x[3] ^ y[3];
            differentbits |= x[4] ^ y[4];
            differentbits |= x[5] ^ y[5];
            differentbits |= x[6] ^ y[6];
            differentbits |= x[7] ^ y[7];
            differentbits |= x[8] ^ y[8];
            differentbits |= x[9] ^ y[9];
            differentbits |= x[10] ^ y[10];
            differentbits |= x[11] ^ y[11];
            differentbits |= x[12] ^ y[12];
            differentbits |= x[13] ^ y[13];
            differentbits |= x[14] ^ y[14];
            differentbits |= x[15] ^ y[15];
            differentbits |= x[16] ^ y[16];
            differentbits |= x[17] ^ y[17];
            differentbits |= x[18] ^ y[18];
            differentbits |= x[19] ^ y[19];
            differentbits |= x[20] ^ y[20];
            differentbits |= x[21] ^ y[21];
            differentbits |= x[22] ^ y[22];
            differentbits |= x[23] ^ y[23];
            differentbits |= x[24] ^ y[24];
            differentbits |= x[25] ^ y[25];
            differentbits |= x[26] ^ y[26];
            differentbits |= x[27] ^ y[27];
            differentbits |= x[28] ^ y[28];
            differentbits |= x[29] ^ y[29];
            differentbits |= x[30] ^ y[30];
            differentbits |= x[31] ^ y[31];
            return (1 & ((differentbits - 1) >> 8)) - 1;
        }
    }
}
