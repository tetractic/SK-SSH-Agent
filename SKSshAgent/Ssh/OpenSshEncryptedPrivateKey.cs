// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh
{
    internal sealed class OpenSshEncryptedPrivateKey : SshEncryptedPrivateKey, IEquatable<OpenSshEncryptedPrivateKey>
    {
        private const int _bcryptSaltLength = 16;

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public OpenSshEncryptedPrivateKey(SshKdfInfo kdfInfo, ImmutableArray<byte> kdfOptions, SshCipherInfo cipherInfo, ImmutableArray<byte> data)
        {
            if (kdfInfo is null)
                throw new ArgumentNullException(nameof(kdfInfo));
            if (kdfOptions == null)
                throw new ArgumentNullException(nameof(kdfOptions));
            if (cipherInfo is null)
                throw new ArgumentNullException(nameof(cipherInfo));
            if ((kdfInfo == SshKdfInfo.None) != (cipherInfo == SshCipherInfo.None))
                throw new ArgumentException("Incompatible KDF and cipher.");
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length < cipherInfo.TagLength)
                throw new ArgumentException("Data length must at least the cipher tag length.");
            if (((data.Length - cipherInfo.TagLength) % cipherInfo.BlockLength) != 0)
                throw new ArgumentException("Data length (excluding tag) must be a multiple of the cipher block length.");

            CipherInfo = cipherInfo;
            KdfInfo = kdfInfo;
            KdfOptions = kdfOptions;
            Data = data;
        }

        public SshCipherInfo CipherInfo { get; }

        public SshKdfInfo KdfInfo { get; }

        public ImmutableArray<byte> KdfOptions { get; }

        public ImmutableArray<byte> Data { get; }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="CryptographicException"/>
        public static OpenSshEncryptedPrivateKey Encrypt(SshKey privateKey, string comment, ReadOnlySpan<byte> password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
        {
            if (privateKey is null)
                throw new ArgumentNullException(nameof(privateKey));
            if (!privateKey.HasDecryptedPrivateKey)
                throw new ArgumentException("Private key is not present or is not decrypted.", nameof(privateKey));
            if (comment is null)
                throw new ArgumentNullException(nameof(comment));
            if (password.Length == 0 && kdfInfo != SshKdfInfo.None)
                throw new ArgumentException("Invalid password.", nameof(password));
            if (kdfInfo is null)
                throw new ArgumentNullException(nameof(kdfInfo));
            if (kdfRounds < 1 && kdfInfo != SshKdfInfo.None)
                throw new ArgumentOutOfRangeException(nameof(kdfRounds));
            if (cipherInfo is null)
                throw new ArgumentNullException(nameof(cipherInfo));
            if ((kdfInfo == SshKdfInfo.None) != (cipherInfo == SshCipherInfo.None))
                throw new ArgumentException("Incompatible KDF and cipher.");

            using (var plaintextBuffer = new PlaintextBufferWriter())
            {
                var writer = new SshWireWriter(plaintextBuffer);

                Span<byte> checkBytes = stackalloc byte[4];
                using (var rng = RandomNumberGenerator.Create())
                    rng.GetBytes(checkBytes);
                uint checkValue = BinaryPrimitives.ReadUInt32LittleEndian(checkBytes);

                writer.WriteUInt32(checkValue);
                writer.WriteUInt32(checkValue);

                privateKey.WritePrivateKeyTo(ref writer);

                writer.WriteString(comment);

                writer.Flush();

                for (byte i = 1; plaintextBuffer.WrittenLength % cipherInfo.BlockLength != 0; ++i)
                {
                    plaintextBuffer.GetSpan(1)[0] = i;
                    plaintextBuffer.Advance(1);
                }

                Span<byte> keyAndIV = stackalloc byte[cipherInfo.KeyLength + cipherInfo.IVLength];
                Span<byte> key = keyAndIV.Slice(0, cipherInfo.KeyLength);
                Span<byte> iv = keyAndIV.Slice(cipherInfo.KeyLength);

                var kdfOptionsBuffer = new ArrayBufferWriter<byte>();

                if (kdfInfo == SshKdfInfo.None)
                {
                    // Nothing to do.
                }
                else if (kdfInfo == SshKdfInfo.Bcrypt)
                {
                    Span<byte> salt = stackalloc byte[_bcryptSaltLength];
                    using (var rng = RandomNumberGenerator.Create())
                        rng.GetBytes(salt);

                    BcryptPbkdf.DeriveKey(password, salt, keyAndIV, kdfRounds);

                    WriteBcryptOptions(kdfOptionsBuffer, salt, kdfRounds);
                }
                else
                    throw new UnreachableException();

                Span<byte> plaintext = plaintextBuffer.WrittenSpan;

                byte[] data = new byte[plaintextBuffer.WrittenLength + cipherInfo.TagLength];

                if (cipherInfo == SshCipherInfo.None)
                {
                    plaintext.CopyTo(data);
                }
                else if (cipherInfo == SshCipherInfo.Aes128Cbc ||
                         cipherInfo == SshCipherInfo.Aes192Cbc ||
                         cipherInfo == SshCipherInfo.Aes256Cbc)
                {
                    using (var cipher = new SshAesCbcCipher(key, iv))
                        cipher.Encrypt(plaintext, data);
                }
                else if (cipherInfo == SshCipherInfo.Aes128Ctr ||
                         cipherInfo == SshCipherInfo.Aes192Ctr ||
                         cipherInfo == SshCipherInfo.Aes256Ctr)
                {
                    using (var cipher = new SshAesCtrCipher(key, iv))
                        cipher.Encrypt(plaintext, data);
                }
                else if (cipherInfo == SshCipherInfo.OpenSshAes128Gcm ||
                         cipherInfo == SshCipherInfo.OpenSshAes256Gcm)
                {
                    var ciphertext = data.AsSpan().Slice(0, plaintext.Length);
                    var tag = data.AsSpan().Slice(plaintext.Length);
                    using (var cipher = new OpenSshAesGcmCipher(key))
                        cipher.Encrypt(iv, plaintext, ciphertext, tag);
                }
                else
                    throw new UnreachableException();

                return new OpenSshEncryptedPrivateKey(
                    kdfInfo: kdfInfo,
                    kdfOptions: kdfOptionsBuffer.WrittenSpan.ToImmutableArray(),
                    cipherInfo: cipherInfo,
                    data: data.ToImmutableArray());
            }

            static void WriteBcryptOptions(ArrayBufferWriter<byte> buffer, Span<byte> salt, uint rounds)
            {
                var writer = new SshWireWriter(buffer);
                writer.WriteByteString(salt);
                writer.WriteUInt32(rounds);
                writer.Flush();
            }
        }

        /// <exception cref="ArgumentException"/>
        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="CryptographicException"/>
        public override bool TryDecrypt(ReadOnlySpan<byte> password, [MaybeNullWhen(false)] out SshKey privateKey, [MaybeNullWhen(false)] out string comment)
        {
            if (password.Length == 0 && KdfInfo != SshKdfInfo.None)
                throw new ArgumentException("Invalid password.", nameof(password));

            Span<byte> keyAndIV = stackalloc byte[CipherInfo.KeyLength + CipherInfo.IVLength];
            Span<byte> key = keyAndIV.Slice(0, CipherInfo.KeyLength);
            Span<byte> iv = keyAndIV.Slice(CipherInfo.KeyLength);

            int plaintextLength = Data.Length - CipherInfo.TagLength;
            byte[] plaintext = GC.AllocateUninitializedArray<byte>(plaintextLength, pinned: true);

            try
            {
                if (KdfInfo == SshKdfInfo.None)
                {
                    // Nothing to do.
                }
                else if (KdfInfo == SshKdfInfo.Bcrypt)
                {
                    var reader = new SshWireReader(KdfOptions.AsSpan());
                    var salt = reader.ReadByteString();
                    uint rounds = reader.ReadUInt32();
                    if (reader.BytesRemaining != 0)
                        throw new InvalidDataException("Excess data.");

                    if (rounds < 1)
                        throw new InvalidDataException("Invalid number of KDF rounds.");

                    BcryptPbkdf.DeriveKey(password, salt, keyAndIV, rounds);
                }
                else
                    throw new UnreachableException();

                if (CipherInfo == SshCipherInfo.None)
                {
                    Data.CopyTo(plaintext);
                }
                else if (CipherInfo == SshCipherInfo.Aes128Cbc ||
                         CipherInfo == SshCipherInfo.Aes192Cbc ||
                         CipherInfo == SshCipherInfo.Aes256Cbc)
                {
                    using (var cipher = new SshAesCbcCipher(key, iv))
                        cipher.Decrypt(Data.AsSpan(), plaintext);
                }
                else if (CipherInfo == SshCipherInfo.Aes128Ctr ||
                         CipherInfo == SshCipherInfo.Aes192Ctr ||
                         CipherInfo == SshCipherInfo.Aes256Ctr)
                {
                    using (var cipher = new SshAesCtrCipher(key, iv))
                        cipher.Decrypt(Data.AsSpan(), plaintext);
                }
                else if (CipherInfo == SshCipherInfo.OpenSshAes128Gcm ||
                         CipherInfo == SshCipherInfo.OpenSshAes256Gcm)
                {
                    var ciphertext = Data.AsSpan().Slice(0, plaintextLength);
                    var tag = Data.AsSpan().Slice(plaintextLength);
                    try
                    {
                        using (var cipher = new OpenSshAesGcmCipher(key))
                            cipher.Decrypt(iv, ciphertext, tag, plaintext);
                    }
                    catch (CryptographicException ex)
                        when (ex.HResult == unchecked((int)0x80131501))
                    {
                        privateKey = null;
                        comment = null;
                        return false;
                    }
                }
                else
                    throw new UnreachableException();

                // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.key#L36-L49

                var privateReader = new SshWireReader(plaintext);

                uint checkValue1 = privateReader.ReadUInt32();
                uint checkValue2 = privateReader.ReadUInt32();
                if (checkValue1 != checkValue2)
                {
                    privateKey = null;
                    comment = null;
                    return false;
                }

                privateKey = SshKey.ReadPrivateKey(ref privateReader);

                comment = privateReader.ReadString();

                for (byte i = 1; privateReader.BytesRemaining > 0; ++i)
                    if (privateReader.ReadByte() != i)
                        throw new InvalidDataException("Invalid padding.");

                return true;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(plaintext);
                CryptographicOperations.ZeroMemory(keyAndIV);
            }
        }

        public bool Equals([NotNullWhen(true)] OpenSshEncryptedPrivateKey? other)
        {
            return other != null &&
                   CipherInfo == other.CipherInfo &&
                   KdfInfo == other.KdfInfo &&
                   KdfOptions.SequenceEqual(other.KdfOptions) &&
                   Data.SequenceEqual(other.Data);
        }

        public override bool Equals([NotNullWhen(true)] SshEncryptedPrivateKey? other) => Equals(other as OpenSshEncryptedPrivateKey);

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(CipherInfo);
            hashCode.Add(KdfInfo);
            hashCode.AddBytes(KdfOptions.AsSpan());
            hashCode.AddBytes(Data.AsSpan());
            return hashCode.ToHashCode();
        }
    }
}
