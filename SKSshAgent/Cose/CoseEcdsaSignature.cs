// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;

namespace SKSshAgent.Cose;

internal sealed class CoseEcdsaSignature : CoseSignature
{
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public CoseEcdsaSignature(CoseAlgorithm algorithm, CoseEllipticCurve curve, ImmutableArray<byte> r, ImmutableArray<byte> s)
        : base(algorithm)
    {
        switch (algorithm)
        {
            case CoseAlgorithm.ES256:
            case CoseAlgorithm.ES384:
            case CoseAlgorithm.ES512:
                break;
            default:
                throw new ArgumentException("Invalid algorithm.", nameof(algorithm));
        }
        switch (curve)
        {
            case CoseEllipticCurve.P256:
            case CoseEllipticCurve.P384:
            case CoseEllipticCurve.P521:
                break;
            default:
                throw new ArgumentException("Invalid curve.", nameof(curve));
        }
        int fieldSizeBits = curve.GetFieldSizeBits();
        int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);
        if (r == null)
            throw new ArgumentNullException(nameof(r));
        if (r.Length != fieldElementLength || MPInt.GetBitLength(r.AsSpan()) > fieldSizeBits)
            throw new ArgumentException("Invalid EC field element.", nameof(r));
        if (s == null)
            throw new ArgumentNullException(nameof(s));
        if (s.Length != fieldElementLength || MPInt.GetBitLength(s.AsSpan()) > fieldSizeBits)
            throw new ArgumentException("Invalid EC field element.", nameof(s));

        Curve = curve;
        R = r;
        S = s;
    }

    public CoseEllipticCurve Curve { get; }

    public ImmutableArray<byte> R { get; }

    public ImmutableArray<byte> S { get; }
}
