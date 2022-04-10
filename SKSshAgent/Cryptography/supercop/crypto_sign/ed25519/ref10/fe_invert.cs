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

namespace supercop.crypto_sign.ed25519.ref10
{
    internal partial struct fe
    {
        internal static void fe_invert(out fe @out, in fe z)
        {
            fe t0;
            fe t1;
            fe t2;
            fe t3;
            int i;

            /* qhasm: z2 = z1^2^1 */
            fe_sq(out t0, in z); for (i = 1; i < 1; ++i) fe_sq(out t0, in t0);

            /* qhasm: z8 = z2^2^2 */
            fe_sq(out t1, in t0); for (i = 1; i < 2; ++i) fe_sq(out t1, in t1);

            /* qhasm: z9 = z1*z8 */
            fe_mul(out t1, in z, in t1);

            /* qhasm: z11 = z2*z9 */
            fe_mul(out t0, in t0, in t1);

            /* qhasm: z22 = z11^2^1 */
            fe_sq(out t2, in t0); for (i = 1; i < 1; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_5_0 = z9*z22 */
            fe_mul(out t1, in t1, in t2);

            /* qhasm: z_10_5 = z_5_0^2^5 */
            fe_sq(out t2, in t1); for (i = 1; i < 5; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_10_0 = z_10_5*z_5_0 */
            fe_mul(out t1, in t2, in t1);

            /* qhasm: z_20_10 = z_10_0^2^10 */
            fe_sq(out t2, in t1); for (i = 1; i < 10; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_20_0 = z_20_10*z_10_0 */
            fe_mul(out t2, in t2, in t1);

            /* qhasm: z_40_20 = z_20_0^2^20 */
            fe_sq(out t3, in t2); for (i = 1; i < 20; ++i) fe_sq(out t3, in t3);

            /* qhasm: z_40_0 = z_40_20*z_20_0 */
            fe_mul(out t2, in t3, in t2);

            /* qhasm: z_50_10 = z_40_0^2^10 */
            fe_sq(out t2, in t2); for (i = 1; i < 10; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_50_0 = z_50_10*z_10_0 */
            fe_mul(out t1, in t2, in t1);

            /* qhasm: z_100_50 = z_50_0^2^50 */
            fe_sq(out t2, in t1); for (i = 1; i < 50; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_100_0 = z_100_50*z_50_0 */
            fe_mul(out t2, in t2, in t1);

            /* qhasm: z_200_100 = z_100_0^2^100 */
            fe_sq(out t3, in t2); for (i = 1; i < 100; ++i) fe_sq(out t3, in t3);

            /* qhasm: z_200_0 = z_200_100*z_100_0 */
            fe_mul(out t2, in t3, in t2);

            /* qhasm: z_250_50 = z_200_0^2^50 */
            fe_sq(out t2, in t2); for (i = 1; i < 50; ++i) fe_sq(out t2, in t2);

            /* qhasm: z_250_0 = z_250_50*z_50_0 */
            fe_mul(out t1, in t2, in t1);

            /* qhasm: z_255_5 = z_250_0^2^5 */
            fe_sq(out t1, in t1); for (i = 1; i < 5; ++i) fe_sq(out t1, in t1);

            /* qhasm: z_255_21 = z_255_5*z11 */
            fe_mul(out @out, in t1, in t0);
        }
    }
}
