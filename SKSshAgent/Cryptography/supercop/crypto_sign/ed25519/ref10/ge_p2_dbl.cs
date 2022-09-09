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
        r = 2 * p
        */

        internal static void ge_p2_dbl(out ge_p1p1 r, in ge_p2 p)
        {
            fe t0;

            /* qhasm: XX=X1^2 */
            fe_sq(out r.X, in p.X);

            /* qhasm: YY=Y1^2 */
            fe_sq(out r.Z, in p.Y);

            /* qhasm: B=2*Z1^2 */
            fe_sq2(out r.T, in p.Z);

            /* qhasm: A=X1+Y1 */
            fe_add(out r.Y, in p.X, in p.Y);

            /* qhasm: AA=A^2 */
            fe_sq(out t0, in r.Y);

            /* qhasm: Y3=YY+XX */
            fe_add(out r.Y, in r.Z, in r.X);

            /* qhasm: Z3=YY-XX */
            fe_sub(out r.Z, in r.Z, in r.X);

            /* qhasm: X3=AA-Y3 */
            fe_sub(out r.X, in t0, in r.Y);

            /* qhasm: T3=B-Z3 */
            fe_sub(out r.T, in r.T, in r.Z);
        }
    }
}
