// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Ssh;
using System;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace SKSshAgent.Tests.Ssh
{
    public static class OpenSshEcdsaSKKeyTests
    {
        private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

        public static readonly TheoryData<TestCase> TestCases = new()
        {
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAfwAAACJzay1lY2
RzYS1zaGEyLW5pc3RwMjU2QG9wZW5zc2guY29tAAAACG5pc3RwMjU2AAAAQQQYseJGSXsU
/OpWury3byC2mn2f1LeaFD/u6XH7xRgOAU9fbFa+budMYSpvUOZRh/VIyfAYvFqbgoMAJv
sQbAS/AAAABHNzaDoAAADYyqVrCcqlawkAAAAic2stZWNkc2Etc2hhMi1uaXN0cDI1NkBv
cGVuc3NoLmNvbQAAAAhuaXN0cDI1NgAAAEEEGLHiRkl7FPzqVrq8t28gtpp9n9S3mhQ/7u
lx+8UYDgFPX2xWvm7nTGEqb1DmUYf1SMnwGLxam4KDACb7EGwEvwAAAARzc2g6AQAAADDX
fpqIuj84e2ePdKANw8/HLgMXBUdcR33lourkTG/9Tosj4aVDDvhYiD9ZdhuzqGQAAAAAAA
AAEGNhcmxAZXhhbXBsZS5jb20BAgME
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "sk-ecdsa-sha2-nistp256@openssh.com AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcfvFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOg== carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256,
                KeyX = Convert.FromBase64String("GLHiRkl7FPzqVrq8t28gtpp9n9S3mhQ/7ulx+8UYDgE="),
                KeyY = Convert.FromBase64String("T19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8="),
                KeyApplication = Convert.FromBase64String("c3NoOg=="),
                KeyFlags = OpenSshSKFlags.UserPresenceRequired,
                KeyHandle = Convert.FromBase64String("136aiLo/OHtnj3SgDcPPxy4DFwVHXEd95aLq5Exv/U6LI+GlQw74WIg/WXYbs6hk"),
                SignatureR = Convert.FromBase64String("KGe+dFY8K+5ihlZzwdQm34T5Covn/BHlEkaGIpOR7dM="),
                SignatureS = Convert.FromBase64String("QY4SVpb0H5Jwdy6GO6lfZR4UDd8CBi37VMU4sPiWBq0="),
                SignatureFlags = (byte)WebAuthn.WebAuthnAuthenticatorDataFlags.UserPresent,
                SignatureCounter = 100,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void ParseOpenSshPrivateKey_OpenSshGenerated_HasExpectedKeyTypeInfo(TestCase testCase)
        {
            var privateKey = SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyTypeInfo, privateKey.KeyTypeInfo);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void ParseOpenSshPrivateKey_OpenSshGenerated_HasExpectedParameters(TestCase testCase)
        {
            var privateKey = (OpenSshEcdsaSKKey)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyX, privateKey.X.ToArray());
            Assert.Equal(testCase.KeyY, privateKey.Y.ToArray());
            Assert.Equal(testCase.KeyApplication, privateKey.Application.ToArray());
            Assert.Equal(testCase.KeyFlags, privateKey.Flags);
            using (var keyHandleUnshieldScope = privateKey.KeyHandle.Unshield())
                Assert.Equal(testCase.KeyHandle, keyHandleUnshieldScope.UnshieldedSpan.ToArray());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void FormatOpenSshPublicKey_Always_ReturnsExpectedValue(TestCase testCase)
        {
            var key = new OpenSshEcdsaSKKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());

            string publicKeyFile = new(key.FormatOpenSshPublicKey("carl@example.com"));

            Assert.Equal(testCase.PublicKeyFile, publicKeyFile);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
        {
            var key = new OpenSshEcdsaSKKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());
            var signature = new OpenSshEcdsaSKSignature(testCase.KeyTypeInfo, testCase.SignatureR.ToImmutableArray(), testCase.SignatureS.ToImmutableArray(), testCase.SignatureFlags, testCase.SignatureCounter);

            bool verified = key.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_InvalidSignature_ReturnsFalse(TestCase testCase)
        {
            var key = new OpenSshEcdsaSKKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());
            byte[] r = (byte[])testCase.SignatureR.Clone();
            r[r.Length - 1] ^= 1;
            var signature = new OpenSshEcdsaSKSignature(testCase.KeyTypeInfo, r.ToImmutableArray(), testCase.SignatureS.ToImmutableArray(), testCase.SignatureFlags, testCase.SignatureCounter);

            bool verified = key.Verify(_testData, signature);

            Assert.False(verified);
        }

        public struct TestCase
        {
            public string PrivateKeyFile;
            public string PublicKeyFile;
            internal SshKeyTypeInfo KeyTypeInfo;
            public byte[] KeyX;
            public byte[] KeyY;
            public byte[] KeyApplication;
            internal OpenSshSKFlags KeyFlags;
            public byte[] KeyHandle;
            public byte[] SignatureR;
            public byte[] SignatureS;
            public byte SignatureFlags;
            public uint SignatureCounter;
        }
    }
}
