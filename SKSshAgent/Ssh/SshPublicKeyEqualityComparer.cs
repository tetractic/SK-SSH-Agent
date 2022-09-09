// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SKSshAgent.Ssh
{
    internal sealed class SshPublicKeyEqualityComparer : IEqualityComparer<SshKey>
    {
        public static readonly SshPublicKeyEqualityComparer Instance = new();

        private SshPublicKeyEqualityComparer()
        {
        }

        public bool Equals(SshKey? x, SshKey? y)
        {
            return x == null ? y == null : x.Equals(y, publicOnly: true);
        }

        public int GetHashCode([DisallowNull] SshKey obj)
        {
            return obj.GetHashCode(publicOnly: true);
        }
    }
}
