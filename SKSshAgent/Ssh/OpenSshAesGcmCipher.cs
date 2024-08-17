// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh;

internal sealed class OpenSshAesGcmCipher : IDisposable
{
    public const int BlockLength = 16;

    private readonly AesGcm _aesGcm;

    /// <exception cref="ArgumentException"/>
    public OpenSshAesGcmCipher(ReadOnlySpan<byte> key)
    {
        if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            throw new ArgumentException("Invalid length.", nameof(key));

        _aesGcm = new AesGcm(key);
    }

    public static bool IsSupported = AesGcm.IsSupported;

    public void Dispose()
    {
        _aesGcm.Dispose();
    }

    /// <exception cref="ArgumentException"/>
    /// <exception cref="CryptographicException"/>
    public void Encrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> plaintext, Span<byte> ciphertext, Span<byte> tag)
    {
        _aesGcm.Encrypt(iv, plaintext, ciphertext, tag);
    }

    /// <exception cref="ArgumentException"/>
    /// <exception cref="CryptographicException"/>
    public void Decrypt(ReadOnlySpan<byte> iv, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, Span<byte> plaintext)
    {
        _aesGcm.Decrypt(iv, ciphertext, tag, plaintext);
    }
}
