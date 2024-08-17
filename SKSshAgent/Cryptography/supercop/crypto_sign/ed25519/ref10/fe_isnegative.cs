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

namespace supercop.crypto_sign.ed25519.ref10;

internal partial struct fe
{
    /*
    return 1 if f is in {1,3,5,...,q-2}
    return 0 if f is in {0,2,4,...,q-1}

    Preconditions:
       |f| bounded by 1.1*2^26,1.1*2^25,1.1*2^26,1.1*2^25,etc.
    */

    internal static int fe_isnegative(in fe f)
    {
        Span<byte> s = stackalloc byte[32];
        fe_tobytes(s, f);
        return s[0] & 1;
    }
}
