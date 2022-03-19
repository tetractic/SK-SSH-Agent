// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SKSshAgent
{
    internal static class KeyConversionExtensions
    {
        internal static readonly Dictionary<SshKeyTypeInfo, (CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve)> OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo = new()
        {
            [SshKeyTypeInfo.SKEcdsaSha2NistP256KeyType] = (CoseKeyType.EC2, CoseAlgorithm.ES256, CoseEllipticCurve.P256),
        };

        internal static readonly Dictionary<(CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve), SshKeyTypeInfo> WebAuthnEllipticCurveInfoToOpenSshKeyInfoName = InvertDictionary(OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo);

        /// <exception cref="NotSupportedException"/>
        internal static CoseKey ToWebAuthnKey(this SshKey key)
        {
            switch (key.KeyTypeInfo.Type)
            {
                case SshKeyType.OpenSshEcdsaSK:
                {
                    var ecdsaSKKey = (OpenSshEcdsaSKKey)key;
                    return ToWebAuthnKey(ecdsaSKKey);
                }
                default:
                    throw new NotSupportedException("Unsupported key type.");
            }
        }

        /// <exception cref="NotSupportedException"/>
        internal static CoseEC2Key ToWebAuthnKey(this OpenSshEcdsaSKKey ecdsaSKKey)
        {
            var keyTypeInfo = ecdsaSKKey.KeyTypeInfo;

            if (!OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo.TryGetValue(keyTypeInfo, out var info))
                throw new NotSupportedException("Unsupported key type.");

            Debug.Assert(info.KeyType == CoseKeyType.EC2);

            return new CoseEC2Key(
                algorithm: info.Algorithm,
                curve: info.Curve,
                x: Sec1.FieldElementToImmutableBytes(ecdsaSKKey.X, keyTypeInfo.KeySizeBits),
                y: Sec1.FieldElementToImmutableBytes(ecdsaSKKey.Y, keyTypeInfo.KeySizeBits));
        }

        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKKey ToOpenSshKey(this CoseEC2Key ec2PublicKey, ImmutableArray<byte> application, OpenSshSKFlags flags, ImmutableArray<byte> keyHandle)
        {
            var info = (ec2PublicKey.KeyType, ec2PublicKey.Algorithm, ec2PublicKey.Curve);
            if (!WebAuthnEllipticCurveInfoToOpenSshKeyInfoName.TryGetValue(info, out var keyTypeInfo))
                throw new NotSupportedException("Unsupported key type parameters.");

            Debug.Assert(keyTypeInfo.Type == SshKeyType.OpenSshEcdsaSK);

            return new OpenSshEcdsaSKKey(
                keyTypeInfo: keyTypeInfo,
                x: Sec1.BytesToFieldElement(ec2PublicKey.X.AsSpan(), keyTypeInfo.KeySizeBits),
                y: Sec1.BytesToFieldElement(ec2PublicKey.Y.AsSpan(), keyTypeInfo.KeySizeBits),
                application: application,
                flags: flags,
                keyHandle: keyHandle);
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
