// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace SKSshAgent.Ssh
{
    internal sealed class SshKdfInfo
    {
        public static readonly SshKdfInfo None = new(
            name: "none");

        public static readonly SshKdfInfo Bcrypt = new(
            name: "bcrypt");

        public static readonly ImmutableArray<SshKdfInfo> KdfInfos = ImmutableArray.Create(new[]
        {
            None,
            Bcrypt,
        });

        private static uint _defaultBcryptRounds;

        private SshKdfInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public static bool TryGetKdfInfoByName(string name, [MaybeNullWhen(false)] out SshKdfInfo kdfInfo)
        {
            foreach (var entry in KdfInfos)
            {
                if (entry.Name == name)
                {
                    kdfInfo = entry;
                    return true;
                }
            }

            kdfInfo = default;
            return false;
        }

        public static uint GetDefaultBcryptRounds()
        {
            if (_defaultBcryptRounds == 0)
                _defaultBcryptRounds = Math.Max(16, BcryptPbkdf.BenchmarkRoundsPerSecond() / 4);

            return _defaultBcryptRounds;
        }
    }
}
