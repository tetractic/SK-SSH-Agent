// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class SshEd25519Key : SshKey
    {
        private const int _pkLength = Ed25519.PublicKeyLength;

        private const int _skLength = Ed25519.SecretKeyLength;

        private readonly ShieldedImmutableBuffer _sk;

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public SshEd25519Key(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk)
            : this(keyTypeInfo, pk, hasDecryptedPrivateKey: false)
        {
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="CryptographicException"/>
        public SshEd25519Key(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk, ShieldedImmutableBuffer sk)
            : this(keyTypeInfo, pk, hasDecryptedPrivateKey: true)
        {
            if (sk == null)
                throw new ArgumentNullException(nameof(sk));

            using (var skUnshieldScope = sk.Unshield())
                if (skUnshieldScope.UnshieldedLength != _skLength)
                    throw new ArgumentOutOfRangeException(nameof(sk), "Invalid length.");

            _sk = sk;
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        private SshEd25519Key(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk, bool hasDecryptedPrivateKey)
            : base(keyTypeInfo, hasDecryptedPrivateKey)
        {
            if (keyTypeInfo.KeyType != SshKeyType.Ed25519)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (pk == null)
                throw new ArgumentNullException(nameof(pk));
            if (pk.Length != _pkLength)
                throw new ArgumentOutOfRangeException(nameof(pk), "Invalid length.");

            PK = pk;
        }

        private SshEd25519Key(SshEd25519Key key, SshEncryptedPrivateKey encryptedPrivateKey)
            : base(key.KeyTypeInfo, encryptedPrivateKey)
        {
            PK = key.PK;
        }

        public ImmutableArray<byte> PK { get; }

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public ShieldedImmutableBuffer SK => GetPrivateKeyField(_sk);

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SshEd25519Key Generate(SshKeyTypeInfo keyTypeInfo)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));
            if (keyTypeInfo.KeyType != SshKeyType.Ed25519)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));

            Span<byte> publicKey = stackalloc byte[Ed25519.PublicKeyLength];
            Span<byte> secretKey = stackalloc byte[Ed25519.SecretKeyLength];
            Ed25519.GenerateKey(publicKey, secretKey);

            var pk = publicKey.ToImmutableArray();
            var sk = ShieldedImmutableBuffer.Create(secretKey);

            CryptographicOperations.ZeroMemory(secretKey);

            return new SshEd25519Key(keyTypeInfo, pk, sk);
        }

        public override void WritePublicKeyTo(ref SshWireWriter writer)
        {
            // https://datatracker.ietf.org/doc/html/rfc8709#section-4
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L906-L919

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteByteString(PK.AsSpan());
        }

        /// <exception cref="InvalidOperationException"/>
        public override void WritePrivateKeyTo(ref SshWireWriter writer)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3245
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3339-L3345

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteByteString(PK.AsSpan());
            using (var skUnshieldScope = _sk.Unshield())
                writer.WriteByteString(skUnshieldScope.UnshieldedSpan);
        }

        /// <exception cref="InvalidOperationException"/>
        public SshEd25519Signature Sign(ReadOnlySpan<byte> data)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            Span<byte> signature = stackalloc byte[Ed25519.SignatureLength];
            using (var skUnshieldScope = _sk.Unshield())
                Ed25519.SignData(skUnshieldScope.UnshieldedSpan, data, signature);

            var rs = signature.ToImmutableArray();

            return new SshEd25519Signature(KeyTypeInfo, rs);
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public bool Verify(ReadOnlySpan<byte> data, SshEd25519Signature signature)
        {
            if (signature is null)
                throw new ArgumentNullException(nameof(signature));
            if (signature.KeyTypeInfo != KeyTypeInfo)
                throw new ArgumentException("Incompatible signature.", nameof(signature));

            return Ed25519.VerifyData(PK.AsSpan(), data, signature.RS.AsSpan());
        }

        public bool Equals([NotNullWhen(true)] SshEd25519Key? other, bool publicOnly)
        {
            if (other == null || !PublicEquals(other))
                return false;

            if (publicOnly)
                return true;

            return (HasDecryptedPrivateKey == other.HasDecryptedPrivateKey) &&
                   (!HasDecryptedPrivateKey || PrivateEquals(other)) &&
                   EqualityComparer<SshEncryptedPrivateKey>.Default.Equals(EncryptedPrivateKey, other.EncryptedPrivateKey);

            bool PublicEquals(SshEd25519Key other)
            {
                return PK.SequenceEqual(other.PK);
            }

            bool PrivateEquals(SshEd25519Key other)
            {
                return _sk.ShieldedSpan.SequenceEqual(other._sk.ShieldedSpan);
            }
        }

        public override bool Equals([NotNullWhen(true)] SshKey? other, bool publicOnly) => Equals(other as SshEd25519Key, publicOnly);

        public override int GetHashCode(bool publicOnly)
        {
            var hashCode = new HashCode();
            hashCode.AddBytes(PK.AsSpan());
            if (!publicOnly)
            {
                hashCode.AddBytes(_sk.ShieldedSpan);
                hashCode.Add(EncryptedPrivateKey);
            }
            return hashCode.ToHashCode();
        }

        protected override SshKey WithEncryptedPrivateKey(SshEncryptedPrivateKey encryptedPrivateKey) => new SshEd25519Key(this, encryptedPrivateKey);

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static SshEd25519Key ReadPublicKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://datatracker.ietf.org/doc/html/rfc8709#section-4
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L2537-L2562

            var pk = reader.ReadByteString();

            try
            {
                return new SshEd25519Key(keyTypeInfo, pk.ToImmutableArray());
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidDataException("Invalid parameter.");
            }
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        internal static SshEd25519Key ReadPrivateKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3634-L3646

            var pk = reader.ReadByteString();
            var unshieldedSk = reader.ReadByteString();

            var sk = ShieldedImmutableBuffer.Create(unshieldedSk);

            try
            {
                return new SshEd25519Key(keyTypeInfo, pk.ToImmutableArray(), sk);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidDataException("Invalid parameter.");
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
