// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SKSshAgent.Ssh
{
    // https://www.openssh.com/specs.html
    internal abstract class SshKey
    {
        private const string _openSshPrivateKeyLabel = "OPENSSH PRIVATE KEY";

        private static readonly ReadOnlyMemory<byte> _openSshKeyV1 = Encoding.ASCII.GetBytes("openssh-key-v1\0");

        /// <exception cref="ArgumentNullException"/>
        protected SshKey(SshKeyTypeInfo keyTypeInfo)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));

            KeyTypeInfo = keyTypeInfo;
        }

        public SshKeyTypeInfo KeyTypeInfo { get; }

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
            if (cipherName != "none")
                throw new NotSupportedException("Encrypted keys are not supported.");

            string kdfName = reader.ReadString();
            if (kdfName != "none")
                throw new NotSupportedException("Encrypted keys are not supported.");

            _ = reader.ReadByteString();  // KDF options

            uint keyCount = reader.ReadUInt32();
            if (keyCount != 1)
                throw new NotSupportedException("Multiple keys are not supported.");

            var publicData = reader.ReadByteString();

            var publicReader = new SshWireReader(publicData);
            var publicKey = ReadPublicKey(ref publicReader);
            if (publicReader.BytesRemaining != 0)
                throw new InvalidDataException("Excess data in public key.");

            var privateKeys = reader.ReadByteString();

            if (reader.BytesRemaining != 0)
                throw new InvalidDataException("Excess data.");

            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.key#L36-L49

            var privateReader = new SshWireReader(privateKeys);

            uint checkValue1 = privateReader.ReadUInt32();
            uint checkValue2 = privateReader.ReadUInt32();
            if (checkValue1 != checkValue2)
                throw new InvalidDataException("Mismatched check values.");

            var privateKey = ReadPrivateKey(ref privateReader);

            string comment = privateReader.ReadString();

            for (byte i = 1; privateReader.BytesRemaining > 0; ++i)
                if (privateReader.ReadByte() != i)
                    throw new InvalidDataException("Invalid padding.");

            if (!publicKey.Equals(privateKey, publicOnly: true))
                throw new InvalidDataException("Mismatched public and private keys.");

            return (privateKey, comment);
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

        public char[] FormatOpenSshPrivateKey(string comment)
        {
            // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3910

            var buffer = new ArrayBufferWriter<byte>();

            var magicValue = _openSshKeyV1.Span;
            magicValue.CopyTo(buffer.GetSpan(magicValue.Length));
            buffer.Advance(magicValue.Length);

            var writer = new SshWireWriter(buffer);

            writer.WriteString("none");  // cipher name
            writer.WriteString("none");  // KDF
            writer.WriteByteString(Span<byte>.Empty);  // KDF options
            writer.WriteUInt32(1);  // key count

            var innerBuffer = new ArrayBufferWriter<byte>();
            var publicWriter = new SshWireWriter(innerBuffer);
            WritePublicKeyTo(ref publicWriter);
            publicWriter.Flush();

            writer.WriteByteString(innerBuffer.WrittenSpan);

            innerBuffer.Clear();
            var privateWriter = new SshWireWriter(innerBuffer);
            using (var rng = RandomNumberGenerator.Create())
            {
                Span<byte> checkBytes = stackalloc byte[4];
                rng.GetBytes(checkBytes);
                uint checkValue = BinaryPrimitives.ReadUInt32LittleEndian(checkBytes);
                privateWriter.WriteUInt32(checkValue);
                privateWriter.WriteUInt32(checkValue);
            }
            WritePrivateKeyTo(ref privateWriter);
            privateWriter.WriteString(comment);
            privateWriter.Flush();

            for (byte i = 1; innerBuffer.WrittenCount % 8 != 0; ++i)
            {
                innerBuffer.GetSpan(1)[0] = i;
                innerBuffer.Advance(1);
            }

            writer.WriteByteString(innerBuffer.WrittenSpan);

            writer.Flush();

            char[] result = PemEncoding.Write(_openSshPrivateKeyLabel, buffer.WrittenSpan);
            Array.Resize(ref result, result.Length + 1);
            result[result.Length - 1] = '\n';
            return result;
        }

        public abstract void WritePublicKeyTo(ref SshWireWriter writer);

        public abstract void WritePrivateKeyTo(ref SshWireWriter writer);

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
            return MD5.HashData(buffer.WrittenSpan);
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

        public abstract bool Equals(SshKey? other, bool publicOnly);
    }
}
