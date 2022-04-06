// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class OpenSshEcdsaSKKey : SshKey
    {
        private readonly OpenSshSKFlags _flags;
        private readonly ShieldedImmutableBuffer _keyHandle;

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CryptographicException"/>
        public OpenSshEcdsaSKKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ImmutableArray<byte> application)
            : this(keyTypeInfo, x, y, application, hasDecryptedPrivateKey: false)
        {
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CryptographicException"/>
        public OpenSshEcdsaSKKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ImmutableArray<byte> application, OpenSshSKFlags flags, ShieldedImmutableBuffer keyHandle)
            : this(keyTypeInfo, x, y, application, hasDecryptedPrivateKey: true)
        {
            if (keyHandle == null)
                throw new ArgumentNullException(nameof(keyHandle));

            _flags = flags;
            _keyHandle = keyHandle;
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="CryptographicException"/>
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

            byte[] xBytes = Sec1.FieldElementToBytes(x, keyTypeInfo.KeySizeBits);
            byte[] yBytes = Sec1.FieldElementToBytes(y, keyTypeInfo.KeySizeBits);
            var ecParameters = new ECParameters
            {
                Curve = keyTypeInfo.Curve,
                Q = new ECPoint
                {
                    X = xBytes,
                    Y = yBytes,
                },
            };
            ecParameters.Validate();
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

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public OpenSshSKFlags Flags => GetPrivateKeyField(_flags);

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public ShieldedImmutableBuffer KeyHandle => GetPrivateKeyField(_keyHandle);

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
            SshEcdsaKey.WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            writer.WriteByteString(Application.AsSpan());
        }

        /// <exception cref="InvalidOperationException"/>
        public override void WritePrivateKeyTo(ref SshWireWriter writer)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L73-L79
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3245
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3315-L3324

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            SshEcdsaKey.WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            writer.WriteByteString(Application.AsSpan());
            writer.WriteByte((byte)Flags);
            using (var keyHandleUnshieldScope = _keyHandle.Unshield())
                writer.WriteByteString(keyHandleUnshieldScope.UnshieldedSpan);
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
                       _keyHandle.ShieldedSpan.SequenceEqual(other._keyHandle.ShieldedSpan);
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
                hashCode.AddBytes(_keyHandle.ShieldedSpan);
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
            (var x, var y) = SshEcdsaKey.ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);
            var application = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            try
            {
                return new OpenSshEcdsaSKKey(keyTypeInfo, x, y, application.ToImmutableArray());
            }
            catch (CryptographicException ex)
            {
                throw new InvalidDataException("Invalid EC parameters.", ex);
            }
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static OpenSshEcdsaSKKey ReadPrivateKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L74-L79
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3553-L3584

            string curveName = reader.ReadString();
            (var x, var y) = SshEcdsaKey.ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);
            var application = reader.ReadByteString();
            var flags = (OpenSshSKFlags)reader.ReadByte();
            var unshieldedKeyHandle = reader.ReadByteString();
            _ = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            var keyHandle = ShieldedImmutableBuffer.Create(unshieldedKeyHandle);

            try
            {
                return new OpenSshEcdsaSKKey(keyTypeInfo, x, y, application.ToImmutableArray(), flags, keyHandle);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidDataException("Invalid EC parameters.", ex);
            }
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
