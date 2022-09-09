// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

// Contributors (in alphabetical order): [1]
//  * Daniel J. Bernstein
//  * Niels Duif
//  * Tanja Lange,
//  * Peter Schwabe
//  * Bo-Yin Yang
// The Ed25519 software is in the public domain. [2]
// [1] https://ed25519.cr.yp.to/
// [2] https://ed25519.cr.yp.to/software.html

using System;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    /*
    fe means field element.
    Here the field is \Z/(2^255-19).
    An element t, entries t[0]...t[9], represents the integer
    t[0]+2^26 t[1]+2^51 t[2]+2^77 t[3]+2^102 t[4]+...+2^230 t[9].
    Bounds on each t[i] vary depending on context.
    */

    internal partial struct fe
    {
        internal int e0;
        internal int e1;
        internal int e2;
        internal int e3;
        internal int e4;
        internal int e5;
        internal int e6;
        internal int e7;
        internal int e8;
        internal int e9;

        internal fe(ReadOnlySpan<int> e)
        {
            e0 = e[0];
            e1 = e[1];
            e2 = e[2];
            e3 = e[3];
            e4 = e[4];
            e5 = e[5];
            e6 = e[6];
            e7 = e[7];
            e8 = e[8];
            e9 = e[9];
        }

        private static long load_3(ReadOnlySpan<byte> @in)
        {
            ulong result;
            result = (ulong)@in[0];
            result |= ((ulong)@in[1]) << 8;
            result |= ((ulong)@in[2]) << 16;
            return (long)result;
        }

        private static long load_4(ReadOnlySpan<byte> @in)
        {
            ulong result;
            result = (ulong)@in[0];
            result |= ((ulong)@in[1]) << 8;
            result |= ((ulong)@in[2]) << 16;
            result |= ((ulong)@in[3]) << 24;
            return (long)result;
        }
    }
}
