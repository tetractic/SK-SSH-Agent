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
    internal sealed class OpenSshEd25519SKSignature : SshSignature
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public OpenSshEd25519SKSignature(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> rs, byte flags, uint counter)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.KeyType != SshKeyType.OpenSshEd25519SK)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (rs == null)
                throw new ArgumentNullException(nameof(rs));
            if (rs.Length != Ed25519.SignatureLength)
                throw new ArgumentException("Invalid RS length.", nameof(rs));

            RS = rs;
            Flags = flags;
            Counter = counter;
        }

        public ImmutableArray<byte> RS { get; }

        public byte Flags { get; }

        public uint Counter { get; }

        public override void WriteTo(ref SshWireWriter writer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L215-L220
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L692
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L605

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteByteString(RS.AsSpan());
            writer.WriteByte(Flags);
            writer.WriteUInt32(Counter);
        }
    }
}
