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

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    internal partial struct fe
    {
        /*
        Ignores top bit of h.
        */

        internal static void fe_frombytes(out fe h, ReadOnlySpan<byte> s)
        {
            long h0 = load_4(s);
            long h1 = load_3(s.Slice(4)) << 6;
            long h2 = load_3(s.Slice(7)) << 5;
            long h3 = load_3(s.Slice(10)) << 3;
            long h4 = load_3(s.Slice(13)) << 2;
            long h5 = load_4(s.Slice(16));
            long h6 = load_3(s.Slice(20)) << 7;
            long h7 = load_3(s.Slice(23)) << 5;
            long h8 = load_3(s.Slice(26)) << 4;
            long h9 = (load_3(s.Slice(29)) & 8388607) << 2;
            long carry0;
            long carry1;
            long carry2;
            long carry3;
            long carry4;
            long carry5;
            long carry6;
            long carry7;
            long carry8;
            long carry9;

            carry9 = (h9 + (long)(1<<24)) >> 25; h0 += carry9 * 19; h9 -= carry9 << 25;
            carry1 = (h1 + (long)(1<<24)) >> 25; h2 += carry1; h1 -= carry1 << 25;
            carry3 = (h3 + (long)(1<<24)) >> 25; h4 += carry3; h3 -= carry3 << 25;
            carry5 = (h5 + (long)(1<<24)) >> 25; h6 += carry5; h5 -= carry5 << 25;
            carry7 = (h7 + (long)(1<<24)) >> 25; h8 += carry7; h7 -= carry7 << 25;

            carry0 = (h0 + (long)(1<<25)) >> 26; h1 += carry0; h0 -= carry0 << 26;
            carry2 = (h2 + (long)(1<<25)) >> 26; h3 += carry2; h2 -= carry2 << 26;
            carry4 = (h4 + (long)(1<<25)) >> 26; h5 += carry4; h4 -= carry4 << 26;
            carry6 = (h6 + (long)(1<<25)) >> 26; h7 += carry6; h6 -= carry6 << 26;
            carry8 = (h8 + (long)(1<<25)) >> 26; h9 += carry8; h8 -= carry8 << 26;

            h.e0 = (int)h0;
            h.e1 = (int)h1;
            h.e2 = (int)h2;
            h.e3 = (int)h3;
            h.e4 = (int)h4;
            h.e5 = (int)h5;
            h.e6 = (int)h6;
            h.e7 = (int)h7;
            h.e8 = (int)h8;
            h.e9 = (int)h9;
        }
    }
}
