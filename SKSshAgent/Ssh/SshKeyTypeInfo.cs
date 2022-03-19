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
        public static readonly SshKeyTypeInfo SKEcdsaSha2NistP256KeyType = new(
            name: "sk-ecdsa-sha2-nistp256@openssh.com",
            type: SshKeyType.OpenSshEcdsaSK,
            hashAlgorithmName: HashAlgorithmName.SHA256,
            curveName: "nistp256",
            curve: ECCurve.NamedCurves.nistP256,
            keySizeBits: 256);

        public static ImmutableArray<SshKeyTypeInfo> KeyTypeInfos = ImmutableArray.Create(new[]
        {
            SKEcdsaSha2NistP256KeyType,
        });

        internal readonly ECCurve Curve;
        internal readonly int KeySizeBits;

        internal SshKeyTypeInfo(string name, SshKeyType type, HashAlgorithmName hashAlgorithmName, string? curveName, ECCurve curve, int keySizeBits)
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

        internal static bool TryGetKeyTypeInfoByName(string name, [MaybeNullWhen(false)] out SshKeyTypeInfo keyTypeInfo)
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
