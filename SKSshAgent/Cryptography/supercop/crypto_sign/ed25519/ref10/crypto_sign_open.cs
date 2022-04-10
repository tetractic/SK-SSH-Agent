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
using System.Security.Cryptography;
using static nacl.crypto_verify.crypto;
using static supercop.crypto_sign.ed25519.ref10.ge;
using static supercop.crypto_sign.ed25519.ref10.sc;

namespace supercop.crypto_sign.ed25519.ref10
{
    internal static partial class crypto
    {
        /// <remarks>
        /// <para>
        /// This is modified from the reference implementation to take the message in
        /// <paramref name="m"/> rather than in <paramref name="sm"/>.
        /// </para>
        /// <para>
        /// Note that the reference implementation does not provide some security guarantees that
        /// may be desired.  See <see href="https://eprint.iacr.org/2020/1244"/>.
        /// </para>
        /// </remarks>
        internal static int crypto_sign_open(ReadOnlySpan<byte> m, Span<byte> sm, ReadOnlySpan<byte> pk)
        {
            Span<byte> scopy = stackalloc byte[32];
            Span<byte> hram = stackalloc byte[64];
            Span<byte> rcheck = stackalloc byte[32];
            ge_p3 A;
            ge_p2 R;

            if ((sm[63] & 224) != 0)
                return -1;
            if (ge_frombytes_negate_vartime(out A, pk) != 0)
                return -1;

            sm.Slice(32, 32).CopyTo(scopy);

            m.CopyTo(sm.Slice(64));
            pk.CopyTo(sm.Slice(32));
            _ = SHA512.HashData(sm, hram);
            sc_reduce(hram);

            ge_double_scalarmult_vartime(out R, hram, in A, scopy);
            ge_tobytes(rcheck, in R);

            if (crypto_verify_32(sm.Slice(0, 32), rcheck) == 0)
                return 0;

            return -1;
        }
    }
}
