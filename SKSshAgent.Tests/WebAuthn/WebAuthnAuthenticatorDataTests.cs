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
    public static class WebAuthnAuthenticatorDataTests
    {
        [Fact]
        public static void Parse_NoKey_ReturnsExpectedResults()
        {
            byte[] bytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF0500000075");

            var authenticatorData = WebAuthnAuthenticatorData.Parse(bytes, out int bytesUsed);
            Assert.Equal(bytes.Length, bytesUsed);

            byte[] expectedRPIdHash = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF");
            var expectedFlags = WebAuthnAuthenticatorDataFlags.UserPresent | WebAuthnAuthenticatorDataFlags.UserVerified;

            Assert.Equal(expectedRPIdHash, authenticatorData.RPIdHash);
            Assert.Equal(expectedFlags, authenticatorData.Flags);
            Assert.Equal(0x00000075U, authenticatorData.SignCount);
            Assert.Null(authenticatorData.AttestedCredentialData);
        }

        [Fact]
        public static void Parse_EC2ES256P256_ReturnsExpectedResults()
        {
            byte[] bytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF45000000159C835346796B4C278898D6032F515CC50030B25B4B0956812591375FF00932A63B89ADC52187526438B224B8F23BABE1E228F432A3D976305CCE8A37A1AEA67AD14FA50102032620012158201D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369225820B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845");

            var authenticatorData = WebAuthnAuthenticatorData.Parse(bytes, out int bytesUsed);
            Assert.Equal(bytes.Length, bytesUsed);

            byte[] expectedRPIdHash = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF");
            var expectedFlags = WebAuthnAuthenticatorDataFlags.UserPresent | WebAuthnAuthenticatorDataFlags.UserVerified | WebAuthnAuthenticatorDataFlags.AttestedCredentialDataIncluded;
            byte[] expectedAAGuid = Convert.FromHexString("9C835346796B4C278898D6032F515CC5");
            byte[] expectedCredentialId = Convert.FromHexString("B25B4B0956812591375FF00932A63B89ADC52187526438B224B8F23BABE1E228F432A3D976305CCE8A37A1AEA67AD14F");
            byte[] expectedCredentialPublicKeyX = Convert.FromHexString("1D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369");
            byte[] expectedCredentialPublicKeyY = Convert.FromHexString("B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845");

            Assert.Equal(expectedRPIdHash, authenticatorData.RPIdHash);
            Assert.Equal(expectedFlags, authenticatorData.Flags);
            Assert.Equal(0x00000015U, authenticatorData.SignCount);
            Assert.NotNull(authenticatorData.AttestedCredentialData);
            Assert.Equal(expectedAAGuid, authenticatorData.AttestedCredentialData.AAGuid);
            Assert.Equal(expectedCredentialId, authenticatorData.AttestedCredentialData.CredentialId);
            Assert.Equal(CoseKeyType.EC2, authenticatorData.AttestedCredentialData.CredentialPublicKey.KeyType);
            Assert.Equal(CoseAlgorithm.ES256, authenticatorData.AttestedCredentialData.CredentialPublicKey.Algorithm);
            var ec2Key = Assert.IsType<CoseEC2Key>(authenticatorData.AttestedCredentialData.CredentialPublicKey);
            Assert.Equal(CoseEllipticCurve.P256, ec2Key.Curve);
            Assert.Equal(expectedCredentialPublicKeyX, ec2Key.X);
            Assert.Equal(expectedCredentialPublicKeyY, ec2Key.Y);
        }
    }
}
