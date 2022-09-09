// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.WebAuthn
{
    /// <seealso href="https://www.w3.org/TR/webauthn/#sctn-authenticator-data"/>
    [Flags]
    internal enum WebAuthnAuthenticatorDataFlags : byte
    {
        None = 0,
        UserPresent = 0x01,
        UserVerified = 0x04,
        AttestedCredentialDataIncluded = 0x40,
        ExtensionsIncluded = 0x80,
    }
}
