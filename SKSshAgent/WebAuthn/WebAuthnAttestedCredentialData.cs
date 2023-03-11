// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Formats.Cbor;
using System.IO;
using System.Security.Cryptography;

namespace SKSshAgent.WebAuthn
{
    /// <seealso href="https://www.w3.org/TR/webauthn/#sctn-attested-credential-data"/>
    internal sealed class WebAuthnAttestedCredentialData
    {
        private WebAuthnAttestedCredentialData(ImmutableArray<byte> aaGuid, ImmutableArray<byte> credentialId, CoseKey credentialPublicKey)
        {
            AAGuid = aaGuid;
            CredentialId = credentialId;
            CredentialPublicKey = credentialPublicKey;
        }

        public ImmutableArray<byte> AAGuid { get; }

        public ImmutableArray<byte> CredentialId { get; }

        public CoseKey CredentialPublicKey { get; }

        /// <exception cref="InvalidDataException"/>
        public static WebAuthnAttestedCredentialData Parse(ReadOnlyMemory<byte> bytes, out int bytesUsed)
        {
            const int fixedLength = 18;

            if (bytes.Length < fixedLength)
                throw new InvalidDataException("Insufficient data.");

            var span = bytes.Span;
            var aaGuid = span.Slice(0, 16).ToImmutableArray();
            ushort credentialIdLength = BinaryPrimitives.ReadUInt16BigEndian(span.Slice(16));
            bytes = bytes.Slice(fixedLength);

            if (bytes.Length < credentialIdLength)
                throw new InvalidDataException("Insufficient data.");

            var credentialId = bytes.Slice(0, credentialIdLength).ToImmutableArray();
            bytes = bytes.Slice(credentialIdLength);

            var credentialPublicKey = ParsePublicKey(bytes, out int credentialPublicKeyBytesUsed);

            bytesUsed = fixedLength + credentialIdLength + credentialPublicKeyBytesUsed;
            return new WebAuthnAttestedCredentialData(aaGuid, credentialId, credentialPublicKey);
        }

        /// <exception cref="InvalidDataException"/>
        private static CoseKey ParsePublicKey(ReadOnlyMemory<byte> bytes, out int publicKeyBytesUsed)
        {
            var reader = new CborReader(bytes, CborConformanceMode.Ctap2Canonical);

            if (bytes.Length < 1)
                throw new InvalidDataException("Insufficient data.");

            try
            {
                CoseKey publicKey;

                if (reader.PeekState() != CborReaderState.StartMap)
                    throw new InvalidDataException("Invalid CBOR data.");
                _ = reader.ReadStartMap()!.Value;

                ReadExpectedLabel(reader, 1);
                var keyType = (CoseKeyType)ReadInt32(reader);

                switch (keyType)
                {
                    case CoseKeyType.EC2:
                    {
                        ReadExpectedLabel(reader, 3);
                        var algorithm = (CoseAlgorithm)ReadInt32(reader);

                        switch (algorithm)
                        {
                            case CoseAlgorithm.ES256:
                            case CoseAlgorithm.ES384:
                            case CoseAlgorithm.ES512:
                            {
                                // https://datatracker.ietf.org/doc/html/rfc8152#section-13.1.1

                                ReadExpectedLabel(reader, -1);
                                var curve = (CoseEllipticCurve)ReadInt32(reader);

                                // https://www.w3.org/TR/webauthn-2/#sctn-alg-identifier
                                switch ((algorithm, curve))
                                {
                                    case (CoseAlgorithm.ES256, CoseEllipticCurve.P256):
                                    case (CoseAlgorithm.ES384, CoseEllipticCurve.P384):
                                    case (CoseAlgorithm.ES512, CoseEllipticCurve.P521):
                                        break;
                                    default:
                                        throw new InvalidDataException("Invalid elliptic curve for algorithm.");
                                }

                                ReadExpectedLabel(reader, -2);
                                byte[] x = ReadByteString(reader);

                                ReadExpectedLabel(reader, -3);
                                byte[] y = ReadByteString(reader);

                                try
                                {
                                    publicKey = new CoseEC2Key(algorithm, curve, x, y);
                                }
                                catch (CryptographicException ex)
                                {
                                    throw new InvalidDataException("Invalid elliptic curve public key parameters.", ex);
                                }
                                break;
                            }
                            default:
                                throw new InvalidDataException("Unrecognized signature algorithm.");
                        }
                        break;
                    }
                    default:
                        throw new InvalidDataException("Unrecognized key type.");
                }

                if (reader.PeekState() != CborReaderState.EndMap)
                    throw new InvalidDataException("Invalid CBOR data.");
                reader.ReadEndMap();

                publicKeyBytesUsed = bytes.Length - reader.BytesRemaining;
                return publicKey;
            }
            catch (Exception ex)
                when (ex is CborContentException ||
                      ex is OverflowException)
            {
                throw new InvalidDataException("Invalid CBOR data.", ex);
            }

            static void ReadExpectedLabel(CborReader reader, int expectedLabel)
            {
                int label = ReadInt32(reader);
                if (label != expectedLabel)
                    throw new InvalidDataException("Invalid CBOR data.");
            }

            static int ReadInt32(CborReader reader)
            {
                var state = reader.PeekState();
                if (state != CborReaderState.UnsignedInteger && state != CborReaderState.NegativeInteger)
                    throw new InvalidDataException("Invalid CBOR data.");

                return reader.ReadInt32();
            }

            static byte[] ReadByteString(CborReader reader)
            {
                var state = reader.PeekState();
                if (state != CborReaderState.ByteString)
                    throw new InvalidDataException("Invalid CBOR data.");

                return reader.ReadByteString();
            }
        }
    }
}
