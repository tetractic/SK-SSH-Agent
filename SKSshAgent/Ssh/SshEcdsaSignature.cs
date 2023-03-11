// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Immutable;

namespace SKSshAgent.Ssh
{
    internal sealed class SshEcdsaSignature : SshSignature
    {
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public SshEcdsaSignature(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> r, ImmutableArray<byte> s)
            : base(keyTypeInfo)
        {
            if (keyTypeInfo.Type != SshKeyType.Ecdsa)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            int fieldSizeBits = keyTypeInfo.KeySizeBits;
            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);
            if (r == null)
                throw new ArgumentNullException(nameof(r));
            if (r.Length != fieldElementLength || MPInt.GetBitLength(r.AsSpan()) > fieldSizeBits)
                throw new ArgumentException("Invalid EC field element.", nameof(r));
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (s.Length != fieldElementLength || MPInt.GetBitLength(s.AsSpan()) > fieldSizeBits)
                throw new ArgumentException("Invalid EC field element.", nameof(s));

            R = r;
            S = s;
        }

        public ImmutableArray<byte> R { get; }

        public ImmutableArray<byte> S { get; }

        public override void WriteTo(ref SshWireWriter writer)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1.2

            writer.WriteString(KeyTypeInfo.Name);
            WriteEcdsaSignature(ref writer, R, S);
        }

        internal static void WriteEcdsaSignature(ref SshWireWriter writer, ImmutableArray<byte> r, ImmutableArray<byte> s)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1.2

            byte[] signatureBuffer = new byte[4 + 1 + r.Length + 4 + 1 + s.Length];

            var signatureWriter = new SshWireWriter(signatureBuffer);
            SshEC.WriteFieldElementAsMPInt(ref signatureWriter, r.AsSpan());
            SshEC.WriteFieldElementAsMPInt(ref signatureWriter, s.AsSpan());
            signatureWriter.Flush();

            writer.WriteByteString(signatureBuffer.AsSpan(0, signatureWriter.BufferedLength));
        }
    }
}
