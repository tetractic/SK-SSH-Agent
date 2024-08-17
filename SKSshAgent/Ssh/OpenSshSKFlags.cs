// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.Ssh;

[Flags]
internal enum OpenSshSKFlags : byte
{
    // Indicates that the security key must attest to user presence.
    UserPresenceRequired = 0x01,

    // Indicates that the security key must attest to user verification.
    UserVerificationRequired = 0x04,

    // Indicates that the private key is completely stored on the security key.
    ResidentKey = 0x20,
}
