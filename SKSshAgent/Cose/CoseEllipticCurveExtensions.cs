// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.Cose
{
    internal static class CoseEllipticCurveExtensions
    {
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static int GetFieldSizeBits(this CoseEllipticCurve curve)
        {
            return curve switch
            {
                CoseEllipticCurve.P256 => 256,
                CoseEllipticCurve.P384 => 384,
                CoseEllipticCurve.P521 => 521,
                _ => throw new ArgumentOutOfRangeException(nameof(curve)),
            };
        }
    }
}
