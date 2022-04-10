// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
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

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class sc
    {
        /*
        The set of scalars is \Z/l
        where l = 2^252 + 27742317777372353535851937790883648493.
        */

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
