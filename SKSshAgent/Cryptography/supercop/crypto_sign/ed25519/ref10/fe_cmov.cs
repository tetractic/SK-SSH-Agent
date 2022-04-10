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
        /*
        Replace (f,g) with (g,g) if b == 1;
        replace (f,g) with (f,g) if b == 0.

        Preconditions: b in {0,1}.
        */

        internal static void fe_cmov(ref fe f, in fe g, int b)
        {
            int f0 = f.e0;
            int f1 = f.e1;
            int f2 = f.e2;
            int f3 = f.e3;
            int f4 = f.e4;
            int f5 = f.e5;
            int f6 = f.e6;
            int f7 = f.e7;
            int f8 = f.e8;
            int f9 = f.e9;
            int g0 = g.e0;
            int g1 = g.e1;
            int g2 = g.e2;
            int g3 = g.e3;
            int g4 = g.e4;
            int g5 = g.e5;
            int g6 = g.e6;
            int g7 = g.e7;
            int g8 = g.e8;
            int g9 = g.e9;
            int x0 = f0 ^ g0;
            int x1 = f1 ^ g1;
            int x2 = f2 ^ g2;
            int x3 = f3 ^ g3;
            int x4 = f4 ^ g4;
            int x5 = f5 ^ g5;
            int x6 = f6 ^ g6;
            int x7 = f7 ^ g7;
            int x8 = f8 ^ g8;
            int x9 = f9 ^ g9;
            b = -b;
            x0 &= b;
            x1 &= b;
            x2 &= b;
            x3 &= b;
            x4 &= b;
            x5 &= b;
            x6 &= b;
            x7 &= b;
            x8 &= b;
            x9 &= b;
            f.e0 = f0 ^ x0;
            f.e1 = f1 ^ x1;
            f.e2 = f2 ^ x2;
            f.e3 = f3 ^ x3;
            f.e4 = f4 ^ x4;
            f.e5 = f5 ^ x5;
            f.e6 = f6 ^ x6;
            f.e7 = f7 ^ x7;
            f.e8 = f8 ^ x8;
            f.e9 = f9 ^ x9;
        }
    }
}
