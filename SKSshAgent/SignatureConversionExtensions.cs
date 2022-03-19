// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace SKSshAgent
{
    internal static class SignatureConversionExtensions
    {
        internal static Dictionary<SshKeyTypeInfo, (CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve)> OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo => KeyConversionExtensions.OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo;

        internal static Dictionary<(CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve), SshKeyTypeInfo> WebAuthnEllipticCurveInfoToOpenSshKeyInfoName => KeyConversionExtensions.WebAuthnEllipticCurveInfoToOpenSshKeyInfoName;

        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKSignature ToOpenSshSignature(this CoseEcdsaSignature ecdsaSignature, byte flags, uint counter)
        {
            var info = (ecdsaSignature.KeyType, ecdsaSignature.Algorithm, ecdsaSignature.Curve);
            if (!WebAuthnEllipticCurveInfoToOpenSshKeyInfoName.TryGetValue(info, out var keyTypeInfo))
                throw new NotSupportedException("Unsupported signature type parameters.");

            Debug.Assert(keyTypeInfo.Type == SshKeyType.OpenSshEcdsaSK);

            return new OpenSshEcdsaSKSignature(
                keyTypeInfo: keyTypeInfo,
                r: new BigInteger(ecdsaSignature.R.AsSpan(), isUnsigned: true, isBigEndian: true),
                s: new BigInteger(ecdsaSignature.S.AsSpan(), isUnsigned: true, isBigEndian: true),
                flags: flags,
                counter: counter);
        }
    }
}
