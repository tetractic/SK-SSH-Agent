// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace SKSshAgent;

internal static class KeyConversionExtensions
{
    internal static readonly Dictionary<SshKeyTypeInfo, (CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve)> OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo = new()
    {
        [SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256] = (CoseKeyType.EC2, CoseAlgorithm.ES256, CoseEllipticCurve.P256),
        [SshKeyTypeInfo.OpenSshSKEd25519] = (CoseKeyType.Okp, CoseAlgorithm.EdDsa, CoseEllipticCurve.Ed25519),
    };

    internal static readonly Dictionary<(CoseKeyType KeyType, CoseAlgorithm Algorithm, CoseEllipticCurve Curve), SshKeyTypeInfo> WebAuthnEllipticCurveInfoToOpenSshKeyInfoName = InvertDictionary(OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo);

    /// <exception cref="NotSupportedException"/>
    internal static CoseKey ToWebAuthnKey(this SshKey key)
    {
        switch (key.KeyTypeInfo.KeyType)
        {
            case SshKeyType.OpenSshEcdsaSK:
            {
                var ecdsaSKKey = (OpenSshEcdsaSKKey)key;

                return ToWebAuthnKey(ecdsaSKKey);
            }
            case SshKeyType.OpenSshEd25519SK:
            {
                var ed25519SKKey = (OpenSshEd25519SKKey)key;

                return ToWebAuthnKey(ed25519SKKey);
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
            x: ecdsaSKKey.X,
            y: ecdsaSKKey.Y);
    }

    /// <exception cref="NotSupportedException"/>
    internal static CoseOkpKey ToWebAuthnKey(this OpenSshEd25519SKKey ed25519SKKey)
    {
        var keyTypeInfo = ed25519SKKey.KeyTypeInfo;

        if (!OpenSshKeyInfoNameToWebAuthnEllipticCurveInfo.TryGetValue(keyTypeInfo, out var info))
            throw new NotSupportedException("Unsupported key type.");

        Debug.Assert(info.KeyType == CoseKeyType.Okp);

        return new CoseOkpKey(
            algorithm: info.Algorithm,
            curve: info.Curve,
            x: ed25519SKKey.PK);
    }

    /// <exception cref="NotSupportedException"/>
    internal static OpenSshEd25519SKKey ToOpenSshKey(this CoseOkpKey okpPublicKey, ImmutableArray<byte> application, OpenSshSKFlags flags, ImmutableArray<byte> keyHandle)
    {
        var info = (okpPublicKey.KeyType, okpPublicKey.Algorithm, okpPublicKey.Curve);
        if (!WebAuthnEllipticCurveInfoToOpenSshKeyInfoName.TryGetValue(info, out var keyTypeInfo))
            throw new NotSupportedException("Unsupported key type parameters.");

        Debug.Assert(keyTypeInfo.KeyType == SshKeyType.OpenSshEd25519SK);

        return new OpenSshEd25519SKKey(
            keyTypeInfo: keyTypeInfo,
            pk: okpPublicKey.X,
            application: application,
            flags: flags,
            keyHandle: ShieldedImmutableBuffer.Create(keyHandle.AsSpan()));
    }

    /// <exception cref="NotSupportedException"/>
    internal static OpenSshEcdsaSKKey ToOpenSshKey(this CoseEC2Key ec2PublicKey, ImmutableArray<byte> application, OpenSshSKFlags flags, ImmutableArray<byte> keyHandle)
    {
        var info = (ec2PublicKey.KeyType, ec2PublicKey.Algorithm, ec2PublicKey.Curve);
        if (!WebAuthnEllipticCurveInfoToOpenSshKeyInfoName.TryGetValue(info, out var keyTypeInfo))
            throw new NotSupportedException("Unsupported key type parameters.");

        Debug.Assert(keyTypeInfo.KeyType == SshKeyType.OpenSshEcdsaSK);

        return new OpenSshEcdsaSKKey(
            keyTypeInfo: keyTypeInfo,
            x: ec2PublicKey.X,
            y: ec2PublicKey.Y,
            application: application,
            flags: flags,
            keyHandle: ShieldedImmutableBuffer.Create(keyHandle.AsSpan()));
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
