﻿// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;

namespace SKSshAgent.Ssh
{
    internal sealed class OpenSshEcdsaSKKey : SshKey
    {
        private readonly OpenSshSKFlags _flags;
        private readonly ImmutableArray<byte> _keyHandle;

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public OpenSshEcdsaSKKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ImmutableArray<byte> application)
            : this(keyTypeInfo, x, y, application, hasDecryptedPrivateKey: false)
        {
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        public OpenSshEcdsaSKKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ImmutableArray<byte> application, OpenSshSKFlags flags, ImmutableArray<byte> keyHandle)
            : this(keyTypeInfo, x, y, application, hasDecryptedPrivateKey: true)
        {
            if (keyHandle == null)
                throw new ArgumentNullException(nameof(keyHandle));

            _flags = flags;
            _keyHandle = keyHandle;
        }

        private OpenSshEcdsaSKKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ImmutableArray<byte> application, bool hasDecryptedPrivateKey)
            : base(keyTypeInfo, hasDecryptedPrivateKey)
        {
            if (keyTypeInfo.Type != SshKeyType.OpenSshEcdsaSK)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (x < 0 || x.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(y));
            if (application == null)
                throw new ArgumentNullException(nameof(application));

            X = x;
            Y = y;
            Application = application;
        }

        private OpenSshEcdsaSKKey(OpenSshEcdsaSKKey key, SshEncryptedPrivateKey encryptedPrivateKey)
            : base(key.KeyTypeInfo, encryptedPrivateKey)
        {
            X = key.X;
            Y = key.Y;
            Application = key.Application;
        }

        public BigInteger X { get; }

        public BigInteger Y { get; }

        public ImmutableArray<byte> Application { get; }

        // TODO: Private parts should be protected using CryptProtectMemory.  (Though they are not particularly sensitive for an SK key.)

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public OpenSshSKFlags Flags => GetPrivateKeyField(_flags);

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public ImmutableArray<byte> KeyHandle => GetPrivateKeyField(_keyHandle);

        public override string GetOpenSshKeyAuthorization(string comment)
        {
            string result = base.GetOpenSshKeyAuthorization(comment);

            var options = ImmutableArray<string>.Empty;
            if (HasDecryptedPrivateKey)
            {
                if ((Flags & OpenSshSKFlags.UserPresenceRequired) == 0)
                    options = options.Add("no-touch-required");
                if ((Flags & OpenSshSKFlags.UserVerificationRequired) != 0)
                    options = options.Add("verify-required");
            }

            return options.Length > 0
                ? string.Join(',', options) + " " + result
                : result;
        }

        public override void WritePublicKeyTo(ref SshWireWriter writer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L66-L69
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L881-L894

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            writer.WriteByteString(Application.AsSpan());
        }

        public override void WritePrivateKeyTo(ref SshWireWriter writer)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L73-L79
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3245
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3315-L3324

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            writer.WriteByteString(Application.AsSpan());
            writer.WriteByte((byte)Flags);
            writer.WriteByteString(KeyHandle.AsSpan());
            writer.WriteByteString(Span<byte>.Empty);
        }

        public bool Equals([NotNullWhen(true)] OpenSshEcdsaSKKey? other, bool publicOnly)
        {
            if (other == null || !PublicEquals(other))
                return false;

            if (publicOnly)
                return true;

            return (HasDecryptedPrivateKey == other.HasDecryptedPrivateKey) &&
                   (!HasDecryptedPrivateKey || PrivateEquals(other)) &&
                   EqualityComparer<SshEncryptedPrivateKey>.Default.Equals(EncryptedPrivateKey, other.EncryptedPrivateKey);

            bool PublicEquals(OpenSshEcdsaSKKey other)
            {
                return X == other.X &&
                       Y == other.Y &&
                       Application.SequenceEqual(other.Application);
            }

            bool PrivateEquals(OpenSshEcdsaSKKey other)
            {
                return _flags == other._flags &&
                       _keyHandle.SequenceEqual(other._keyHandle);
            }
        }

        public override bool Equals([NotNullWhen(true)] SshKey? other, bool publicOnly) => Equals(other as OpenSshEcdsaSKKey, publicOnly);

        public override int GetHashCode(bool publicOnly)
        {
            var hashCode = new HashCode();
            hashCode.Add(X);
            hashCode.Add(Y);
            hashCode.AddBytes(Application.AsSpan());
            if (!publicOnly)
            {
                hashCode.Add(_flags);
                hashCode.AddBytes(_keyHandle.AsSpan());
                hashCode.Add(EncryptedPrivateKey);
            }
            return hashCode.ToHashCode();
        }

        protected override SshKey WithEncryptedPrivateKey(SshEncryptedPrivateKey encryptedPrivateKey) => new OpenSshEcdsaSKKey(this, encryptedPrivateKey);

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKKey ReadPublicKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L67-L69
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L2474-L2526

            string curveName = reader.ReadString();
            (var x, var y) = ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);
            var application = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            return new OpenSshEcdsaSKKey(keyTypeInfo, x, y, application.ToImmutableArray());
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKKey ReadPrivateKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L74-L79
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3553-L3584

            string curveName = reader.ReadString();
            (var x, var y) = ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);
            var application = reader.ReadByteString();
            var flags = (OpenSshSKFlags)reader.ReadByte();
            var keyHandle = reader.ReadByteString();
            _ = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            return new OpenSshEcdsaSKKey(keyTypeInfo, x, y, application.ToImmutableArray(), flags, keyHandle.ToImmutableArray());
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        private static (BigInteger X, BigInteger Y) ReadECPoint2(ref SshWireReader reader, int fieldSizeBits)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshbuf-getput-crypto.c#L74
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.4

            int fieldElementLength = Sec1.SizeBitsToLength(fieldSizeBits);

            var ecPointBuffer = reader.ReadByteString();

            int ecPointLength = 1 + 2 * fieldElementLength;

            if (ecPointBuffer.Length != ecPointLength || ecPointBuffer[0] != 4)
                throw new InvalidDataException("Invalid EC point data.");

            var x = Sec1.BytesToFieldElement(ecPointBuffer.Slice(1, fieldElementLength), fieldSizeBits);
            var y = Sec1.BytesToFieldElement(ecPointBuffer.Slice(1 + fieldElementLength, fieldElementLength), fieldSizeBits);
            return (x, y);
        }

        private static void WriteECPoint2(ref SshWireWriter writer, int fieldSizeBits, BigInteger x, BigInteger y)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshbuf-getput-crypto.c#L154
            // https://www.secg.org/sec1-v2.pdf#subsubsection.2.3.3

            int fieldElementLength = Sec1.SizeBitsToLength(fieldSizeBits);

            int ecPointLength = 1 + 2 * fieldElementLength;

            byte[] ecPointBuffer = new byte[ecPointLength];

            ecPointBuffer[0] = 4;
            bool xCompletelyWritten = Sec1.TryWriteFieldElementBytes(x, fieldSizeBits, ecPointBuffer.AsSpan(1), out _);
            Debug.Assert(xCompletelyWritten);
            bool yCompletelyWritten = Sec1.TryWriteFieldElementBytes(y, fieldSizeBits, ecPointBuffer.AsSpan(1 + fieldElementLength), out _);
            Debug.Assert(yCompletelyWritten);

            writer.WriteByteString(ecPointBuffer);
        }

        /// <exception cref="InvalidOperationException"/>
        private T GetPrivateKeyField<T>(in T field)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            return field;
        }
    }
}
