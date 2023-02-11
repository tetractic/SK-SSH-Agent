// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Text;
using Xunit;

namespace SKSshAgent.Cryptography.Tests
{
    public static partial class Ed25519Tests
    {
        [Fact]
        public static void VerifyData_ValidSignature_ReturnsTrue()
        {
            byte[] sk = new byte[Ed25519.SecretKeyLength];
            byte[] pk = new byte[Ed25519.PublicKeyLength];
            Ed25519.GenerateKey(pk, sk);

            Span<byte> data = Encoding.UTF8.GetBytes("This is a test.");

            byte[] signature = new byte[Ed25519.SignatureLength];
            Ed25519.SignData(sk, data, signature);

            bool result = Ed25519.VerifyData(pk, data, signature);

            Assert.True(result);
        }

        [Fact]
        public static void VerifyData_InvalidSignature_ReturnsFalse()
        {
            for (int i = 0; i < Ed25519.SignatureLength * 8; ++i)
            {
                byte[] sk = new byte[Ed25519.SecretKeyLength];
                byte[] pk = new byte[Ed25519.PublicKeyLength];
                Ed25519.GenerateKey(pk, sk);

                Span<byte> data = Encoding.UTF8.GetBytes("This is a test.");

                byte[] signature = new byte[Ed25519.SignatureLength];
                Ed25519.SignData(sk, data, signature);
                signature[i / 8] ^= (byte)(1 << (i % 8));

                bool result = Ed25519.VerifyData(pk, data, signature);

                Assert.False(result);
            }
        }
    }
}
