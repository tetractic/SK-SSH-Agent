// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Collections.Immutable;

namespace SKSshAgent.Ssh;

internal sealed class SshEd25519Signature : SshSignature
{
    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public SshEd25519Signature(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> rs)
        : base(keyTypeInfo)
    {
        if (keyTypeInfo.KeyType != SshKeyType.Ed25519)
            throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
        if (rs == null)
            throw new ArgumentNullException(nameof(rs));
        if (rs.Length != Ed25519.SignatureLength)
            throw new ArgumentException("Invalid RS length.", nameof(rs));

        RS = rs;
    }

    public ImmutableArray<byte> RS { get; }

    public override void WriteTo(ref SshWireWriter writer)
    {
        // https://datatracker.ietf.org/doc/html/rfc8709#section-6

        writer.WriteString(KeyTypeInfo.Name);
        writer.WriteByteString(RS.AsSpan());
    }
}
