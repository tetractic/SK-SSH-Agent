// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using System;
using Xunit;

namespace SKSshAgent.WebAuthn.Tests
{
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
    }
}
