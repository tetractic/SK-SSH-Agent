// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.Ssh;

internal static class SshKeyTypeExtensions
{
    /// <exception cref="ArgumentException"/>
    public static string GetDefaultFileName(this SshKeyType keyType)
    {
        return keyType switch
        {
            SshKeyType.Ecdsa => "id_ecdsa",
            SshKeyType.Ed25519 => "id_ed25519",
            SshKeyType.OpenSshEcdsaSK => "id_ecdsa_sk",
            SshKeyType.OpenSshEd25519SK => "id_ed25519_sk",
            _ => throw new ArgumentException("Invalid key type.", nameof(keyType)),
        };
    }
}
