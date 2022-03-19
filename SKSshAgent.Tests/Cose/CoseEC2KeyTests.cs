// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.WebAuthn;
using System;
using Xunit;

namespace SKSshAgent.Cose.Tests
{
    public static class CoseEC2KeyTests
    {
        [Fact]
        public static void VerifyData_ValidSignature_ReturnsTrue()
        {
            // TODO: Simplify.

            byte[] registerAuthenticatorDataBytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF45000000159C835346796B4C278898D6032F515CC50030B25B4B0956812591375FF00932A63B89ADC52187526438B224B8F23BABE1E228F432A3D976305CCE8A37A1AEA67AD14FA50102032620012158201D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369225820B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845");
            var registerAuthenticatorData = WebAuthnAuthenticatorData.Parse(registerAuthenticatorDataBytes, out _);
            var credentialPublicKey = (CoseEC2Key)registerAuthenticatorData.AttestedCredentialData.CredentialPublicKey;

            byte[] challenge = Convert.FromHexString("A5907C0C1D4EE0BA460D79F2CE6428C589F375AA7E3AF7276CF4157750B02AB6A6B012FF54C3AA0FD49DD97B923F8DDA2826099BBE5DE12B94E323A367966A8D");
            byte[] authenticateAuthenticatorDataBytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF010000001C");
            byte[] signedData = WebAuthnSignature.GetSignedData(challenge, authenticateAuthenticatorDataBytes);
            byte[] signatureBytes = Convert.FromHexString("304502205E561F80ADBDB233BF1553EE645278F35379922945F17EDC359873084BA60CC5022100AC00CD08408A71941F0AF3F3E9FC48FED4559BB7B138D5F28047AC15236255F5");
            var signature = WebAuthnSignature.ParseEcdsaAsn1(CoseAlgorithm.ES256, CoseEllipticCurve.P256, signatureBytes, out _);

            bool verified = credentialPublicKey.VerifyData(signedData, signature);

            Assert.True(verified);
        }
    }
}
