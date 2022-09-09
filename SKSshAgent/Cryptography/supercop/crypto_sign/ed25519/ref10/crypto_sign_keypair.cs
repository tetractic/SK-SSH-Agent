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
using System.Security.Cryptography;
using static supercop.crypto_sign.ed25519.ref10.ge;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class crypto
    {
        internal static void crypto_sign_keypair(Span<byte> pk, Span<byte> sk)
        {
            Span<byte> az = stackalloc byte[64];
            ge_p3 A;

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(sk.Slice(0, 32));
            _ = SHA512.HashData(sk.Slice(0, 32), az);
            az[0] &= 248;
            az[31] &= 63;
            az[31] |= 64;

            ge_scalarmult_base(out A, az);
            ge_p3_tobytes(pk, in A);

            pk.CopyTo(sk.Slice(32));
        }
    }
}
