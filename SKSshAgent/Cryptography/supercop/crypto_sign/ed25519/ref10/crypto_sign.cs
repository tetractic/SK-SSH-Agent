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
using static supercop.crypto_sign.ed25519.ref10.sc;

#pragma warning disable CA1704 // Identifiers should be spelled correctly
#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace supercop.crypto_sign.ed25519.ref10;

internal static partial class crypto
{
    internal static void crypto_sign(Span<byte> sm, ReadOnlySpan<byte> m, ReadOnlySpan<byte> sk)
    {
        Span<byte> pk = stackalloc byte[32];
        Span<byte> az = stackalloc byte[64];
        Span<byte> nonce = stackalloc byte[64];
        Span<byte> hram = stackalloc byte[64];
        ge_p3 R;

        sk.Slice(32).CopyTo(pk);

        _ = SHA512.HashData(sk.Slice(0, 32), az);
        az[0] &= 248;
        az[31] &= 63;
        az[31] |= 64;

        m.CopyTo(sm.Slice(64));
        az.Slice(32).CopyTo(sm.Slice(32));
        _ = SHA512.HashData(sm.Slice(32), nonce);
        pk.CopyTo(sm.Slice(32));

        sc_reduce(nonce);
        ge_scalarmult_base(out R, nonce);
        ge_p3_tobytes(sm, in R);

        _ = SHA512.HashData(sm, hram);
        sc_reduce(hram);
        sc_muladd(sm.Slice(32), hram, az, nonce);
    }
}
