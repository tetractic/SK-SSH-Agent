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
using static supercop.crypto_sign.ed25519.ref10.fe;

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class ge
    {
        internal static void ge_tobytes(Span<byte> s, in ge_p2 h)
        {
            fe recip;
            fe x;
            fe y;

            fe_invert(out recip, in h.Z);
            fe_mul(out x, in h.X, in recip);
            fe_mul(out y, in h.Y, in recip);
            fe_tobytes(s, y);
            s[31] ^= (byte)(fe_isnegative(in x) << 7);
        }
    }
}
