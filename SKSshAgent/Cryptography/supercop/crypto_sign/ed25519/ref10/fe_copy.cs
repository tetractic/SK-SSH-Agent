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

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    internal partial struct fe
    {
        /*
        h = f
        */

        internal static void fe_copy(out fe h, in fe f)
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
            h.e0 = f0;
            h.e1 = f1;
            h.e2 = f2;
            h.e3 = f3;
            h.e4 = f4;
            h.e5 = f5;
            h.e6 = f6;
            h.e7 = f7;
            h.e8 = f8;
            h.e9 = f9;
        }
    }
}
