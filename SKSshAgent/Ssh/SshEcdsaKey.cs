// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class SshEcdsaKey : SshKey
    {
        private readonly ShieldedImmutableBuffer _d;

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CryptographicException"/>
        public SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> x, ImmutableArray<byte> y)
            : this(keyTypeInfo, x, y, hasDecryptedPrivateKey: false)
        {
            var ecParameters = new ECParameters
            {
                Curve = keyTypeInfo.Curve,
                Q = new ECPoint
                {
                    X = x.ToArray(),
                    Y = y.ToArray(),
                },
            };
            ecParameters.Validate();
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="CryptographicException"/>
        public SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> x, ImmutableArray<byte> y, ShieldedImmutableBuffer d)
            : this(keyTypeInfo, x, y, hasDecryptedPrivateKey: true)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));

            _d = d;

            int fieldSizeBits = keyTypeInfo.KeySizeBits;
            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);

            byte[] dBytes = GC.AllocateArray<byte>(fieldElementLength, pinned: true);
            try
            {
                using (var dUnshieldScope = d.Unshield())
                {
                    if (dUnshieldScope.UnshieldedLength != fieldElementLength || MPInt.GetBitLength(dUnshieldScope.UnshieldedSpan) > fieldSizeBits)
                        throw new ArgumentException("Invalid EC field element.", nameof(d));

                    dUnshieldScope.UnshieldedSpan.CopyTo(dBytes);
                }

                var ecParameters = new ECParameters
                {
                    Curve = keyTypeInfo.Curve,
                    Q = new ECPoint
                    {
                        X = x.ToArray(),
                        Y = y.ToArray(),
                    },
                    D = dBytes,
                };
                ecParameters.Validate();
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dBytes);
            }
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        private SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> x, ImmutableArray<byte> y, bool hasDecryptedPrivateKey)
            : base(keyTypeInfo, hasDecryptedPrivateKey)
        {
            if (keyTypeInfo.KeyType != SshKeyType.Ecdsa)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            int fieldSizeBits = keyTypeInfo.KeySizeBits;
            int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);
            if (x == null)
                throw new ArgumentNullException(nameof(x));
            if (x.Length != fieldElementLength || MPInt.GetBitLength(x.AsSpan()) > fieldSizeBits)
                throw new ArgumentException("Invalid EC field element.", nameof(x));
            if (y == null)
                throw new ArgumentNullException(nameof(y));
            if (y.Length != fieldElementLength || MPInt.GetBitLength(y.AsSpan()) > fieldSizeBits)
                throw new ArgumentException("Invalid EC field element.", nameof(y));

            X = x;
            Y = y;
        }

        private SshEcdsaKey(SshEcdsaKey key, SshEncryptedPrivateKey encryptedPrivateKey)
            : base(key.KeyTypeInfo, encryptedPrivateKey)
        {
            X = key.X;
            Y = key.Y;
        }

        public ImmutableArray<byte> X { get; }

        public ImmutableArray<byte> Y { get; }

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public ShieldedImmutableBuffer D => GetPrivateKeyField(_d);

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SshEcdsaKey Generate(SshKeyTypeInfo keyTypeInfo)
        {
            ArgumentNullException.ThrowIfNull(keyTypeInfo);
            if (keyTypeInfo.KeyType != SshKeyType.Ecdsa)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));

            using (var ecdsa = ECDsa.Create())
            {
                ecdsa.GenerateKey(keyTypeInfo.Curve);

                var ecParameters = ecdsa.ExportParameters(includePrivateParameters: true);
                ShieldedImmutableBuffer d;
                unsafe
                {
                    fixed (byte* dPtr = ecParameters.D)
                    {
                        d = ShieldedImmutableBuffer.Create(ecParameters.D);

                        CryptographicOperations.ZeroMemory(ecParameters.D);
                    }
                }
                var x = ecParameters.Q.X!.ToImmutableArray();
                var y = ecParameters.Q.Y!.ToImmutableArray();

                return new SshEcdsaKey(keyTypeInfo, x, y, d);
            }
        }

        public override void WritePublicKeyTo(ref SshWireWriter writer)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L880-L894

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            SshEC.WriteECPoint(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
        }

        /// <exception cref="InvalidOperationException"/>
        public override void WritePrivateKeyTo(ref SshWireWriter writer)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3245
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3297-L3304

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            SshEC.WriteECPoint(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            using (var dUnshieldScope = _d.Unshield())
                SshEC.WriteFieldElementAsMPInt(ref writer, dUnshieldScope.UnshieldedSpan);
        }

        /// <exception cref="InvalidOperationException"/>
        public SshEcdsaSignature Sign(ReadOnlySpan<byte> data)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            int fieldElementLength = MPInt.SizeBitsToLength(KeyTypeInfo.KeySizeBits);

            byte[] dBytes = GC.AllocateArray<byte>(fieldElementLength, pinned: true);
            try
            {
                using (var dUnshieldScope = _d.Unshield())
                    dUnshieldScope.UnshieldedSpan.CopyTo(dBytes);

                var ecParameters = new ECParameters
                {
                    Curve = KeyTypeInfo.Curve,
                    Q = new ECPoint
                    {
                        X = X.ToArray(),
                        Y = Y.ToArray(),
                    },
                    D = dBytes,
                };
                using (var ecdsa = ECDsa.Create(ecParameters))
                {
                    byte[] signatureBytes = new byte[2 * fieldElementLength];
                    if (!ecdsa.TrySignData(data, signatureBytes, KeyTypeInfo.HashAlgorithmName, out _))
                        throw new UnreachableException();

                    var r = signatureBytes.AsSpan(0, fieldElementLength).ToImmutableArray();
                    var s = signatureBytes.AsSpan(fieldElementLength).ToImmutableArray();

                    return new SshEcdsaSignature(KeyTypeInfo, r, s);
                }
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dBytes);
            }
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public bool Verify(ReadOnlySpan<byte> data, SshEcdsaSignature signature)
        {
            ArgumentNullException.ThrowIfNull(signature);
            if (signature.KeyTypeInfo != KeyTypeInfo)
                throw new ArgumentException("Incompatible signature.", nameof(signature));

            int fieldElementLength = MPInt.SizeBitsToLength(KeyTypeInfo.KeySizeBits);

            var ecParameters = new ECParameters
            {
                Curve = KeyTypeInfo.Curve,
                Q = new ECPoint
                {
                    X = X.ToArray(),
                    Y = Y.ToArray(),
                },
            };
            using (var ecdsa = ECDsa.Create(ecParameters))
            {
                byte[] signatureBytes = new byte[2 * fieldElementLength];
                signature.R.CopyTo(signatureBytes);
                signature.S.CopyTo(signatureBytes, fieldElementLength);

                return ecdsa.VerifyData(data, signatureBytes, KeyTypeInfo.HashAlgorithmName);
            }
        }

        public bool Equals([NotNullWhen(true)] SshEcdsaKey? other, bool publicOnly)
        {
            if (other == null || !PublicEquals(other))
                return false;

            if (publicOnly)
                return true;

            return (HasDecryptedPrivateKey == other.HasDecryptedPrivateKey) &&
                   (!HasDecryptedPrivateKey || PrivateEquals(other)) &&
                   EqualityComparer<SshEncryptedPrivateKey>.Default.Equals(EncryptedPrivateKey, other.EncryptedPrivateKey);

            bool PublicEquals(SshEcdsaKey other)
            {
                return X.SequenceEqual(other.X) &&
                       Y.SequenceEqual(other.Y);
            }

            bool PrivateEquals(SshEcdsaKey other)
            {
                return _d.ShieldedSpan.SequenceEqual(other._d.ShieldedSpan);
            }
        }

        public override bool Equals([NotNullWhen(true)] SshKey? other, bool publicOnly) => Equals(other as SshEcdsaKey, publicOnly);

        public override int GetHashCode(bool publicOnly)
        {
            var hashCode = new HashCode();
            hashCode.Add(X);
            hashCode.Add(Y);
            if (!publicOnly)
            {
                hashCode.AddBytes(_d.ShieldedSpan);
                hashCode.Add(EncryptedPrivateKey);
            }
            return hashCode.ToHashCode();
        }

        protected override SshKey WithEncryptedPrivateKey(SshEncryptedPrivateKey encryptedPrivateKey) => new SshEcdsaKey(this, encryptedPrivateKey);

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static SshEcdsaKey ReadPublicKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L2473-L2526

            string curveName = reader.ReadString();
            (var x, var y) = SshEC.ReadECPoint(ref reader, keyTypeInfo.KeySizeBits);

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            try
            {
                return new SshEcdsaKey(keyTypeInfo, x, y);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidDataException("Invalid EC parameters.", ex);
            }
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        internal static SshEcdsaKey ReadPrivateKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3522-L3552

            string curveName = reader.ReadString();
            (var x, var y) = SshEC.ReadECPoint(ref reader, keyTypeInfo.KeySizeBits);
            var mpIntD = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            int fieldElementLength = MPInt.SizeBitsToLength(keyTypeInfo.KeySizeBits);

            var d = ShieldedImmutableBuffer.Create(fieldElementLength, mpIntD, SshEC.SshWireMPIntToFieldElementBytes);

            try
            {
                return new SshEcdsaKey(keyTypeInfo, x, y, d);
            }
            catch (ArgumentException ex) when (ex.ParamName == "d")
            {
                throw new InvalidDataException("Invalid EC field element.");
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
