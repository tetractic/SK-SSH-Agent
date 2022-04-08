// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;

namespace SKSshAgent.Ssh
{
    // https://github.com/openssh/openssh-portable/blob/master/PROTOCOL.u2f
    // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-ecdsa-sk.c#L178
    internal sealed class OpenSshEcdsaSKSignature : SshSignature
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public OpenSshEcdsaSKSignature(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> r, ImmutableArray<byte> s, byte flags, uint counter)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.Type != SshKeyType.OpenSshEcdsaSK)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            int fieldSizeBits = keyTypeInfo.KeySizeBits;
            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);
            if (r == null)
                throw new ArgumentNullException(nameof(r));
            if (r.Length != fieldElementLength || MPInt.GetBitLength(r.AsSpan()) > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(r));
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length != fieldElementLength || MPInt.GetBitLength(s.AsSpan()) > fieldSizeBits)
                throw new ArgumentOutOfRangeException(nameof(s));

            R = r;
            S = s;
            Flags = flags;
            Counter = counter;
        }

        public ImmutableArray<byte> R { get; }

        public ImmutableArray<byte> S { get; }

        public byte Flags { get; }

        public uint Counter { get; }

        public override void WriteTo(ref SshWireWriter writer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L199-L213
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L692
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L560

            writer.WriteString(KeyTypeInfo.Name);
            SshEcdsaSignature.WriteEcdsaSignature(ref writer, R, S);
            writer.WriteByte(Flags);
            writer.WriteUInt32(Counter);
        }
    }
}
