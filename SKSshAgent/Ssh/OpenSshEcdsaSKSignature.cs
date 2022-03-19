// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Diagnostics;
using System.Numerics;

namespace SKSshAgent.Ssh
{
    // https://github.com/openssh/openssh-portable/blob/master/PROTOCOL.u2f
    // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-ecdsa-sk.c#L178
    internal sealed class OpenSshEcdsaSKSignature : SshSignature
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public OpenSshEcdsaSKSignature(SshKeyTypeInfo keyTypeInfo, BigInteger r, BigInteger s, byte flags, uint counter)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.Type != SshKeyType.OpenSshEcdsaSK)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (r < 0 || r.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(r));
            if (s < 0 || s.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(s));

            R = r;
            S = s;
            Flags = flags;
            Counter = counter;
        }

        public BigInteger R { get; }

        public BigInteger S { get; }

        public byte Flags { get; }

        public uint Counter { get; }

        public override void WriteTo(ref SshWireWriter writer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L199-L213
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L692
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/ssh-sk.c#L560

            writer.WriteString(KeyTypeInfo.Name);
            WriteEcdsaSignature(ref writer, R, S);
            writer.WriteByte(Flags);
            writer.WriteUInt32(Counter);
        }

        private static void WriteEcdsaSignature(ref SshWireWriter writer, BigInteger r, BigInteger s)
        {
            int rLength = r == 0 ? 0 : r.GetByteCount();
            int sLength = s == 0 ? 0 : s.GetByteCount();

            byte[] signatureBuffer = new byte[4 + rLength + 4 + sLength];

            var signatureWriter = new SshWireWriter(signatureBuffer);
            signatureWriter.WriteBigInteger(r);
            signatureWriter.WriteBigInteger(s);
            signatureWriter.Flush();
            Debug.Assert(signatureWriter.BufferedLength == signatureBuffer.Length);

            writer.WriteByteString(signatureBuffer);
        }
    }
}
