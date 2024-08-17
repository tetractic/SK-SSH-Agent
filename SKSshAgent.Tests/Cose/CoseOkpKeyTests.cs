// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.WebAuthn;
using System;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace SKSshAgent.Cose.Tests;

public static class CoseOkpKeyTests
{
    private static readonly byte[] _challenge = Encoding.UTF8.GetBytes("This is a test.");

    [Fact]
    public static void VerifyData_ValidSignature_ReturnsTrue()
    {
        var publicKey = new CoseOkpKey(
            algorithm: CoseAlgorithm.EdDsa,
            curve: CoseEllipticCurve.Ed25519,
            x: Convert.FromHexString("C06157A485F1A9BD95AB458C709092FB51FF3E2AFA463D685CC839B2E334DD14").ToImmutableArray());

        byte[] authenticatorDataBytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF0500000005");

        var signature = new CoseEdDsaSignature(
            algorithm: CoseAlgorithm.EdDsa,
            curve: CoseEllipticCurve.Ed25519,
            rs: Convert.FromHexString("FBCE071B8435ACCBE184CCCEC2DF83BF2295718E5E189C130226FC4BB77ACAB2D9C47DFA755DF556E334E04F793ED5DC07ED48BEBC475D2A7AE6A808F96CB003").ToImmutableArray());

        byte[] signedData = WebAuthnSignature.GetSignedData(_challenge, authenticatorDataBytes);

        bool verified = publicKey.VerifyData(signedData, signature);

        Assert.True(verified);
    }
}
