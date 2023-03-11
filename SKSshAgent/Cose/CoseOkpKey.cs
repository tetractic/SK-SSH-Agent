// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Collections.Immutable;

namespace SKSshAgent.Cose
{
    internal sealed class CoseOkpKey : CoseKey
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public CoseOkpKey(CoseAlgorithm algorithm, CoseEllipticCurve curve, ImmutableArray<byte> x)
            : base(CoseKeyType.Okp, algorithm)
        {
            int keyLength;
            switch (algorithm)
            {
                case CoseAlgorithm.EdDsa:
                    break;
                default:
                    throw new ArgumentException("Invalid algorithm.", nameof(algorithm));
            }
            switch (curve)
            {
                case CoseEllipticCurve.Ed25519:
                    keyLength = Ed25519.PublicKeyLength;
                    break;
                default:
                    throw new ArgumentException("Invalid curve.", nameof(curve));
            }
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (x.Length != keyLength)
                throw new ArgumentException("Invalid key length.", nameof(x));

            Curve = curve;
            X = x;
        }

        public CoseEllipticCurve Curve { get; }

        public ImmutableArray<byte> X { get; }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public bool VerifyData(ReadOnlySpan<byte> data, CoseEdDsaSignature signature)
        {
            if (signature is null)
                throw new ArgumentNullException(nameof(signature));
            if (signature.Algorithm != Algorithm || signature.Curve != Curve)
                throw new ArgumentException("Incompatible signature.", nameof(signature));

            return Ed25519.VerifyData(X.AsSpan(), data, signature.RS.AsSpan());
        }
    }
}
