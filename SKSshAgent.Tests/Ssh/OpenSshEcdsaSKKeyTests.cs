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

public static class OpenSshEcdsaSKKeyTests
{
    private static readonly byte[] _testData = Encoding.UTF8.GetBytes("This is a test.");

    public static readonly TheoryData<TestCase> OpenSshGeneratedKeyTestCases = new()
    {
        new()
        {
            PrivateKeyFile =
                """
                -----BEGIN OPENSSH PRIVATE KEY-----
                b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAfwAAACJzay1lY2
                RzYS1zaGEyLW5pc3RwMjU2QG9wZW5zc2guY29tAAAACG5pc3RwMjU2AAAAQQQYseJGSXsU
                /OpWury3byC2mn2f1LeaFD/u6XH7xRgOAU9fbFa+budMYSpvUOZRh/VIyfAYvFqbgoMAJv
                sQbAS/AAAABHNzaDoAAADYyqVrCcqlawkAAAAic2stZWNkc2Etc2hhMi1uaXN0cDI1NkBv
                cGVuc3NoLmNvbQAAAAhuaXN0cDI1NgAAAEEEGLHiRkl7FPzqVrq8t28gtpp9n9S3mhQ/7u
                lx+8UYDgFPX2xWvm7nTGEqb1DmUYf1SMnwGLxam4KDACb7EGwEvwAAAARzc2g6AQAAADDX
                fpqIuj84e2ePdKANw8/HLgMXBUdcR33lourkTG/9Tosj4aVDDvhYiD9ZdhuzqGQAAAAAAA
                AAEGNhcmxAZXhhbXBsZS5jb20BAgME
                -----END OPENSSH PRIVATE KEY-----

                """,
            PublicKeyFile = "sk-ecdsa-sha2-nistp256@openssh.com AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcfvFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOg== carl@example.com",
            KeyTypeInfo = SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256,
            KeyX = Convert.FromHexString("18B1E246497B14FCEA56BABCB76F20B69A7D9FD4B79A143FEEE971FBC5180E01"),
            KeyY = Convert.FromHexString("4F5F6C56BE6EE74C612A6F50E65187F548C9F018BC5A9B82830026FB106C04BF"),
            KeyApplication = Convert.FromHexString("7373683A"),
            KeyFlags = OpenSshSKFlags.UserPresenceRequired,
            KeyHandle = Convert.FromHexString("D77E9A88BA3F387B678F74A00DC3CFC72E031705475C477DE5A2EAE44C6FFD4E8B23E1A5430EF858883F59761BB3A864"),
            KeyAuthorizationOptions = null,
            Comment = "carl@example.com",
            SignatureR = Convert.FromHexString("2867BE74563C2BEE62865673C1D426DF84F90A8BE7FC11E5124686229391EDD3"),
            SignatureS = Convert.FromHexString("418E125696F41F9270772E863BA95F651E140DDF02062DFB54C538B0F89606AD"),
            SignatureFlags = (byte)WebAuthn.WebAuthnAuthenticatorDataFlags.UserPresent,
            SignatureCounter = 100,
        },
        new()
        {
            PrivateKeyFile =
                """
                -----BEGIN OPENSSH PRIVATE KEY-----
                b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAfwAAACJzay1lY2
                RzYS1zaGEyLW5pc3RwMjU2QG9wZW5zc2guY29tAAAACG5pc3RwMjU2AAAAQQRiVy98rFhs
                Iht/QX8TP1BKnfkv5XwYlEPz4fA1aOYhnJJmq+MmeV1WYw4mKNi/l0gluuDAQVkzSKGP6m
                avIR4zAAAABHNzaDoAAADoFnXljxZ15Y8AAAAic2stZWNkc2Etc2hhMi1uaXN0cDI1NkBv
                cGVuc3NoLmNvbQAAAAhuaXN0cDI1NgAAAEEEYlcvfKxYbCIbf0F/Ez9QSp35L+V8GJRD8+
                HwNWjmIZySZqvjJnldVmMOJijYv5dIJbrgwEFZM0ihj+pmryEeMwAAAARzc2g6BQAAAEDH
                1PGyaMOsTssCO6mH7MKFkmz37G+DOtuBIGDnILnLmkaKGHFYnL6h4RhRwOd0bf1poJHwWv
                Bi/AA2jcecWZkdAAAAAAAAABBjYXJsQGV4YW1wbGUuY29tAQIDBA==
                -----END OPENSSH PRIVATE KEY-----

                """,
            PublicKeyFile = "sk-ecdsa-sha2-nistp256@openssh.com AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBGJXL3ysWGwiG39BfxM/UEqd+S/lfBiUQ/Ph8DVo5iGckmar4yZ5XVZjDiYo2L+XSCW64MBBWTNIoY/qZq8hHjMAAAAEc3NoOg== carl@example.com",
            KeyTypeInfo = SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256,
            KeyX = Convert.FromHexString("62572F7CAC586C221B7F417F133F504A9DF92FE57C189443F3E1F03568E6219C"),
            KeyY = Convert.FromHexString("9266ABE326795D56630E2628D8BF974825BAE0C041593348A18FEA66AF211E33"),
            KeyApplication = Convert.FromHexString("7373683A"),
            KeyFlags = OpenSshSKFlags.UserPresenceRequired | OpenSshSKFlags.UserVerificationRequired,
            KeyHandle = Convert.FromHexString("C7D4F1B268C3AC4ECB023BA987ECC285926CF7EC6F833ADB812060E720B9CB9A468A1871589CBEA1E11851C0E7746DFD69A091F05AF062FC00368DC79C59991D"),
            KeyAuthorizationOptions = "verify-required",
            Comment = "carl@example.com",
            SignatureR = Convert.FromHexString("C077C3C514A4CB1A504A9781A715500FBF307C2215040591B5EE73367D67D661"),
            SignatureS = Convert.FromHexString("C140E68B5AD5C85FC0B0CED95CF40359262FDE6A5E4EFFD75C5C93F40EB3E9E4"),
            SignatureFlags = (byte)(WebAuthn.WebAuthnAuthenticatorDataFlags.UserPresent | WebAuthn.WebAuthnAuthenticatorDataFlags.UserVerified),
            SignatureCounter = 9,
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
        var privateKey = (OpenSshEcdsaSKKey)SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

        Assert.Equal(testCase.KeyX, privateKey.X.ToArray());
        Assert.Equal(testCase.KeyY, privateKey.Y.ToArray());
        Assert.Equal(testCase.KeyApplication, privateKey.Application.ToArray());
        Assert.Equal(testCase.KeyFlags, privateKey.Flags);
        using (var keyHandleUnshieldScope = privateKey.KeyHandle.Unshield())
            Assert.Equal(testCase.KeyHandle, keyHandleUnshieldScope.UnshieldedSpan.ToArray());
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
    public static void FormatOpenSshPublicKey_Always_ReturnsExpectedResult(TestCase testCase)
    {
        var key = new OpenSshEcdsaSKKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());

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
    public static void GetOpenSshKeyAuthorization_PublicKey_ReturnsExpectedResult(TestCase testCase)
    {
        string keyAuthorization = testCase.PublicKey.GetOpenSshKeyAuthorization(testCase.Comment);

        Assert.Equal(testCase.PublicKeyFile, keyAuthorization);
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
    public static void GetOpenSshKeyAuthorization_PrivateKey_ReturnsExpectedResult(TestCase testCase)
    {
        string keyAuthorization = testCase.PrivateKey.GetOpenSshKeyAuthorization(testCase.Comment);

        Assert.Equal(testCase.KeyAuthorization, keyAuthorization);
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
    public static void Verify_ValidSignature_ReturnsTrue(TestCase testCase)
    {
        var key = new OpenSshEcdsaSKKey(testCase.KeyTypeInfo, testCase.KeyX.ToImmutableArray(), testCase.KeyY.ToImmutableArray(), testCase.KeyApplication.ToImmutableArray());
        var signature = new OpenSshEcdsaSKSignature(testCase.KeyTypeInfo, testCase.SignatureR.ToImmutableArray(), testCase.SignatureS.ToImmutableArray(), testCase.SignatureFlags, testCase.SignatureCounter);

        bool verified = key.Verify(_testData, signature);

        Assert.True(verified);
    }

    [Theory]
    [MemberData(nameof(OpenSshGeneratedKeyTestCases))]
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
        public string? KeyAuthorizationOptions;
        public string Comment;
        public byte[] SignatureR;
        public byte[] SignatureS;
        public byte SignatureFlags;
        public uint SignatureCounter;

        private OpenSshEcdsaSKKey _publicKey;
        private OpenSshEcdsaSKKey _privateKey;

        public string KeyAuthorization => KeyAuthorizationOptions is null ? PublicKeyFile : $"{KeyAuthorizationOptions} {PublicKeyFile}";

        internal OpenSshEcdsaSKKey PublicKey => _publicKey ??= new OpenSshEcdsaSKKey(
            KeyTypeInfo,
            x: KeyX.ToImmutableArray(),
            y: KeyY.ToImmutableArray(),
            application: KeyApplication.ToImmutableArray());

        internal OpenSshEcdsaSKKey PrivateKey => _privateKey ??= new OpenSshEcdsaSKKey(
            KeyTypeInfo,
            x: KeyX.ToImmutableArray(),
            y: KeyY.ToImmutableArray(),
            application: KeyApplication.ToImmutableArray(),
            flags: KeyFlags,
            keyHandle: ShieldedImmutableBuffer.Create(KeyHandle));
    }
}
