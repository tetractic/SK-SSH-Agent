// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Numerics;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class SshEcdsaKey : SshKey
    {
        private readonly ShieldedImmutableBuffer _d;

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="CryptographicException"/>
        public SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y)
            : this(keyTypeInfo, x, y, hasDecryptedPrivateKey: false)
        {
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

        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="CryptographicException"/>
        public SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, ShieldedImmutableBuffer d)
            : this(keyTypeInfo, x, y, hasDecryptedPrivateKey: true)
        {
            if (d == null)
                throw new ArgumentNullException(nameof(d));

            _d = d;

            int fieldElementLength = Sec1.SizeBitsToLength(keyTypeInfo.KeySizeBits);

            byte[] xBytes = Sec1.FieldElementToBytes(x, keyTypeInfo.KeySizeBits);
            byte[] yBytes = Sec1.FieldElementToBytes(y, keyTypeInfo.KeySizeBits);
            byte[] dBytes = GC.AllocateArray<byte>(fieldElementLength, pinned: true);
            try
            {
                using (var dUnshieldScope = _d.Unshield())
                {
                    if (dUnshieldScope.UnshieldedLength != fieldElementLength)
                        throw new ArgumentException("Invalid EC field element.", nameof(d));
                    if (Sec1.GetBitLength(dUnshieldScope.UnshieldedSpan) > keyTypeInfo.KeySizeBits)
                        throw new ArgumentOutOfRangeException(nameof(d));

                    dUnshieldScope.UnshieldedSpan.CopyTo(dBytes);
                }

                var ecParameters = new ECParameters
                {
                    Curve = keyTypeInfo.Curve,
                    Q = new ECPoint
                    {
                        X = xBytes,
                        Y = yBytes,
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
        /// <exception cref="ArgumentOutOfRangeException"/>
        private SshEcdsaKey(SshKeyTypeInfo keyTypeInfo, BigInteger x, BigInteger y, bool hasDecryptedPrivateKey)
            : base(keyTypeInfo, hasDecryptedPrivateKey)
        {
            if (keyTypeInfo.Type != SshKeyType.Ecdsa)
                throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
            if (x < 0 || x.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0 || y.GetBitLength() > keyTypeInfo.KeySizeBits)
                throw new ArgumentOutOfRangeException(nameof(y));

            X = x;
            Y = y;
        }

        private SshEcdsaKey(SshEcdsaKey key, SshEncryptedPrivateKey encryptedPrivateKey)
            : base(key.KeyTypeInfo, encryptedPrivateKey)
        {
            X = key.X;
            Y = key.Y;
        }

        public BigInteger X { get; }

        public BigInteger Y { get; }

        /// <exception cref="InvalidOperationException" accessor="get"/>
        public ShieldedImmutableBuffer D => GetPrivateKeyField(_d);

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SshEcdsaKey Generate(SshKeyTypeInfo keyTypeInfo)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));
            if (keyTypeInfo.Type != SshKeyType.Ecdsa)
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
                var x = new BigInteger(ecParameters.Q.X, isUnsigned: true, isBigEndian: true);
                var y = new BigInteger(ecParameters.Q.Y, isUnsigned: true, isBigEndian: true);

                return new SshEcdsaKey(keyTypeInfo, x, y, d);
            }
        }

        public override void WritePublicKeyTo(ref SshWireWriter writer)
        {
            // https://datatracker.ietf.org/doc/html/rfc5656#section-3.1
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L880-L894

            writer.WriteString(KeyTypeInfo.Name);
            writer.WriteString(KeyTypeInfo.CurveName!);
            WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
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
            WriteECPoint2(ref writer, KeyTypeInfo.KeySizeBits, X, Y);
            using (var dUnshieldScope = _d.Unshield())
                WriteFieldElementAsBigInteger(ref writer, dUnshieldScope.UnshieldedSpan);
        }

        /// <exception cref="InvalidOperationException"/>
        public SshEcdsaSignature Sign(ReadOnlySpan<byte> data)
        {
            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("Private key is not present or is not decrypted.");

            int fieldElementLength = Sec1.SizeBitsToLength(KeyTypeInfo.KeySizeBits);

            byte[] xBytes = Sec1.FieldElementToBytes(X, KeyTypeInfo.KeySizeBits);
            byte[] yBytes = Sec1.FieldElementToBytes(Y, KeyTypeInfo.KeySizeBits);
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
                        X = xBytes,
                        Y = yBytes,
                    },
                    D = dBytes,
                };
                using (var ecdsa = ECDsa.Create(ecParameters))
                {
                    byte[] signatureBytes = new byte[2 * fieldElementLength];
                    if (!ecdsa.TrySignData(data, signatureBytes, KeyTypeInfo.HashAlgorithmName, out _))
                        throw new UnreachableException();

                    var r = new BigInteger(signatureBytes.AsSpan(0, fieldElementLength), isUnsigned: true, isBigEndian: true);
                    var s = new BigInteger(signatureBytes.AsSpan(fieldElementLength), isUnsigned: true, isBigEndian: true);

                    return new SshEcdsaSignature(KeyTypeInfo, r, s);
                }
            }
            finally
            {
                CryptographicOperations.ZeroMemory(dBytes);
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
                return X == other.X &&
                       Y == other.Y;
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
            (var x, var y) = ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);

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
            (var x, var y) = ReadECPoint2(ref reader, keyTypeInfo.KeySizeBits);
            var bigIntegerD = reader.ReadByteString();

            if (curveName != keyTypeInfo.CurveName)
                throw new NotSupportedException("Unrecognized curve name.");

            int fieldElementLength = Sec1.SizeBitsToLength(keyTypeInfo.KeySizeBits);

            var d = ShieldedImmutableBuffer.Create(fieldElementLength, bigIntegerD, SshWireBigIntegerToFieldElementBytes);

            try
            {
                return new SshEcdsaKey(keyTypeInfo, x, y, d);
            }
            catch (CryptographicException ex)
            {
                throw new InvalidDataException("Invalid EC parameters.", ex);
            }
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        internal static (BigInteger X, BigInteger Y) ReadECPoint2(ref SshWireReader reader, int fieldSizeBits)
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

        internal static void WriteECPoint2(ref SshWireWriter writer, int fieldSizeBits, BigInteger x, BigInteger y)
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

        /// <exception cref="InvalidDataException"/>
        private static void SshWireBigIntegerToFieldElementBytes(ReadOnlySpan<byte> bytes, Span<byte> fieldElementBytes)
        {
            if (bytes.Length > 0 && (bytes[0] & 0x80) != 0)
                throw new InvalidDataException("Invalid EC field element.");

            while (bytes.Length > 0 && bytes[0] == 0)
                bytes = bytes.Slice(1);

            if (bytes.Length > fieldElementBytes.Length)
                throw new InvalidDataException("Invalid EC field element.");

            int offset = fieldElementBytes.Length - bytes.Length;
            fieldElementBytes.Slice(0, offset).Clear();
            bytes.CopyTo(fieldElementBytes.Slice(offset));
        }

        private static void WriteFieldElementAsBigInteger(ref SshWireWriter writer, ReadOnlySpan<byte> fieldElementBytes)
        {
            while (fieldElementBytes.Length > 0 && fieldElementBytes[0] == 0)
                fieldElementBytes = fieldElementBytes.Slice(1);

            if (fieldElementBytes.Length > 0 && (fieldElementBytes[0] & 0x80) != 0)
            {
                writer.WriteUInt32((uint)fieldElementBytes.Length + 1);
                writer.WriteByte(0);
            }
            else
            {
                writer.WriteUInt32((uint)fieldElementBytes.Length);
            }
            writer.WriteBytes(fieldElementBytes);
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
