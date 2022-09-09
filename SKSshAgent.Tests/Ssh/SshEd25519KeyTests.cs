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

        public static readonly TheoryData<TestCase> TestCases = new()
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
                KeyPK = Convert.FromBase64String("IFPMC20rCDU1A7EFG8//7UnMR1m9d3Bg7BVBUj2Zpt4="),
                KeySK = Convert.FromBase64String("5VeyLo9ZvHvPLru54IvfM9+K61gihLCSfXlRC1zoj/YgU8wLbSsINTUDsQUbz//tScxHWb13cGDsFUFSPZmm3g=="),
                SignatureRS = Convert.FromBase64String("dLKSl001B9rUzar23HsTMBohxEhPp3ukLrOTihn/KmteNx07tXrPFYbgT+3ZI/8aT+S8Mfp7k8CZclIy5TxkCQ=="),
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
            var privateKey = (SshEd25519Key)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyPK, privateKey.PK.ToArray());
            using (var skUnshieldScope = privateKey.SK.Unshield())
                Assert.Equal(testCase.KeySK, skUnshieldScope.UnshieldedSpan.ToArray());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void FormatOpenSshPublicKey_Always_ReturnsExpectedValue(TestCase testCase)
        {
            var key = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray());

            string publicKeyFile = new(key.FormatOpenSshPublicKey("carl@example.com"));

            Assert.Equal(testCase.PublicKeyFile, publicKeyFile);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Sign_Always_ProducesExpectedSignature(TestCase testCase)
        {
            var privateKey = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray(), ShieldedImmutableBuffer.Create(testCase.KeySK));

            var signature = privateKey.Sign(_testData);

            Assert.Equal(testCase.SignatureRS, signature.RS);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
        {
            var key = new SshEd25519Key(testCase.KeyTypeInfo, testCase.KeyPK.ToImmutableArray());
            var signature = new SshEd25519Signature(testCase.KeyTypeInfo, testCase.SignatureRS.ToImmutableArray());

            bool verified = key.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
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
            public byte[] SignatureRS;
        }
    }
}
