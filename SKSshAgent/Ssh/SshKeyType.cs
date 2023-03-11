// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent.Ssh
{
    internal enum SshKeyType
    {
        Ecdsa,
        Ed25519,
        OpenSshEcdsaSK,
        OpenSshEd25519SK,
    }
}
