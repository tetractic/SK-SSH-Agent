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
    public static class SshEd25519KeyTests
    {
        private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

        public static readonly TheoryData<TestCase> OpenSshGeneratedKeyTestCases = new()
        {
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAMwAAAAtzc2gtZW
QyNTUxOQAAACAgU8wLbSsINTUDsQUbz//tScxHWb13cGDsFUFSPZmm3gAAAJgu2KKWLtii
lgAAAAtzc2gtZWQyNTUxOQAAACAgU8wLbSsINTUDsQUbz//tScxHWb13cGDsFUFSPZmm3g
AAAEDlV7Iuj1m8e88uu7ngi98z34rrWCKEsJJ9eVELXOiP9iBTzAttKwg1NQOxBRvP/+1J
zEdZvXdwYOwVQVI9mabeAAAAEGNhcmxAZXhhbXBsZS5jb20BAgMEBQ==
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAICBTzAttKwg1NQOxBRvP/+1JzEdZvXdwYOwVQVI9mabe carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.Ed25519,
                KeyPK = Convert.FromHexString("2053CC0B6D2B08353503B1051BCFFFED49CC4759BD777060EC1541523D99A6DE"),
                KeySK = Convert.FromHexString("E557B22E8F59BC7BCF2EBBB9E08BDF33DF8AEB582284B0927D79510B5CE88FF62053CC0B6D2B08353503B1051BCFFFED49CC4759BD777060EC1541523D99A6DE"),
                Comment = "carl@example.com",
                SignatureRS = Convert.FromHexString("74B292974D3507DAD4CDAAF6DC7B13301A21C4484FA77BA42EB3938A19FF2A6B5E371D3BB57ACF1586E04FEDD923FF1A4FE4BC31FA7B93C099725232E53C6409"),
            },
        };

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void ParseOpenSshPrivateKey_OpenSshGenerated_HasExpectedKeyTypeInfo(TestCase testCase)
        {
            var privateKey = SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyTypeInfo, privateKey.KeyTypeInfo);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void ParseOpenSshPrivateKey_OpenSshGenerated_HasExpectedParameters(TestCase testCase)
        {
            var privateKey = (SshEd25519Key)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyPK, privateKey.PK.ToArray());
            using (var skUnshieldScope = privateKey.SK.Unshield())
                Assert.Equal(testCase.KeySK, skUnshieldScope.UnshieldedSpan.ToArray());
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void FormatOpenSshPublicKey_Always_ReturnsExpectedResult(TestCase testCase)
        {
            var key = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray());

            string publicKeyFile = new(key.FormatOpenSshPublicKey(testCase.Comment));

            Assert.Equal(testCase.PublicKeyFile, publicKeyFile);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void FormatOpenSshPrivateKey_Always_CanBeParsedBackToOriginalKey(TestCase testCase)
        {
            char[] formattedKey = testCase.PrivateKey.FormatOpenSshPrivateKey(testCase.Comment);

            (var parsedKey, string parsedComment) = SshKey.ParseOpenSshPrivateKey(formattedKey);

            Assert.True(testCase.PrivateKey.Equals(parsedKey, publicOnly: false));
            Assert.Equal(testCase.Comment, parsedComment);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void GetOpenSshKeyAuthorization_Always_ReturnsExpectedResult(TestCase testCase)
        {
            string keyAuthorization = testCase.PublicKey.GetOpenSshKeyAuthorization(testCase.Comment);

            Assert.Equal(testCase.KeyAuthorization, keyAuthorization);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void Sign_Always_ProducesExpectedSignature(TestCase testCase)
        {
            var privateKey = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray(), ShieldedImmutableBuffer.Create(testCase.KeySK));

            var signature = privateKey.Sign(_testData);

            Assert.Equal(testCase.SignatureRS, signature.RS);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
        {
            var key = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray());
            var signature = new SshEd25519Signature(testCase.KeyTypeInfo, testCase.SignatureRS.ToImmutableArray());

            bool verified = key.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
        public static void Verify_InvalidSignature_ReturnsFalse(TestCase testCase)
        {
            var key = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray());
            byte[] rs = (byte[])testCase.SignatureRS.Clone();
            rs[1] ^= 1;
            var signature = new SshEd25519Signature(testCase.KeyTypeInfo, rs.ToImmutableArray());

            bool verified = key.Verify(_testData, signature);

            Assert.False(verified);
        }

        public struct TestCase
        {
            public string PrivateKeyFile;
            public string PublicKeyFile;
            internal SshKeyTypeInfo KeyTypeInfo;
            public byte[] KeyPK;
            public byte[] KeySK;
            public string Comment;
            public byte[] SignatureRS;

            private SshEd25519Key _publicKey;
            private SshEd25519Key _privateKey;

            public string KeyAuthorization => PublicKeyFile;

            internal SshEd25519Key PublicKey => _publicKey ??= new SshEd25519Key(
                KeyTypeInfo,
                pk: KeyPK.ToImmutableArray());

            internal SshEd25519Key PrivateKey => _privateKey ??= new SshEd25519Key(
                KeyTypeInfo,
                pk: KeyPK.ToImmutableArray(),
                sk: ShieldedImmutableBuffer.Create(KeySK));
        }
    }
}
