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
    internal static partial class ge
    {
        private static void slide(Span<sbyte> r, ReadOnlySpan<byte> a)
        {
            int i;
            int b;
            int k;

            for (i = 0; i < 256; ++i)
                r[i] = (sbyte)(1 & (a[i >> 3] >> (i & 7)));

            for (i = 0; i < 256; ++i)
            {
                if (r[i] != 0)
                {
                    for (b = 1; b <= 6 && i + b < 256; ++b)
                    {
                        if (r[i + b] != 0)
                        {
                            if (r[i] + (r[i + b] << b) <= 15)
                            {
                                r[i] += (sbyte)(r[i + b] << b);
                                r[i + b] = 0;
                            }
                            else if (r[i] - (r[i + b] << b) >= -15)
                            {
                                r[i] -= (sbyte)(r[i + b] << b);
                                for (k = i + b; k < 256; ++k)
                                {
                                    if (r[k] == 0)
                                    {
                                        r[k] = 1;
                                        break;
                                    }
                                    r[k] = 0;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
            }
        }

        private static readonly ge_precomp[] Bi = LoadBase(base2_data.Span);

        /*
        r = a * A + b * B
        where a = a[0]+256*a[1]+...+256^31 a[31].
        and b = b[0]+256*b[1]+...+256^31 b[31].
        B is the Ed25519 base point (x,4/5) with x positive.
        */

        internal static void ge_double_scalarmult_vartime(out ge_p2 r, ReadOnlySpan<byte> a, in ge_p3 A, ReadOnlySpan<byte> b)
        {
            Span<sbyte> aslide = stackalloc sbyte[256];
            Span<sbyte> bslide = stackalloc sbyte[256];
            Span<ge_cached> Ai = stackalloc ge_cached[8]; /* A,3A,5A,7A,9A,11A,13A,15A */
            ge_p1p1 t;
            ge_p3 u;
            ge_p3 A2;
            int i;

            slide(aslide,a);
            slide(bslide,b);

            ge_p3_to_cached(out Ai[0], in A);
            ge_p3_dbl(out t, in A); ge_p1p1_to_p3(out A2, in t);
            ge_add(out t,in A2,in Ai[0]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[1], in u);
            ge_add(out t,in A2,in Ai[1]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[2], in u);
            ge_add(out t,in A2,in Ai[2]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[3], in u);
            ge_add(out t,in A2,in Ai[3]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[4], in u);
            ge_add(out t,in A2,in Ai[4]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[5], in u);
            ge_add(out t,in A2,in Ai[5]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[6], in u);
            ge_add(out t,in A2,in Ai[6]); ge_p1p1_to_p3(out u, in t); ge_p3_to_cached(out Ai[7], in u);

            ge_p2_0(out r);

            for (i = 255; i >= 0; --i)
                if (aslide[i] != 0 || bslide[i] != 0)
                    break;

            for (; i >= 0; --i)
            {
                ge_p2_dbl(out t, in r);

                if (aslide[i] > 0)
                {
                    ge_p1p1_to_p3(out u, in t);
                    ge_add(out t, in u, in Ai[aslide[i] / 2]);
                }
                else if (aslide[i] < 0)
                {
                    ge_p1p1_to_p3(out u, in t);
                    ge_sub(out t, in u, in Ai[-aslide[i] / 2]);
                }

                if (bslide[i] > 0)
                {
                    ge_p1p1_to_p3(out u, in t);
                    ge_madd(out t, in u, in Bi[bslide[i] / 2]);
                }
                else if (bslide[i] < 0)
                {
                    ge_p1p1_to_p3(out u, in t);
                    ge_msub(out t, in u, in Bi[-bslide[i] / 2]);
                }

                ge_p1p1_to_p2(out r, in t);
            }
        }
    }
}
