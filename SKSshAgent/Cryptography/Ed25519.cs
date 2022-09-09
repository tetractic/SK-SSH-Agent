// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using static supercop.crypto_sign.ed25519.ref10.crypto;

namespace SKSshAgent.Cryptography
{
    internal static class Ed25519
    {
        public const int PublicKeyLength = 32;

        public const int SecretKeyLength = 64;

        public const int SignatureLength = 64;

        /// <exception cref="ArgumentException"/>
        public static void GenerateKey(Span<byte> publicKey, Span<byte> secretKey)
        {
            if (publicKey.Length < PublicKeyLength)
                throw new ArgumentException("Insufficient length.", nameof(publicKey));
            if (secretKey.Length < SecretKeyLength)
                throw new ArgumentException("Insufficient length.", nameof(secretKey));

            Span<byte> pk = publicKey.Slice(0, PublicKeyLength);
            Span<byte> sk = secretKey.Slice(0, SecretKeyLength);

            crypto_sign_keypair(pk, sk);

            // Zero the stack.  (Size was determined empirically with a generous margin.)
            ZeroStack(8 * 1024);
        }

        /// <exception cref="ArgumentException"/>
        public static void SignData(ReadOnlySpan<byte> secretKey, ReadOnlySpan<byte> data, Span<byte> signature)
        {
            if (secretKey.Length < SecretKeyLength)
                throw new ArgumentException("Insufficient length.", nameof(secretKey));
            if (signature.Length < SignatureLength)
                throw new ArgumentException("Insufficient length.", nameof(signature));

            ReadOnlySpan<byte> m = data;
            ReadOnlySpan<byte> sk = secretKey.Slice(0, SecretKeyLength);

            byte[] sm = new byte[64 + data.Length];
            unsafe
            {
                fixed (byte* smPtr = sm)
                {
                    ExceptionDispatchInfo? exInfo = null;
                    try
                    {
                        crypto_sign(sm, m, sk);

                        sm.AsSpan(0, SignatureLength).CopyTo(signature);
                    }
                    catch (Exception ex)
                    {
                        exInfo = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        CryptographicOperations.ZeroMemory(sm);
                    }

                    // Zero the stack.  (Size was determined empirically with a generous margin.)
                    ZeroStack(12 * 1024);

                    exInfo?.Throw();
                }
            }
        }

        /// <exception cref="ArgumentException"/>
        public static bool VerifyData(ReadOnlySpan<byte> publicKey, ReadOnlySpan<byte> data, ReadOnlySpan<byte> signature)
        {
            if (publicKey.Length < PublicKeyLength)
                throw new ArgumentException("Insufficient length.", nameof(publicKey));
            if (signature.Length < SignatureLength)
                throw new ArgumentException("Insufficient length.", nameof(signature));

            ReadOnlySpan<byte> pk = publicKey.Slice(0, PublicKeyLength);
            ReadOnlySpan<byte> s = signature.Slice(0, SignatureLength);

            byte[] sm = new byte[64 + data.Length];
            unsafe
            {
                fixed (byte* smPtr = sm)
                {
                    bool result = default;
                    ExceptionDispatchInfo? exInfo = null;
                    try
                    {
                        s.CopyTo(sm);

                        result = crypto_sign_open(data, sm, pk) == 0;
                    }
                    catch (Exception ex)
                    {
                        exInfo = ExceptionDispatchInfo.Capture(ex);
                    }
                    finally
                    {
                        CryptographicOperations.ZeroMemory(sm);
                    }

                    // Zero the stack.  (Size was determined empirically with a generous margin.)
                    ZeroStack(12 * 1024);

                    exInfo?.Throw();

                    return result;
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [SkipLocalsInit]
        private static void ZeroStack(int size)
        {
            Span<byte> stack = stackalloc byte[size];
            CryptographicOperations.ZeroMemory(stack);
        }
    }
}
