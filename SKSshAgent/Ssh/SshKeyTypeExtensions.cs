// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.Ssh
{
    internal static class SshKeyTypeExtensions
    {
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static string GetDefaultFileName(this SshKeyType keyType)
        {
            return keyType switch
            {
                SshKeyType.Ecdsa => "id_ecdsa",
                SshKeyType.OpenSshEcdsaSK => "id_ecdsa_sk",
                _ => throw new ArgumentOutOfRangeException(nameof(keyType)),
            };
        }
    }
}
