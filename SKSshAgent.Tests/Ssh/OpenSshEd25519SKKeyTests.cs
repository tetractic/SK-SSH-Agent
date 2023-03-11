// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace SKSshAgent.Ssh.Tests
{
    public static class OpenSshEd25519SKKeyTests
    {
        private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

        public static readonly TheoryData<TestCase> TestCases = new()
        {
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAASgAAABpzay1zc2
gtZWQyNTUxOUBvcGVuc3NoLmNvbQAAACD6yjd2c0+Iq26bMmudfMo1sCiR2+emc2sH94fZ
rmwTmgAAAARzc2g6AAAA8HC1CiJwtQoiAAAAGnNrLXNzaC1lZDI1NTE5QG9wZW5zc2guY2
9tAAAAIPrKN3ZzT4irbpsya518yjWwKJHb56Zzawf3h9mubBOaAAAABHNzaDoBAAAAgKuE
E4rQ0mkN4ZtUz2yWk3bDiAs19+4UrRBQuX3ZCMPXLuaQRtTO2Kw4YKqUY07cXJg76x1rnM
QDtqjNnX7dMcri0FFgx8As2nH2DflLMi7j2y0VVFoePrmQrYltMKepnrbzO8Eyk9gLQcgg
KwBW05kT/3EbQT5UwHTDH+Qq86g4AAAAAAAAABBjYXJsQGV4YW1wbGUuY29tAQ==
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "sk-ssh-ed25519@openssh.com AAAAGnNrLXNzaC1lZDI1NTE5QG9wZW5zc2guY29tAAAAIPrKN3ZzT4irbpsya518yjWwKJHb56Zzawf3h9mubBOaAAAABHNzaDo= carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.OpenSshSKEd25519,
                KeyPK = Convert.FromHexString("FACA3776734F88AB6E9B326B9D7CCA35B02891DBE7A6736B07F787D9AE6C139A"),
                KeyApplication = Convert.FromHexString("7373683A"),
                KeyFlags = OpenSshSKFlags.UserPresenceRequired,
                KeyHandle = Convert.FromHexString("AB84138AD0D2690DE19B54CF6C969376C3880B35F7EE14AD1050B97DD908C3D72EE69046D4CED8AC3860AA94634EDC5C983BEB1D6B9CC403B6A8CD9D7EDD31CAE2D05160C7C02CDA71F60DF94B322EE3DB2D15545A1E3EB990AD896D30A7A99EB6F33BC13293D80B41C8202B0056D39913FF711B413E54C074C31FE42AF3A838"),
                SignatureRS = Convert.FromHexString("519C40B1A586497F0A956F7CB1AFBE382BB17CC456632A2D47EE17B18FAA96167EE75F928D4A9B0963E50B58856519E385C80BEEC7B38B0C573E93E0ACA7BF0D"),
                SignatureFlags = (byte)WebAuthn.WebAuthnAuthenticatorDataFlags.UserPresent,
                SignatureCounter = 10,
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
            var privateKey = (OpenSshEd25519SKKey)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyPK, privateKey.PK.ToArray());
            Assert.Equal(testCase.KeyApplication, privateKey.Application.ToArray());
            Assert.Equal(testCase.KeyFlags, privateKey.Flags);
            using (var keyHandleUnshieldScope = privateKey.KeyHandle.Unshield())
                Assert.Equal(testCase.KeyHandle, keyHandleUnshieldScope.UnshieldedSpan.ToArray());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void FormatOpenSshPublicKey_Always_ReturnsExpectedValue(TestCase testCase)
        {
            var key = new OpenSshEd25519SKKey(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());

            string publicKeyFile = new(key.FormatOpenSshPublicKey("carl@example.com"));

            Assert.Equal(testCase.PublicKeyFile, publicKeyFile);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
        {
            var key = new OpenSshEd25519SKKey(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());
            var signature = new OpenSshEd25519SKSignature(testCase.KeyTypeInfo, testCase.SignatureRS.ToImmutableArray(), testCase.SignatureFlags, testCase.SignatureCounter);

            bool verified = key.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_InvalidSignature_ReturnsFalse(TestCase testCase)
        {
            var key = new OpenSshEd25519SKKey(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());
            byte[] rs = (byte[])testCase.SignatureRS.Clone();
            rs[rs.Length - 1] ^= 1;
            var signature = new OpenSshEd25519SKSignature(testCase.KeyTypeInfo, rs.ToImmutableArray(), testCase.SignatureFlags, testCase.SignatureCounter);

            bool verified = key.Verify(_testData, signature);

            Assert.False(verified);
        }

        public struct TestCase
        {
            public string PrivateKeyFile;
            public string PublicKeyFile;
            internal SshKeyTypeInfo KeyTypeInfo;
            public byte[] KeyPK;
            public byte[] KeyApplication;
            internal OpenSshSKFlags KeyFlags;
            public byte[] KeyHandle;
            public byte[] SignatureRS;
            public byte SignatureFlags;
            public uint SignatureCounter;
        }
    }
}
