// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using Xunit;

namespace SKSshAgent.Ssh.Tests
{
    public static class SshEcdsaKeyTests
    {
        public static readonly TheoryData<ParseOpenSshPrivateKey_OpenSshGenerated_TestCase> ParseOpenSshPrivateKey_OpenSshGeneratedEncrypted_Data = new()
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
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP256,
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
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP384,
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
                KeyTypeInfo = SshKeyTypeInfo.EcdsaSha2NistP521,
            },
        };

        [Theory]
        [MemberData(nameof(ParseOpenSshPrivateKey_OpenSshGeneratedEncrypted_Data))]
        public static void ParseOpenSshPrivateKey_OpenSshGenerated_HasExpectedKeyTypeInfo(ParseOpenSshPrivateKey_OpenSshGenerated_TestCase testCase)
        {
            var privateKey = SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile).Key;

            Assert.Equal(testCase.KeyTypeInfo, privateKey.KeyTypeInfo);
        }

        public struct ParseOpenSshPrivateKey_OpenSshGenerated_TestCase
        {
            public string PrivateKeyFile;
            internal SshKeyTypeInfo KeyTypeInfo;
        }
    }
}
