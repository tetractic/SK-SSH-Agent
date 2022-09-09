// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SKSshAgent.Ssh
{
    internal sealed class SshCipherInfo
    {
        public static readonly SshCipherInfo None = new(
            name: "none",
            blockLength: 8,
            keyLength: 0,
            ivLength: 0);

        public static readonly SshCipherInfo Aes128Cbc = new(
            name: "aes128-cbc",
            blockLength: 16,
            keyLength: 16,
            ivLength: 16);

        public static readonly SshCipherInfo Aes192Cbc = new(
            name: "aes192-cbc",
            blockLength: 16,
            keyLength: 24,
            ivLength: 16);

        public static readonly SshCipherInfo Aes256Cbc = new(
            name: "aes256-cbc",
            blockLength: 16,
            keyLength: 32,
            ivLength: 16);

        public static readonly SshCipherInfo Aes128Ctr = new(
            name: "aes128-ctr",
            blockLength: 16,
            keyLength: 16,
            ivLength: 16);

        public static readonly SshCipherInfo Aes192Ctr = new(
            name: "aes192-ctr",
            blockLength: 16,
            keyLength: 24,
            ivLength: 16);

        public static readonly SshCipherInfo Aes256Ctr = new(
            name: "aes256-ctr",
            blockLength: 16,
            keyLength: 32,
            ivLength: 16);

        public static readonly SshCipherInfo OpenSshAes128Gcm = new(
            name: "aes128-gcm@openssh.com",
            blockLength: 16,
            keyLength: 16,
            ivLength: 12,
            tagLength: 16);

        public static readonly SshCipherInfo OpenSshAes256Gcm = new(
            name: "aes256-gcm@openssh.com",
            blockLength: 16,
            keyLength: 32,
            ivLength: 12,
            tagLength: 16);

        public static readonly ImmutableArray<SshCipherInfo> CipherInfos = GetSupportedCipherInfos();

        private static ImmutableArray<SshCipherInfo> GetSupportedCipherInfos()
        {
            var builder = ImmutableArray.CreateBuilder<SshCipherInfo>();
            builder.Add(None);
            builder.Add(Aes128Cbc);
            builder.Add(Aes192Cbc);
            builder.Add(Aes256Cbc);
            builder.Add(Aes128Ctr);
            builder.Add(Aes192Ctr);
            builder.Add(Aes256Ctr);
            if (OpenSshAesGcmCipher.IsSupported)
            {
                builder.Add(OpenSshAes128Gcm);
                builder.Add(OpenSshAes256Gcm);
            }
            return builder.ToImmutable();
        }

        private SshCipherInfo(string name, int blockLength, int keyLength, int ivLength, int tagLength = 0)
        {
            Name = name;
            BlockLength = blockLength;
            KeyLength = keyLength;
            IVLength = ivLength;
            TagLength = tagLength;
        }

        public string Name { get; }

        public int BlockLength { get; }

        public int KeyLength { get; }

        public int IVLength { get; }

        public int TagLength { get; }

        public static bool TryGetCipherInfoByName(string name, [MaybeNullWhen(false)] out SshCipherInfo cipherInfo)
        {
            foreach (var entry in CipherInfos)
            {
                if (entry.Name == name)
                {
                    cipherInfo = entry;
                    return true;
                }
            }

            cipherInfo = default;
            return false;
        }
    }
}
