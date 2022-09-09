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
    public static class SshEcdsaKeyTests
    {
        private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

        public static readonly TheoryData<TestCase> TestCases = new()
        {
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAaAAAABNlY2RzYS
1zaGEyLW5pc3RwMjU2AAAACG5pc3RwMjU2AAAAQQRWINLe3i9yMv2GXv8OieA7C02yeWou
gG4EP6Dqs7vwS+GA/8FVTkYN8NzLQO7mRQqS3Vs8qEsdXJMnd8/XheoRAAAAqCAK130gCt
d9AAAAE2VjZHNhLXNoYTItbmlzdHAyNTYAAAAIbmlzdHAyNTYAAABBBFYg0t7eL3Iy/YZe
/w6J4DsLTbJ5ai6AbgQ/oOqzu/BL4YD/wVVORg3w3MtA7uZFCpLdWzyoSx1ckyd3z9eF6h
EAAAAgCGOAYt3j4ImErailmr0XAVmxTpGtQHVE01Gl3/vdqTsAAAAQY2FybEBleGFtcGxl
LmNvbQ==
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "ecdsa-sha2-nistp256 AAAAE2VjZHNhLXNoYTItbmlzdHAyNTYAAAAIbmlzdHAyNTYAAABBBFYg0t7eL3Iy/YZe/w6J4DsLTbJ5ai6AbgQ/oOqzu/BL4YD/wVVORg3w3MtA7uZFCpLdWzyoSx1ckyd3z9eF6hE= carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP256,
                KeyX = Convert.FromBase64String("ViDS3t4vcjL9hl7/DongOwtNsnlqLoBuBD+g6rO78Es="),
                KeyY = Convert.FromBase64String("4YD/wVVORg3w3MtA7uZFCpLdWzyoSx1ckyd3z9eF6hE="),
                KeyD = Convert.FromBase64String("CGOAYt3j4ImErailmr0XAVmxTpGtQHVE01Gl3/vdqTs="),
                SignatureR = Convert.FromBase64String("vd2OIEiADXeuOKRSC22V8sztnjOILKUN7KKh5uR/3G4="),
                SignatureS = Convert.FromBase64String("C619RR3ZcuXiRI2tsL8RZfCp916CxfmuopftSR/I5Lc="),
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAiAAAABNlY2RzYS
1zaGEyLW5pc3RwMzg0AAAACG5pc3RwMzg0AAAAYQQpnYR4NLIXHU8/RfseUSNhqNlwDpzX
55h3Vaq9twrqT1pX7mGWB70SW8inNSp2WTcCSQeoN24DN+bnSzicD0L3cuB7FMHgWV5g8y
LQbXSAnRvlT8rocAVeSw2JczlbVugAAADgppL5cqaS+XIAAAATZWNkc2Etc2hhMi1uaXN0
cDM4NAAAAAhuaXN0cDM4NAAAAGEEKZ2EeDSyFx1PP0X7HlEjYajZcA6c1+eYd1WqvbcK6k
9aV+5hlge9ElvIpzUqdlk3AkkHqDduAzfm50s4nA9C93LgexTB4FleYPMi0G10gJ0b5U/K
6HAFXksNiXM5W1boAAAAMQDlguwtggSU8YdfUHj9aLNJ/6u++A+4hTh57WvrEXgk0MIZMf
IE8+2dpqkkTAyca64AAAAQY2FybEBleGFtcGxlLmNvbQECAwQFBgc=
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "ecdsa-sha2-nistp384 AAAAE2VjZHNhLXNoYTItbmlzdHAzODQAAAAIbmlzdHAzODQAAABhBCmdhHg0shcdTz9F+x5RI2Go2XAOnNfnmHdVqr23CupPWlfuYZYHvRJbyKc1KnZZNwJJB6g3bgM35udLOJwPQvdy4HsUweBZXmDzItBtdICdG+VPyuhwBV5LDYlzOVtW6A== carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP384,
                KeyX = Convert.FromBase64String("KZ2EeDSyFx1PP0X7HlEjYajZcA6c1+eYd1WqvbcK6k9aV+5hlge9ElvIpzUqdlk3"),
                KeyY = Convert.FromBase64String("AkkHqDduAzfm50s4nA9C93LgexTB4FleYPMi0G10gJ0b5U/K6HAFXksNiXM5W1bo"),
                KeyD = Convert.FromBase64String("5YLsLYIElPGHX1B4/WizSf+rvvgPuIU4ee1r6xF4JNDCGTHyBPPtnaapJEwMnGuu"),
                SignatureR = Convert.FromBase64String("Jl22jgvNiExJ018pDVCBjWuXWesHSk9AgkH0vNfNvKSbRS3a7YMAJ+6L7Hry0261"),
                SignatureS = Convert.FromBase64String("9RLVb84Cr869EtGpSNppLoQa00cheNtUsVZ4+NN3yxNgWB+aoDTKudFTnCqwjPp3"),
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAArAAAABNlY2RzYS
1zaGEyLW5pc3RwNTIxAAAACG5pc3RwNTIxAAAAhQQB/bqE/7FBkGdlRxVw8urLk22PiA4+
H/rxq5bbsPVkRjqu2orZOP8pFE5SIxse7tGJE6QBxGL0LsQqMhNmizmqNAEBaS0qzKoaG9
Ve+yn0m/2EtsN28rWBZk+hfXaAeUieqOxOPLmFElB3FQoAJKn8tFpBl2P5oTOOJMGs7X4f
+I1WD60AAAEQSngzgEp4M4AAAAATZWNkc2Etc2hhMi1uaXN0cDUyMQAAAAhuaXN0cDUyMQ
AAAIUEAf26hP+xQZBnZUcVcPLqy5Ntj4gOPh/68auW27D1ZEY6rtqK2Tj/KRROUiMbHu7R
iROkAcRi9C7EKjITZos5qjQBAWktKsyqGhvVXvsp9Jv9hLbDdvK1gWZPoX12gHlInqjsTj
y5hRJQdxUKACSp/LRaQZdj+aEzjiTBrO1+H/iNVg+tAAAAQgE6ks4H545lUJMUfHOg9eod
0JxYSPvGKo0GjxQt5u/+WOPBSxyjw6JaIJVxtSxwCxK55rGO85uXdZCc+FJkfFGT9QAAAB
BjYXJsQGV4YW1wbGUuY29tAQI=
-----END OPENSSH PRIVATE KEY-----
",
                PublicKeyFile = "ecdsa-sha2-nistp521 AAAAE2VjZHNhLXNoYTItbmlzdHA1MjEAAAAIbmlzdHA1MjEAAACFBAH9uoT/sUGQZ2VHFXDy6suTbY+IDj4f+vGrltuw9WRGOq7aitk4/ykUTlIjGx7u0YkTpAHEYvQuxCoyE2aLOao0AQFpLSrMqhob1V77KfSb/YS2w3bytYFmT6F9doB5SJ6o7E48uYUSUHcVCgAkqfy0WkGXY/mhM44kwaztfh/4jVYPrQ== carl@example.com",
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP521,
                KeyX = Convert.FromBase64String("Af26hP+xQZBnZUcVcPLqy5Ntj4gOPh/68auW27D1ZEY6rtqK2Tj/KRROUiMbHu7RiROkAcRi9C7EKjITZos5qjQB"),
                KeyY = Convert.FromBase64String("AWktKsyqGhvVXvsp9Jv9hLbDdvK1gWZPoX12gHlInqjsTjy5hRJQdxUKACSp/LRaQZdj+aEzjiTBrO1+H/iNVg+t"),
                KeyD = Convert.FromBase64String("ATqSzgfnjmVQkxR8c6D16h3QnFhI+8YqjQaPFC3m7/5Y48FLHKPDologlXG1LHALErnmsY7zm5d1kJz4UmR8UZP1"),
                SignatureR = Convert.FromBase64String("AIBpFiQA/1I5v8xcM6VIjML0VtIr77sgjmzU4V5fdr2Odm4+84V9O9FybtoAy1ipw/OItF9zTnln23Jf5dA2z0+z"),
                SignatureS = Convert.FromBase64String("AXK+3C5ske086o1T1dqXItLsUVGe6OXT5YAH7EPoNaAaQhNQCsog9WAcNPQ8CdKyWSk807awhI9BZPjA9IkCBQuD"),
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
            var privateKey = (SshEcdsaKey)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyX, privateKey.X.ToArray());
            Assert.Equal(testCase.KeyY, privateKey.Y.ToArray());
            using (var dUnshieldScope = privateKey.D.Unshield())
                Assert.Equal(testCase.KeyD, dUnshieldScope.UnshieldedSpan.ToArray());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void FormatOpenSshPublicKey_Always_ReturnsExpectedValue(TestCase testCase)
        {
            var key = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray());

            string publicKeyFile = new(key.FormatOpenSshPublicKey("carl@example.com"));

            Assert.Equal(testCase.PublicKeyFile, publicKeyFile);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Sign_Always_CanBeVerified(TestCase testCase)
        {
            var privateKey = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), ShieldedImmutableBuffer.Create(testCase.KeyD));

            var signature = privateKey.Sign(_testData);

            bool verified = privateKey.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
        {
            var key = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray());
            var signature = new SshEcdsaSignature(testCase.KeyTypeInfo, testCase.SignatureR.ToImmutableArray(), testCase.SignatureS.ToImmutableArray());

            bool verified = key.Verify(_testData, signature);

            Assert.True(verified);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void Verify_InvalidSignature_ReturnsFalse(TestCase testCase)
        {
            var key = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray());
            byte[] r = (byte[])testCase.SignatureR.Clone();
            r[r.Length - 1] ^= 1;
            var signature = new SshEcdsaSignature(testCase.KeyTypeInfo, r.ToImmutableArray(), testCase.SignatureS.ToImmutableArray());

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
            public byte[] KeyD;
            public byte[] SignatureR;
            public byte[] SignatureS;
        }
    }
}
