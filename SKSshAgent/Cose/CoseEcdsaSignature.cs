// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;

namespace SKSshAgent.Cose
{
    internal sealed class CoseEcdsaSignature : CoseSignature
    {
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public CoseEcdsaSignature(CoseAlgorithm algorithm, CoseEllipticCurve curve, ImmutableArray<byte> r, ImmutableArray<byte> s)
            : base(CoseKeyType.EC2, algorithm)
        {
            switch (algorithm)
            {
                case CoseAlgorithm.ES256:
                case CoseAlgorithm.ES384:
                case CoseAlgorithm.ES512:
                    switch (curve)
                    {
                        case CoseEllipticCurve.P256:
                        case CoseEllipticCurve.P384:
                        case CoseEllipticCurve.P521:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(curve));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithm));
            }
            int fieldSizeBits = curve.GetFieldSizeBits();
            int fieldElementLength = Sec1.SizeBitsToLength(fieldSizeBits);
            if (r == null)
                throw new ArgumentNullException(nameof(r));
            if (r.Length != fieldElementLength)
                throw new ArgumentException("Invalid size for EC field element.", nameof(r));
            if (Sec1.GetBitLength(r.AsSpan()) > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(r));
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length != fieldElementLength)
                throw new ArgumentException("Invalid size for EC field element.", nameof(s));
            if (Sec1.GetBitLength(s.AsSpan()) > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(s));

            Curve = curve;
            R = r;
            S = s;
        }

        public CoseEllipticCurve Curve { get; }

        public ImmutableArray<byte> R { get; }

        public ImmutableArray<byte> S { get; }
    }
}
