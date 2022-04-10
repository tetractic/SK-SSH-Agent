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
        h = -f

        Preconditions:
           |f| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24,etc.

        Postconditions:
           |h| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24,etc.
        */

        internal static void fe_neg(out fe h, in fe f)
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
            int h0 = -f0;
            int h1 = -f1;
            int h2 = -f2;
            int h3 = -f3;
            int h4 = -f4;
            int h5 = -f5;
            int h6 = -f6;
            int h7 = -f7;
            int h8 = -f8;
            int h9 = -f9;
            h.e0 = h0;
            h.e1 = h1;
            h.e2 = h2;
            h.e3 = h3;
            h.e4 = h4;
            h.e5 = h5;
            h.e6 = h6;
            h.e7 = h7;
            h.e8 = h8;
            h.e9 = h9;
        }
    }
}
