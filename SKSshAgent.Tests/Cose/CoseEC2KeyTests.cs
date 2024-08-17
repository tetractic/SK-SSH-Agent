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

public static class CoseEC2KeyTests
{
    private static readonly byte[] _challenge = Encoding.UTF8.GetBytes("This is a test.");

    [Fact]
    public static void VerifyData_ValidSignature_ReturnsTrue()
    {
        var publicKey = new CoseEC2Key(
            algorithm: CoseAlgorithm.ES256,
            curve: CoseEllipticCurve.P256,
            x: Convert.FromHexString("1D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369"),
            y: Convert.FromHexString("B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845"));

        byte[] authenticatorDataBytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF0500000075");

        var signature = new CoseEcdsaSignature(
            algorithm: CoseAlgorithm.ES256,
            curve: CoseEllipticCurve.P256,
            r: Convert.FromHexString("3D6BE9F978C502E115FADA2AEAFDD6A7AABF92A738EFBE9A3D4E37968E78900E").ToImmutableArray(),
            s: Convert.FromHexString("1CA46336A44774CBE73910D18BD674E1CE71A924E1A6BC7C6FE896C71FF9C33A").ToImmutableArray());

        byte[] signedData = WebAuthnSignature.GetSignedData(_challenge, authenticatorDataBytes);

        bool verified = publicKey.VerifyData(signedData, signature);

        Assert.True(verified);
    }
}
