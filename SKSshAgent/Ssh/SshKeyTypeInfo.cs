// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class SshKeyTypeInfo
    {
        public static readonly SshKeyTypeInfo EcdsaSha2NistP256 = new(
            name: "ecdsa-sha2-nistp256",
            type: SshKeyType.Ecdsa,
            hashAlgorithmName: HashAlgorithmName.SHA256,
            curveName: "nistp256",
            curve: ECCurve.NamedCurves.nistP256,
            keySizeBits: 256);

        public static readonly SshKeyTypeInfo EcdsaSha2NistP384 = new (
            name: "ecdsa-sha2-nistp384",
            type: SshKeyType.Ecdsa,
            hashAlgorithmName: HashAlgorithmName.SHA384,
            curveName: "nistp384",
            curve: ECCurve.NamedCurves.nistP384,
            keySizeBits: 384);

        public static readonly SshKeyTypeInfo EcdsaSha2NistP521 = new(
            name: "ecdsa-sha2-nistp521",
            type: SshKeyType.Ecdsa,
            hashAlgorithmName: HashAlgorithmName.SHA512,
            curveName: "nistp521",
            curve: ECCurve.NamedCurves.nistP521,
            keySizeBits: 521);

        public static readonly SshKeyTypeInfo SKEcdsaSha2NistP256 = new(
            name: "sk-ecdsa-sha2-nistp256@openssh.com",
            type: SshKeyType.OpenSshEcdsaSK,
            hashAlgorithmName: HashAlgorithmName.SHA256,
            curveName: "nistp256",
            curve: ECCurve.NamedCurves.nistP256,
            keySizeBits: 256);

        public static readonly ImmutableArray<SshKeyTypeInfo> KeyTypeInfos = ImmutableArray.Create(new[]
        {
            EcdsaSha2NistP256,
            EcdsaSha2NistP384,
            EcdsaSha2NistP521,
            SKEcdsaSha2NistP256,
        });

        internal readonly ECCurve Curve;
        internal readonly int KeySizeBits;

        private SshKeyTypeInfo(string name, SshKeyType type, HashAlgorithmName hashAlgorithmName, string? curveName, ECCurve curve, int keySizeBits)
        {
            Name = name;
            Type = type;
            CurveName = curveName;
            HashAlgorithmName = hashAlgorithmName;
            Curve = curve;
            KeySizeBits = keySizeBits;
        }

        public string Name { get; }

        public SshKeyType Type { get; }

        public HashAlgorithmName HashAlgorithmName { get; }

        public string? CurveName { get; }

        public static bool TryGetKeyTypeInfoByName(string name, [MaybeNullWhen(false)] out SshKeyTypeInfo keyTypeInfo)
        {
            foreach (var entry in KeyTypeInfos)
            {
                if (entry.Name == name)
                {
                    keyTypeInfo = entry;
                    return true;
                }
            }

            keyTypeInfo = default;
            return false;
        }
    }
}
