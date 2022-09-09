// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Collections.Immutable;

namespace SKSshAgent.Ssh
{
    internal sealed class SshEd25519Signature : SshSignature
    {
        private const int _rsLength = Ed25519.SignatureLength;

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public SshEd25519Signature(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> rs)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.Type != SshKeyType.Ed25519)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (rs == null)
                throw new ArgumentNullException(nameof(rs));
            if (rs.Length != _rsLength)
                throw new ArgumentOutOfRangeException(nameof(rs));

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
}
