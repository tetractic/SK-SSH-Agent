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

using static supercop.crypto_sign.ed25519.ref10.fe;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class ge
    {
        /*
        r = p + q
        */

        internal static void ge_madd(out ge_p1p1 r, in ge_p3 p, in ge_precomp q)
        {
            fe t0;

            /* qhasm: YpX1 = Y1+X1 */
            fe_add(out r.X, in p.Y, in p.X);

            /* qhasm: YmX1 = Y1-X1 */
            fe_sub(out r.Y, in p.Y, in p.X);

            /* qhasm: A = YpX1*ypx2 */
            fe_mul(out r.Z, in r.X, in q.yplusx);

            /* qhasm: B = YmX1*ymx2 */
            fe_mul(out r.Y, in r.Y, in q.yminusx);

            /* qhasm: C = xy2d2*T1 */
            fe_mul(out r.T, in q.xy2d, in p.T);

            /* qhasm: D = 2*Z1 */
            fe_add(out t0, in p.Z, in p.Z);

            /* qhasm: X3 = A-B */
            fe_sub(out r.X, in r.Z, in r.Y);

            /* qhasm: Y3 = A+B */
            fe_add(out r.Y, in r.Z, in r.Y);

            /* qhasm: Z3 = D+C */
            fe_add(out r.Z, in t0, in r.T);

            /* qhasm: T3 = D-C */
            fe_sub(out r.T, in t0, in r.T);
        }
    }
}
