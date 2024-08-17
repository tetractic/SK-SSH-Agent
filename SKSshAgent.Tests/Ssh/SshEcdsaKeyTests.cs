// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using System.Text;
using Xunit;

namespace SKSshAgent.Ssh.Tests;

public static class SshEcdsaKeyTests
{
    private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

    public static readonly TheoryData<TestCase> OpenSshGeneratedKeyTestCases = new()
    {
        new()
        {
            PrivateKeyFile =
                """
                -----BEGIN OPENSSH PRIVATE KEY-----
                b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAaAAAABNlY2RzYS
                1zaGEyLW5pc3RwMjU2AAAACG5pc3RwMjU2AAAAQQRWINLe3i9yMv2GXv8OieA7C02yeWou
                gG4EP6Dqs7vwS+GA/8FVTkYN8NzLQO7mRQqS3Vs8qEsdXJMnd8/XheoRAAAAqCAK130gCt
                d9AAAAE2VjZHNhLXNoYTItbmlzdHAyNTYAAAAIbmlzdHAyNTYAAABBBFYg0t7eL3Iy/YZe
                /w6J4DsLTbJ5ai6AbgQ/oOqzu/BL4YD/wVVORg3w3MtA7uZFCpLdWzyoSx1ckyd3z9eF6h
                EAAAAgCGOAYt3j4ImErailmr0XAVmxTpGtQHVE01Gl3/vdqTsAAAAQY2FybEBleGFtcGxl
                LmNvbQ==
                -----END OPENSSH PRIVATE KEY-----

                """,
            PublicKeyFile = "ecdsa-sha2-nistp256 AAAAE2VjZHNhLXNoYTItbmlzdHAyNTYAAAAIbmlzdHAyNTYAAABBBFYg0t7eL3Iy/YZe/w6J4DsLTbJ5ai6AbgQ/oOqzu/BL4YD/wVVORg3w3MtA7uZFCpLdWzyoSx1ckyd3z9eF6hE= carl@example.com",
            KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP256,
            KeyX = Convert.FromHexString("5620D2DEDE2F7232FD865EFF0E89E03B0B4DB2796A2E806E043FA0EAB3BBF04B"),
            KeyY = Convert.FromHexString("E180FFC1554E460DF0DCCB40EEE6450A92DD5B3CA84B1D5C932777CFD785EA11"),
            KeyD = Convert.FromHexString("08638062DDE3E08984ADA8A59ABD170159B14E91AD407544D351A5DFFBDDA93B"),
            Comment = "carl@example.com",
            SignatureR = Convert.FromHexString("BDDD8E2048800D77AE38A4520B6D95F2CCED9E33882CA50DECA2A1E6E47FDC6E"),
            SignatureS = Convert.FromHexString("0BAD7D451DD972E5E2448DADB0BF1165F0A9F75E82C5F9AEA297ED491FC8E4B7"),
        },
        new()
        {
            PrivateKeyFile =
                """
                -----BEGIN OPENSSH PRIVATE KEY-----
                b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAiAAAABNlY2RzYS
                1zaGEyLW5pc3RwMzg0AAAACG5pc3RwMzg0AAAAYQQpnYR4NLIXHU8/RfseUSNhqNlwDpzX
                55h3Vaq9twrqT1pX7mGWB70SW8inNSp2WTcCSQeoN24DN+bnSzicD0L3cuB7FMHgWV5g8y
                LQbXSAnRvlT8rocAVeSw2JczlbVugAAADgppL5cqaS+XIAAAATZWNkc2Etc2hhMi1uaXN0
                cDM4NAAAAAhuaXN0cDM4NAAAAGEEKZ2EeDSyFx1PP0X7HlEjYajZcA6c1+eYd1WqvbcK6k
                9aV+5hlge9ElvIpzUqdlk3AkkHqDduAzfm50s4nA9C93LgexTB4FleYPMi0G10gJ0b5U/K
                6HAFXksNiXM5W1boAAAAMQDlguwtggSU8YdfUHj9aLNJ/6u++A+4hTh57WvrEXgk0MIZMf
                IE8+2dpqkkTAyca64AAAAQY2FybEBleGFtcGxlLmNvbQECAwQFBgc=
                -----END OPENSSH PRIVATE KEY-----

                """,
            PublicKeyFile = "ecdsa-sha2-nistp384 AAAAE2VjZHNhLXNoYTItbmlzdHAzODQAAAAIbmlzdHAzODQAAABhBCmdhHg0shcdTz9F+x5RI2Go2XAOnNfnmHdVqr23CupPWlfuYZYHvRJbyKc1KnZZNwJJB6g3bgM35udLOJwPQvdy4HsUweBZXmDzItBtdICdG+VPyuhwBV5LDYlzOVtW6A== carl@example.com",
            KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP384,
            KeyX = Convert.FromHexString("299D847834B2171D4F3F45FB1E512361A8D9700E9CD7E7987755AABDB70AEA4F5A57EE619607BD125BC8A7352A765937"),
            KeyY = Convert.FromHexString("024907A8376E0337E6E74B389C0F42F772E07B14C1E0595E60F322D06D74809D1BE54FCAE870055E4B0D8973395B56E8"),
            KeyD = Convert.FromHexString("E582EC2D820494F1875F5078FD68B349FFABBEF80FB8853879ED6BEB117824D0C21931F204F3ED9DA6A9244C0C9C6BAE"),
            Comment = "carl@example.com",
            SignatureR = Convert.FromHexString("265DB68E0BCD884C49D35F290D50818D6B9759EB074A4F408241F4BCD7CDBCA49B452DDAED830027EE8BEC7AF2D36EB5"),
            SignatureS = Convert.FromHexString("F512D56FCE02AFCEBD12D1A948DA692E841AD3472178DB54B15678F8D377CB1360581F9AA034CAB9D1539C2AB08CFA77"),
        },
        new()
        {
            PrivateKeyFile =
                """
                -----BEGIN OPENSSH PRIVATE KEY-----
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

                """,
            PublicKeyFile = "ecdsa-sha2-nistp521 AAAAE2VjZHNhLXNoYTItbmlzdHA1MjEAAAAIbmlzdHA1MjEAAACFBAH9uoT/sUGQZ2VHFXDy6suTbY+IDj4f+vGrltuw9WRGOq7aitk4/ykUTlIjGx7u0YkTpAHEYvQuxCoyE2aLOao0AQFpLSrMqhob1V77KfSb/YS2w3bytYFmT6F9doB5SJ6o7E48uYUSUHcVCgAkqfy0WkGXY/mhM44kwaztfh/4jVYPrQ== carl@example.com",
            KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP521,
            KeyX = Convert.FromHexString("01FDBA84FFB141906765471570F2EACB936D8F880E3E1FFAF1AB96DBB0F564463AAEDA8AD938FF29144E52231B1EEED18913A401C462F42EC42A3213668B39AA3401"),
            KeyY = Convert.FromHexString("01692D2ACCAA1A1BD55EFB29F49BFD84B6C376F2B581664FA17D768079489EA8EC4E3CB985125077150A0024A9FCB45A419763F9A1338E24C1ACED7E1FF88D560FAD"),
            KeyD = Convert.FromHexString("013A92CE07E78E655093147C73A0F5EA1DD09C5848FBC62A8D068F142DE6EFFE58E3C14B1CA3C3A25A209571B52C700B12B9E6B18EF39B9775909CF852647C5193F5"),
            Comment = "carl@example.com",
            SignatureR = Convert.FromHexString("008069162400FF5239BFCC5C33A5488CC2F456D22BEFBB208E6CD4E15E5F76BD8E766E3EF3857D3BD1726EDA00CB58A9C3F388B45F734E7967DB725FE5D036CF4FB3"),
            SignatureS = Convert.FromHexString("0172BEDC2E6C91ED3CEA8D53D5DA9722D2EC51519EE8E5D3E58007EC43E835A01A4213500ACA20F5601C34F43C09D2B259293CD3B6B0848F4164F8C0F48902050B83"),
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
        var privateKey = (SshEcdsaKey)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

        Assert.Equal(testCase.KeyX, privateKey.X.ToArray());
        Assert.Equal(testCase.KeyY, privateKey.Y.ToArray());
        using (var dUnshieldScope = privateKey.D.Unshield())
            Assert.Equal(testCase.KeyD, dUnshieldScope.UnshieldedSpan.ToArray());
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
    public static void FormatOpenSshPublicKey_Always_ReturnsExpectedResult(TestCase testCase)
    {
        var key = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray());

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
    public static void Sign_Always_CanBeVerified(TestCase testCase)
    {
        var privateKey = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), ShieldedImmutableBuffer.Create(testCase.KeyD));

        var signature = privateKey.Sign(_testData);

        bool verified = privateKey.Verify(_testData, signature);

        Assert.True(verified);
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
    public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
    {
        var key = new SshEcdsaKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray());
        var signature = new SshEcdsaSignature(testCase.KeyTypeInfo, testCase.SignatureR.ToImmutableArray(), testCase.SignatureS.ToImmutableArray());

        bool verified = key.Verify(_testData, signature);

        Assert.True(verified);
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
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
        public string Comment;
        public byte[] SignatureR;
        public byte[] SignatureS;

        private SshEcdsaKey _publicKey;
        private SshEcdsaKey _privateKey;

        public string KeyAuthorization => PublicKeyFile;

        internal SshEcdsaKey PublicKey => _publicKey ??= new SshEcdsaKey(
            KeyTypeInfo,
            x: KeyX.ToImmutableArray(),
            y: KeyY.ToImmutableArray());

        internal SshEcdsaKey PrivateKey => _privateKey ??= new SshEcdsaKey(
            KeyTypeInfo,
            x: KeyX.ToImmutableArray(),
            y: KeyY.ToImmutableArray(),
            d: ShieldedImmutableBuffer.Create(KeyD));
    }
}
