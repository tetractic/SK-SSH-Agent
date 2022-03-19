// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Numerics;
using Xunit;

namespace SKSshAgent.Ssh.Tests
{
    public static class SshKeyTests
    {
        [Fact]
        public static void GetOpenSshKeyAuthorization_Always_ReturnsExpectedResult()
        {
            var key = new OpenSshEcdsaSKKey(
                SshKeyTypeInfo.SKEcdsaSha2NistP256KeyType,
                BigInteger.Parse("98079922062085566346477182080616450541634317430015585846706072972772164289161"),
                BigInteger.Parse("10791305083879448925504919972385807166056302535066617515522020748831529237409"),
                application: Convert.FromBase64String("c3NoOg==").ToImmutableArray(),
                flags: OpenSshSKFlags.UserPresenceRequired,
                keyHandle: Convert.FromBase64String("xe8nnmSTqdDaRSKcNNYFPEnJlL59hZ+3KQHHfPmisvUEJS1RS99rGWLIi2xMbK1q").ToImmutableArray());
            const string comment = "test";

            string keyAuthorization = key.GetOpenSshKeyAuthorization(comment);

            Assert.Equal("sk-ecdsa-sha2-nistp256@openssh.com AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBNjXRMW3X+VWb75RuhznZsOm+ZNrtuZIT78aC0LaqMKJF9uph+kjsf9IhFrn49QRzLubevhdGv4/4q+nmpOV+6EAAAAEc3NoOg== test", keyAuthorization);
        }

        [Fact]
        public static void ParseOpenSshPrivateKey_ValidInput_ReturnsExpectedResult()
        {
            const string openSshPrivateKey =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAfwAAACJzay1lY2
RzYS1zaGEyLW5pc3RwMjU2QG9wZW5zc2guY29tAAAACG5pc3RwMjU2AAAAQQTY10TFt1/l
Vm++Uboc52bDpvmTa7bmSE+/GgtC2qjCiRfbqYfpI7H/SIRa5+PUEcy7m3r4XRr+P+Kvp5
qTlfuhAAAABHNzaDoAAADYPXixvD14sbwAAAAic2stZWNkc2Etc2hhMi1uaXN0cDI1NkBv
cGVuc3NoLmNvbQAAAAhuaXN0cDI1NgAAAEEE2NdExbdf5VZvvlG6HOdmw6b5k2u25khPvx
oLQtqowokX26mH6SOx/0iEWufj1BHMu5t6+F0a/j/ir6eak5X7oQAAAARzc2g6AQAAADDF
7yeeZJOp0NpFIpw01gU8ScmUvn2Fn7cpAcd8+aKy9QQlLVFL32sZYsiLbExsrWoAAAAAAA
AAEGNhcmxAZXhhbXBsZS5jb20BAgME
-----END OPENSSH PRIVATE KEY-----
";

            (var key, string comment) = SshKey.ParseOpenSshPrivateKey(openSshPrivateKey);

            Assert.Equal("sk-ecdsa-sha2-nistp256@openssh.com", key.KeyTypeInfo.Name);
            Assert.Equal("carl@example.com", comment);
        }

        [Fact]
        public static void FormatOpenSshPrivateKey_Always_CanBeParsedBackToOriginalKey()
        {
            var key = new OpenSshEcdsaSKKey(
                SshKeyTypeInfo.SKEcdsaSha2NistP256KeyType,
                BigInteger.Parse("98079922062085566346477182080616450541634317430015585846706072972772164289161"),
                BigInteger.Parse("10791305083879448925504919972385807166056302535066617515522020748831529237409"),
                application: Convert.FromBase64String("c3NoOg==").ToImmutableArray(),
                flags: OpenSshSKFlags.UserPresenceRequired,
                keyHandle: Convert.FromBase64String("xe8nnmSTqdDaRSKcNNYFPEnJlL59hZ+3KQHHfPmisvUEJS1RS99rGWLIi2xMbK1q").ToImmutableArray());
            const string comment = "test";

            char[] formattedKey = key.FormatOpenSshPrivateKey(comment);

            (var parsedKey, string parsedComment) = SshKey.ParseOpenSshPrivateKey(formattedKey);

            Assert.True(key.Equals(parsedKey, publicOnly: false));
            Assert.Equal(comment, parsedComment);
        }
    }
}
