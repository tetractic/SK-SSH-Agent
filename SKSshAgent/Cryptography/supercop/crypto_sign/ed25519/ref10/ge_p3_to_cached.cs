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

        private static readonly fe d2 = new(new[] { -21827239, -5839606, -30745221, 13898782, 229458, 15978800, -12551817, -6495438, 29715968, 9444199 });

        internal static void ge_p3_to_cached(out ge_cached r, in ge_p3 p)
        {
            fe_add(out r.YplusX, in p.Y, in p.X);
            fe_sub(out r.YminusX, in p.Y, in p.X);
            fe_copy(out r.Z, in p.Z);
            fe_mul(out r.T2d, in p.T, in d2);
        }
    }
}
