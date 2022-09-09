// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SKSshAgent.Ssh
{
    // https://www.openssh.com/specs.html
    internal abstract class SshKey : IEquatable<SshKey?>
    {
        private const string _openSshPrivateKeyLabel = "OPENSSH PRIVATE KEY";

        private static readonly ReadOnlyMemory<byte> _openSshKeyV1 = Encoding.ASCII.GetBytes("openssh-key-v1\0");

        /// <exception cref="ArgumentNullException"/>
        protected SshKey(SshKeyTypeInfo keyTypeInfo, bool hasDecryptedPrivateKey)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));

            KeyTypeInfo = keyTypeInfo;
            HasDecryptedPrivateKey = hasDecryptedPrivateKey;
        }

        /// <exception cref="ArgumentNullException"/>
        protected SshKey(SshKeyTypeInfo keyTypeInfo, SshEncryptedPrivateKey encryptedPrivateKey)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));
            if (encryptedPrivateKey is null)
                throw new ArgumentNullException(nameof(encryptedPrivateKey));

            KeyTypeInfo = keyTypeInfo;
            EncryptedPrivateKey = encryptedPrivateKey;
        }

        public SshKeyTypeInfo KeyTypeInfo { get; }

        public bool HasDecryptedPrivateKey { get; }

        public SshEncryptedPrivateKey? EncryptedPrivateKey { get; }

        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SshWireContentException"/>
        /// <exception cref="NotSupportedException"/>
        public static (SshKey Key, string Comment) ParseOpenSshPublicKey(ReadOnlySpan<char> data)
        {
            throw new NotImplementedException();  // TODO
        }

        /// <exception cref="InvalidDataException"/>
        /// <exception cref="SshWireContentException"/>
        /// <exception cref="NotSupportedException"/>
        public static (SshKey Key, string Comment) ParseOpenSshPrivateKey(ReadOnlySpan<char> data)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L4114
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L4286

            if (!PemEncoding.TryFind(data, out var fields))
                throw new InvalidDataException("Invalid BEGIN or END.");

            string label = data[fields.Label].ToString();

            if (label != _openSshPrivateKeyLabel)
                throw new InvalidDataException("Unrecognized label.");

            Span<byte> keyData = new byte[fields.DecodedDataLength];
            if (!Convert.TryFromBase64Chars(data[fields.Base64Data], keyData, out int bytesWritten))
                throw new InvalidDataException("Invalid data.");
            Debug.Assert(bytesWritten == fields.DecodedDataLength);

            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.key#L10-L19

            var magicValue = _openSshKeyV1.Span;
            if (keyData.Length < magicValue.Length)
                throw new InvalidDataException("Insufficient data.");
            if (!keyData.Slice(0, magicValue.Length).SequenceEqual(magicValue))
                throw new InvalidDataException("Mismatched magic value.");
            keyData = keyData.Slice(magicValue.Length);

            var reader = new SshWireReader(keyData);

            string cipherName = reader.ReadString();
            if (!SshCipherInfo.TryGetCipherInfoByName(cipherName, out var cipherInfo))
                throw new NotSupportedException("Unrecognized private key encryption cipher.");

            string kdfName = reader.ReadString();
            if (!SshKdfInfo.TryGetKdfInfoByName(kdfName, out var kdfInfo))
                throw new NotSupportedException("Unrecognized private key encryption KDF.");

            var kdfOptions = reader.ReadByteString();

            uint keyCount = reader.ReadUInt32();
            if (keyCount != 1)
                throw new NotSupportedException("Multiple keys are not supported.");

            var publicData = reader.ReadByteString();

            var publicReader = new SshWireReader(publicData);
            var publicKey = ReadPublicKey(ref publicReader);
            if (publicReader.BytesRemaining != 0)
                throw new InvalidDataException("Excess data in public key.");

            int ciphertextLength = (int)reader.ReadUInt32();
            if (ciphertextLength < 0 || ciphertextLength > int.MaxValue - cipherInfo.TagLength)
                throw new SshWireContentException("Excessively long byte string.");

            var encryptedPrivateKeys = reader.ReadBytes(ciphertextLength + cipherInfo.TagLength);

            if (reader.BytesRemaining != 0)
                throw new InvalidDataException("Excess data.");

            if ((cipherInfo == SshCipherInfo.None) != (kdfInfo == SshKdfInfo.None))
                throw new InvalidDataException("Incompatible cipher and KDF.");

            if (encryptedPrivateKeys.Length < cipherInfo.TagLength ||
                (encryptedPrivateKeys.Length - cipherInfo.TagLength) % cipherInfo.BlockLength != 0)
            {
                throw new InvalidDataException("Invalid private key data length.");
            }

            var encryptedPrivateKey = new OpenSshEncryptedPrivateKey(
                kdfInfo: kdfInfo,
                kdfOptions: kdfOptions.ToImmutableArray(),
                cipherInfo: cipherInfo,
                data: encryptedPrivateKeys.ToImmutableArray());

            if (cipherInfo == SshCipherInfo.None)
            {
                if (!encryptedPrivateKey.TryDecrypt(default, out var privateKey, out string? comment))
                    throw new UnreachableException();

                return (privateKey, comment);
            }
            else
            {
                return (publicKey.WithEncryptedPrivateKey(encryptedPrivateKey), string.Empty);
            }
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="InvalidDataException"/>
        public static SshKey ReadPublicKey(ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L2365

            string keyTypeName = reader.ReadString();

            if (!SshKeyTypeInfo.TryGetKeyTypeInfoByName(keyTypeName, out var keyTypeInfo))
                throw new NotSupportedException("Unrecognized key type name.");

            return keyTypeInfo.Type switch
            {
                SshKeyType.Ecdsa => SshEcdsaKey.ReadPublicKey(keyTypeInfo, ref reader),
                SshKeyType.Ed25519 => SshEd25519Key.ReadPublicKey(keyTypeInfo, ref reader),
                SshKeyType.OpenSshEcdsaSK => OpenSshEcdsaSKKey.ReadPublicKey(keyTypeInfo, ref reader),
                _ => throw new UnreachableException(),
            };
        }

        /// <exception cref="SshWireContentException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="InvalidDataException"/>
        public static SshKey ReadPrivateKey(ref SshWireReader reader)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3438

            string keyTypeName = reader.ReadString();

            if (!SshKeyTypeInfo.TryGetKeyTypeInfoByName(keyTypeName, out var keyTypeInfo))
                throw new NotSupportedException("Unrecognized key type name.");

            return keyTypeInfo.Type switch
            {
                SshKeyType.Ecdsa => SshEcdsaKey.ReadPrivateKey(keyTypeInfo, ref reader),
                SshKeyType.Ed25519 => SshEd25519Key.ReadPrivateKey(keyTypeInfo, ref reader),
                SshKeyType.OpenSshEcdsaSK => OpenSshEcdsaSKKey.ReadPrivateKey(keyTypeInfo, ref reader),
                _ => throw new UnreachableException(),
            };
        }

        public char[] FormatOpenSshPublicKey(string comment)
        {
            var buffer = new ArrayBufferWriter<byte>();

            var writer = new SshWireWriter(buffer);
            WritePublicKeyTo(ref writer);
            writer.Flush();

            int base64KeyLength = ((buffer.WrittenCount + 2) / 3) * 4;

            int resultLength = KeyTypeInfo.Name.Length + 1 + base64KeyLength + 1 + comment.Length;
            char[] result = new char[resultLength];

            var span = result.AsSpan();
            KeyTypeInfo.Name.CopyTo(span);
            span = span.Slice(KeyTypeInfo.Name.Length);
            span[0] = ' ';
            span = span.Slice(1);
            if (!Convert.TryToBase64Chars(buffer.WrittenSpan, span, out int charsWritten))
                throw new UnreachableException();
            Debug.Assert(charsWritten == base64KeyLength);
            span = span.Slice(charsWritten);
            span[0] = ' ';
            span = span.Slice(1);
            comment.CopyTo(span);
            span = span.Slice(comment.Length);
            Debug.Assert(span.Length == 0);

            return result;
        }

        /// <exception cref="CryptographicException"/>
        public char[] FormatOpenSshPrivateKey(string comment)
        {
            return FormatOpenSshPrivateKeyCore(comment, default, SshKdfInfo.None, default, SshCipherInfo.None);
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="CryptographicException"/>
        public char[] FormatOpenSshPrivateKey(string comment, ShieldedImmutableBuffer password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
        {
            if (comment is null)
                throw new ArgumentNullException(nameof(comment));
            if (password.IsEmpty && kdfInfo != SshKdfInfo.None)
                throw new ArgumentException("Invalid password.", nameof(password));
            if (kdfInfo is null)
                throw new ArgumentNullException(nameof(kdfInfo));
            if (kdfRounds < 1 && kdfInfo != SshKdfInfo.None)
                throw new ArgumentOutOfRangeException(nameof(kdfRounds));
            if (cipherInfo is null)
                throw new ArgumentNullException(nameof(cipherInfo));
            if ((kdfInfo == SshKdfInfo.None) != (cipherInfo == SshCipherInfo.None))
                throw new ArgumentException("Incompatible KDF and cipher.");

            if (!HasDecryptedPrivateKey)
                throw new InvalidOperationException("The private key is not present or is not decrypted.");

            return FormatOpenSshPrivateKeyCore(comment, password, kdfInfo, kdfRounds, cipherInfo);
        }

        public abstract void WritePublicKeyTo(ref SshWireWriter writer);

        public abstract void WritePrivateKeyTo(ref SshWireWriter writer);

        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="SshWireContentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <exception cref="NotSupportedException"/>
        /// <exception cref="CryptographicException"/>
        public bool TryDecryptPrivateKey(ShieldedImmutableBuffer password, [MaybeNullWhen(false)] out SshKey privateKey, [MaybeNullWhen(false)] out string comment)
        {
            if (password.IsEmpty)
                throw new ArgumentException("Invalid password.", nameof(password));

            if (EncryptedPrivateKey == null)
                throw new InvalidOperationException("The private key is not present or is not decrypted.");

            if (!EncryptedPrivateKey.TryDecrypt(password, out privateKey, out comment))
                return false;

            if (!Equals(privateKey, publicOnly: true))
                throw new InvalidDataException("Mismatched public and private keys.");

            return true;
        }

        public byte[] GetSha256Fingerprint()
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new SshWireWriter(buffer);
            WritePublicKeyTo(ref writer);
            writer.Flush();
            return SHA256.HashData(buffer.WrittenSpan);
        }

        public byte[] GetMd5Fingerprint()
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new SshWireWriter(buffer);
            WritePublicKeyTo(ref writer);
            writer.Flush();
#pragma warning disable CA5351 // Do Not Use Broken Cryptographic Algorithms
            return MD5.HashData(buffer.WrittenSpan);
#pragma warning restore CA5351 // Do Not Use Broken Cryptographic Algorithms
        }

        public virtual string GetOpenSshKeyAuthorization(string comment)
        {
            var buffer = new ArrayBufferWriter<byte>();
            var writer = new SshWireWriter(buffer);
            WritePublicKeyTo(ref writer);
            writer.Flush();

            string base64Blob = Convert.ToBase64String(buffer.WrittenSpan);

            return $"{KeyTypeInfo.Name} {base64Blob} {comment}";
        }

        public abstract bool Equals([NotNullWhen(true)] SshKey? other, bool publicOnly);

        public bool Equals([NotNullWhen(true)] SshKey? other) => Equals(other, publicOnly: false);

        public sealed override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as SshKey, publicOnly: false);

        public abstract int GetHashCode(bool publicOnly);

        public sealed override int GetHashCode() => GetHashCode(publicOnly: false);

        protected abstract SshKey WithEncryptedPrivateKey(SshEncryptedPrivateKey encryptedPrivateKey);

        /// <exception cref="CryptographicException"/>
        private char[] FormatOpenSshPrivateKeyCore(string comment, ShieldedImmutableBuffer password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3910

            var buffer = new ArrayBufferWriter<byte>();

            var magicValue = _openSshKeyV1.Span;
            magicValue.CopyTo(buffer.GetSpan(magicValue.Length));
            buffer.Advance(magicValue.Length);

            var writer = new SshWireWriter(buffer);

            writer.WriteString(cipherInfo.Name);

            writer.WriteString(kdfInfo.Name);

            var encryptedPrivateKey = OpenSshEncryptedPrivateKey.Encrypt(this, comment, password, kdfInfo, kdfRounds, cipherInfo);

            writer.WriteByteString(encryptedPrivateKey.KdfOptions.AsSpan());

            writer.WriteUInt32(1);  // key count

            var publicBuffer = new ArrayBufferWriter<byte>();
            var publicWriter = new SshWireWriter(publicBuffer);
            WritePublicKeyTo(ref publicWriter);
            publicWriter.Flush();

            writer.WriteByteString(publicBuffer.WrittenSpan);

            int ciphertextLength = encryptedPrivateKey.Data.Length - cipherInfo.TagLength;
            writer.WriteUInt32((uint)ciphertextLength);

            writer.WriteBytes(encryptedPrivateKey.Data.AsSpan());

            writer.Flush();

            char[] result = PemEncoding.Write(_openSshPrivateKeyLabel, buffer.WrittenSpan);
            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = '\n';
            return result;
        }
    }
}
