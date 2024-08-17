// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using System;
using System.Collections.Immutable;
using Xunit;

namespace SKSshAgent.WebAuthn.Tests;

public static class WebAuthnSignatureTests
{
    [Fact]
    public static void Parse_ES256P256_ReturnsExpectedResults()
    {
        var publicKey = new CoseEC2Key(
            CoseAlgorithm.ES256,
            CoseEllipticCurve.P256,
            Convert.FromHexString("1D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369"),
            Convert.FromHexString("B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845"));

        byte[] bytes = Convert.FromHexString("304402203D6BE9F978C502E115FADA2AEAFDD6A7AABF92A738EFBE9A3D4E37968E78900E02201CA46336A44774CBE73910D18BD674E1CE71A924E1A6BC7C6FE896C71FF9C33A");

        var signature = WebAuthnSignature.Parse(publicKey, bytes, out int bytesUsed);
        Assert.Equal(bytes.Length, bytesUsed);

        byte[] expectedSignatureR = Convert.FromHexString("3D6BE9F978C502E115FADA2AEAFDD6A7AABF92A738EFBE9A3D4E37968E78900E");
        byte[] expectedSignatureS = Convert.FromHexString("1CA46336A44774CBE73910D18BD674E1CE71A924E1A6BC7C6FE896C71FF9C33A");

        Assert.Equal(publicKey.Algorithm, signature.Algorithm);
        var ecdsaSignature = Assert.IsType<CoseEcdsaSignature>(signature);
        Assert.Equal(publicKey.Curve, ecdsaSignature.Curve);
        Assert.Equal(expectedSignatureR, ecdsaSignature.R);
        Assert.Equal(expectedSignatureS, ecdsaSignature.S);
    }

    [Fact]
    public static void Parse_EdDsaEd25519_ReturnsExpectedResults()
    {
        var publicKey = new CoseOkpKey(
            CoseAlgorithm.EdDsa,
            CoseEllipticCurve.Ed25519,
            Convert.FromHexString("C06157A485F1A9BD95AB458C709092FB51FF3E2AFA463D685CC839B2E334DD14").ToImmutableArray());

        byte[] bytes = Convert.FromHexString("FBCE071B8435ACCBE184CCCEC2DF83BF2295718E5E189C130226FC4BB77ACAB2D9C47DFA755DF556E334E04F793ED5DC07ED48BEBC475D2A7AE6A808F96CB003");

        var signature = WebAuthnSignature.Parse(publicKey, bytes, out int bytesUsed);
        Assert.Equal(bytes.Length, bytesUsed);

        byte[] expectedSignatureRS = Convert.FromHexString("FBCE071B8435ACCBE184CCCEC2DF83BF2295718E5E189C130226FC4BB77ACAB2D9C47DFA755DF556E334E04F793ED5DC07ED48BEBC475D2A7AE6A808F96CB003");

        Assert.Equal(publicKey.Algorithm, signature.Algorithm);
        var edDsaSignature = Assert.IsType<CoseEdDsaSignature>(signature);
        Assert.Equal(publicKey.Curve, edDsaSignature.Curve);
        Assert.Equal(expectedSignatureRS, edDsaSignature.RS);
    }
}
