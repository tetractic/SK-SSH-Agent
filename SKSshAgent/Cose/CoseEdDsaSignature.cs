// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Collections.Immutable;

namespace SKSshAgent.Cose;

internal sealed class CoseEdDsaSignature : CoseSignature
{
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public CoseEdDsaSignature(CoseAlgorithm algorithm, CoseEllipticCurve curve, ImmutableArray<byte> rs)
        : base(algorithm)
    {
        int signatureLength;
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
                signatureLength = Ed25519.SignatureLength;
                break;
            default:
                throw new ArgumentException("Invalid curve.", nameof(curve));
        }
        if (rs == null)
            throw new ArgumentNullException(nameof(rs));
        if (rs.Length != signatureLength)
            throw new ArgumentException("Invalid signature length.", nameof(rs));

        Curve = CoseEllipticCurve.Ed25519;
        RS = rs;
    }

    public CoseEllipticCurve Curve { get; }

    public ImmutableArray<byte> RS { get; }
}
