// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SKSshAgent
{
    internal static class SignatureConversionExtensions
    {
        internal static Dictionary<SshKeyTypeInfo, (CoseAlgorithm Algorithm, CoseEllipticCurve Curve)> OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo = new()
        {
            [SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256] = (CoseAlgorithm.ES256, CoseEllipticCurve.P256),
        };

        internal static Dictionary<(CoseAlgorithm Algorithm, CoseEllipticCurve Curve), SshKeyTypeInfo> WebAuthnEllipticCurveInfoToOpenSshKeyInfoName => InvertDictionary(OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo);

        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKSignature ToOpenSshSignature(this CoseEcdsaSignature ecdsaSignature, byte flags, uint counter)
        {
            var info = (ecdsaSignature.Algorithm, ecdsaSignature.Curve);
            if (!WebAuthnEllipticCurveInfoToOpenSshKeyInfoName.TryGetValue(info, out var keyTypeInfo))
                throw new NotSupportedException("Unsupported signature type parameters.");

            Debug.Assert(keyTypeInfo.Type == SshKeyType.OpenSshEcdsaSK);

            return new OpenSshEcdsaSKSignature(
                keyTypeInfo: keyTypeInfo,
                r: ecdsaSignature.R,
                s: ecdsaSignature.S,
                flags: flags,
                counter: counter);
        }

        private static Dictionary<TValue, TKey> InvertDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
            where TKey : notnull
            where TValue : notnull
        {
            var inverted = new Dictionary<TValue, TKey>();
            foreach (var entry in dictionary)
                inverted.Add(entry.Value, entry.Key);
            return inverted;
        }
    }
}
