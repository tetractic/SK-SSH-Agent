// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;
using Xunit;

namespace SKSshAgent.Ssh.Tests
{
    public static class SshKeyTests
    {
        [Fact]
        public static void GetOpenSshKeyAuthorization_Always_ReturnsExpectedResult()
        {
            var key = new OpenSshEcdsaSKKey(
                SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256,
                Convert.FromBase64String("2NdExbdf5VZvvlG6HOdmw6b5k2u25khPvxoLQtqowok=").ToImmutableArray(),
                Convert.FromBase64String("F9uph+kjsf9IhFrn49QRzLubevhdGv4/4q+nmpOV+6E=").ToImmutableArray(),
                application: Convert.FromBase64String("c3NoOg==").ToImmutableArray(),
                flags: OpenSshSKFlags.UserPresenceRequired,
                keyHandle: ShieldedImmutableBuffer.Create(Convert.FromBase64String("xe8nnmSTqdDaRSKcNNYFPEnJlL59hZ+3KQHHfPmisvUEJS1RS99rGWLIi2xMbK1q")));
            const string comment = "test";

            string keyAuthorization = key.GetOpenSshKeyAuthorization(comment);

            Assert.Equal("sk-ecdsa-sha2-nistp256@openssh.com AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBNjXRMW3X+VWb75RuhznZsOm+ZNrtuZIT78aC0LaqMKJF9uph+kjsf9IhFrn49QRzLubevhdGv4/4q+nmpOV+6EAAAAEc3NoOg== test", keyAuthorization);
        }

        [Fact]
        public static void FormatOpenSshPrivateKey_Always_CanBeParsedBackToOriginalKey()
        {
            var key = new OpenSshEcdsaSKKey(
                SshKeyTypeInfo.OpenSshSKEcdsaSha2NistP256,
                Convert.FromBase64String("2NdExbdf5VZvvlG6HOdmw6b5k2u25khPvxoLQtqowok=").ToImmutableArray(),
                Convert.FromBase64String("F9uph+kjsf9IhFrn49QRzLubevhdGv4/4q+nmpOV+6E=").ToImmutableArray(),
                application: Convert.FromBase64String("c3NoOg==").ToImmutableArray(),
                flags: OpenSshSKFlags.UserPresenceRequired,
                keyHandle: ShieldedImmutableBuffer.Create(Convert.FromBase64String("xe8nnmSTqdDaRSKcNNYFPEnJlL59hZ+3KQHHfPmisvUEJS1RS99rGWLIi2xMbK1q")));
            const string comment = "test";

            char[] formattedKey = key.FormatOpenSshPrivateKey(comment);

            (var parsedKey, string parsedComment) = SshKey.ParseOpenSshPrivateKey(formattedKey);

            Assert.True(key.Equals(parsedKey, publicOnly: false));
            Assert.Equal(comment, parsedComment);
        }
    }
}
