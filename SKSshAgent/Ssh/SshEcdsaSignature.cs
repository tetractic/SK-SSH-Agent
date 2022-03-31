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
    internal sealed class SshEcdsaSignature : SshSignature
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public SshEcdsaSignature(SshKeyTypeInfo keyTypeInfo, BigInteger r, BigInteger s)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.Type != SshKeyType.Ecdsa)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (r < 0 || r.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(r));
            if (s < 0 || s.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(s));

            R = r;
            S = s;
        }

        public BigInteger R { get; }

        public BigInteger S { get; }

        public override void WriteTo(ref SshWireWriter writer)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1.2

            writer.WriteString(KeyTypeInfo.Name);
            WriteEcdsaSignature(ref writer, R, S);
        }

        internal static void WriteEcdsaSignature(ref SshWireWriter writer, BigInteger r, BigInteger s)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1.2

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
