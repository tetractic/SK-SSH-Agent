// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using Xunit;

namespace SKSshAgent.WebAuthn.Tests
{
    public static class WebAuthnAuthenticatorDataTests
    {
        [Fact]
        public static void Test()
        {
            byte[] bytes = Convert.FromHexString("E30610E8A162115960FE1EC223E6529C9F4B6E80200DCB5E5C321C8AF1E2B1BF45000000159C835346796B4C278898D6032F515CC50030B25B4B0956812591375FF00932A63B89ADC52187526438B224B8F23BABE1E228F432A3D976305CCE8A37A1AEA67AD14FA50102032620012158201D15E3709E5FB9F5F7589B77D53C393318EB77D213A1974A36D67EA7E863A369225820B1E54F742081F46A6B8A49545E2AE1721B3CA9612C88CD25012C67B25E3EC845");

            var authenticatorData = WebAuthnAuthenticatorData.Parse(bytes, out int bytesUsed);
            Assert.Equal(bytes.Length, bytesUsed);

            // TODO
        }
    }
}
