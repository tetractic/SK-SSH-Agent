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

using static supercop.crypto_sign.ed25519.ref10.fe;

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class ge
    {
        /*
        r = p
        */

        internal static void ge_p1p1_to_p3(out ge_p3 r, in ge_p1p1 p)
        {
            fe_mul(out r.X, in p.X, in p.T);
            fe_mul(out r.Y, in p.Y, in p.Z);
            fe_mul(out r.Z, in p.Z, in p.T);
            fe_mul(out r.T, in p.X, in p.Y);
        }
    }
}
