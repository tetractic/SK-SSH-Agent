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
using System.Diagnostics;
using static supercop.crypto_sign.ed25519.ref10.fe;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class ge
    {
        private static byte equal(byte b, byte c)
        {
            uint y = (uint)(b ^ c); /* 0: yes; 1..255: no */
            y -= 1; /* 4294967295: yes; 0..254: no */
            y >>= 31; /* 1: yes; 0: no */
            return (byte)y;
        }

        private static byte negative(sbyte b)
        {
            byte x = (byte)b; /* 128..255: yes; 0..127: no */
            x >>= 7; /* 1: yes; 0: no */
            return x;
        }

        private static void cmov(ref ge_precomp t, in ge_precomp u, byte b)
        {
            fe_cmov(ref t.yplusx, in u.yplusx, b);
            fe_cmov(ref t.yminusx, in u.yminusx, b);
            fe_cmov(ref t.xy2d, in u.xy2d, b);
        }

        /* base[i][j] = (j+1)*256^i*B */
        private static readonly Base @base = default;
        private readonly struct Base
        {
            private static readonly ge_precomp[] _items = LoadBase(base_data.Span);

            internal ref readonly ge_precomp this[int i1, int i2]
            {
                get
                {
                    Debug.Assert((uint)i1 < 32);
                    Debug.Assert((uint)i2 < 8);
                    return ref _items[i1 * 8 + i2];
                }
            }
        }

        private static void select(out ge_precomp t, int pos, sbyte b)
        {
            ge_precomp minust;
            byte bnegative = negative(b);
            byte babs = (byte)(b - (((-bnegative) & b) << 1));

            ge_precomp_0(out t);
            cmov(ref t, in @base[pos, 0], equal(babs, 1));
            cmov(ref t, in @base[pos, 1], equal(babs, 2));
            cmov(ref t, in @base[pos, 2], equal(babs, 3));
            cmov(ref t, in @base[pos, 3], equal(babs, 4));
            cmov(ref t, in @base[pos, 4], equal(babs, 5));
            cmov(ref t, in @base[pos, 5], equal(babs, 6));
            cmov(ref t, in @base[pos, 6], equal(babs, 7));
            cmov(ref t, in @base[pos, 7], equal(babs, 8));
            fe_copy(out minust.yplusx, in t.yminusx);
            fe_copy(out minust.yminusx, in t.yplusx);
            fe_neg(out minust.xy2d, in t.xy2d);
            cmov(ref t, in minust, bnegative);
        }

        /*
        h = a * B
        where a = a[0]+256*a[1]+...+256^31 a[31]
        B is the Ed25519 base point (x,4/5) with x positive.

        Preconditions:
          a[31] <= 127
        */

        internal static void ge_scalarmult_base(out ge_p3 h, ReadOnlySpan<byte> a)
        {
            Span<sbyte> e = stackalloc sbyte[64];
            sbyte carry;
            ge_p1p1 r;
            ge_p2 s;
            ge_precomp t;
            int i;

            for (i = 0; i < 32; ++i)
            {
                e[2 * i + 0] = (sbyte)((a[i] >> 0) & 15);
                e[2 * i + 1] = (sbyte)((a[i] >> 4) & 15);
            }
            /* each e[i] is between 0 and 15 */
            /* e[63] is between 0 and 7 */

            carry = 0;
            for (i = 0; i < 63; ++i)
            {
                e[i] += carry;
                carry = (sbyte)(e[i] + 8);
                carry >>= 4;
                e[i] -= (sbyte)(carry << 4);
            }
            e[63] += carry;
            /* each e[i] is between -8 and 8 */

            ge_p3_0(out h);
            for (i = 1; i < 64; i += 2)
            {
                select(out t, i / 2, e[i]);
                ge_madd(out r, in h, in t); ge_p1p1_to_p3(out h, in r);
            }

            ge_p3_dbl(out r, in h); ge_p1p1_to_p2(out s, in r);
            ge_p2_dbl(out r, in s); ge_p1p1_to_p2(out s, in r);
            ge_p2_dbl(out r, in s); ge_p1p1_to_p2(out s, in r);
            ge_p2_dbl(out r, in s); ge_p1p1_to_p3(out h, in r);

            for (i = 0; i < 64; i += 2)
            {
                select(out t, i / 2, e[i]);
                ge_madd(out r, in h, in t); ge_p1p1_to_p3(out h, in r);
            }
        }
    }
}
