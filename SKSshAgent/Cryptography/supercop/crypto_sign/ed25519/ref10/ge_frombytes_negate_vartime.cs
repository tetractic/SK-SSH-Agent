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
        private static readonly fe d = new(new[] { -10913610, 13857413, -15372611, 6949391, 114729, -8787816, -6275908, -3247719, -18696448, -12055116 });

        private static readonly fe sqrtm1 = new(new[] { -32595792, -7943725, 9377950, 3500415, 12389472, -272473, -25146209, -2005654, 326686, 11406482 });

        internal static int ge_frombytes_negate_vartime(out ge_p3 h, ReadOnlySpan<byte> s)
        {
            fe u;
            fe v;
            fe v3;
            fe vxx;
            fe check;

            fe_frombytes(out h.Y, s);
            fe_1(out h.Z);
            fe_sq(out u, in h.Y);
            fe_mul(out v, in u, in d);
            fe_sub(out u, in u, in h.Z);          /* u = y^2-1 */
            fe_add(out v, in v, in h.Z);          /* v = dy^2+1 */

            fe_sq(out v3, in v);
            fe_mul(out v3, in v3, in v);          /* v3 = v^3 */
            fe_sq(out h.X, in v3);
            fe_mul(out h.X, in h.X, in v);
            fe_mul(out h.X, in h.X, in u);        /* x = uv^7 */

            fe_pow22523(out h.X, in h.X);         /* x = (uv^7)^((q-5)/8) */
            fe_mul(out h.X, in h.X, in v3);
            fe_mul(out h.X, in h.X, in u);        /* x = uv^3(uv^7)^((q-5)/8) */

            fe_sq(out vxx, in h.X);
            fe_mul(out vxx, in vxx, in v);
            fe_sub(out check, in vxx, in u);      /* vx^2-u */
            if (fe_isnonzero(in check) != 0)
            {
                fe_add(out check, in vxx, in u);  /* vx^2+u */
                if (fe_isnonzero(in check) != 0)
                {
                    h.T = default;
                    return -1;
                }
                fe_mul(out h.X, in h.X, in sqrtm1);
            }

            if (fe_isnegative(in h.X) == (s[31] >> 7))
                fe_neg(out h.X, in h.X);

            fe_mul(out h.T, in h.X, in h.Y);
            return 0;
        }
    }
}
