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
        h = 1
        */

        internal static void fe_1(out fe h)
        {
            h.e0 = 1;
            h.e1 = 0;
            h.e2 = 0;
            h.e3 = 0;
            h.e4 = 0;
            h.e5 = 0;
            h.e6 = 0;
            h.e7 = 0;
            h.e8 = 0;
            h.e9 = 0;
        }
    }
}
