// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using System;
using System.Formats.Asn1;
using System.IO;
using System.Security.Cryptography;

namespace SKSshAgent.WebAuthn
{
    internal static class WebAuthnSignature
    {
        /// <seealso href="https://www.w3.org/TR/webauthn/#fig-signature"/>
        public static byte[] GetSignedData(ReadOnlySpan<byte> challenge, ReadOnlySpan<byte> authenticatorData)
        {
            const int sha256HashLength = 32;
            byte[] signedData = new byte[authenticatorData.Length + sha256HashLength];
            authenticatorData.CopyTo(signedData);
            _ = SHA256.HashData(challenge, signedData.AsSpan(authenticatorData.Length));
            return signedData;
        }

        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="InvalidDataException"/>
        /// <seealso href="https://www.w3.org/TR/webauthn/#sctn-signature-attestation-types"/>
        public static CoseSignature Parse(CoseKey publicKey, ReadOnlyMemory<byte> bytes, out int bytesUsed)
        {
            if (publicKey is null)
                throw new ArgumentNullException(nameof(publicKey));

            switch (publicKey.Algorithm)
            {
                case CoseAlgorithm.ES256:
                case CoseAlgorithm.ES384:
                case CoseAlgorithm.ES512:
                {
                    var ec2PublicKey = (CoseEC2Key)publicKey;

                    return ParseEcdsaAsn1(ec2PublicKey.Algorithm, ec2PublicKey.Curve, bytes, out bytesUsed);
                }
                default:
                    throw new ArgumentException("Unrecognized algorithm.", nameof(publicKey));
            }
        }

        /// <exception cref="InvalidDataException"/>
        /// <seealso href="https://www.w3.org/TR/webauthn/#sctn-signature-attestation-types"/>
        internal static CoseEcdsaSignature ParseEcdsaAsn1(CoseAlgorithm algorithm, CoseEllipticCurve curve, ReadOnlyMemory<byte> bytes, out int bytesUsed)
        {
            try
            {
                int offset;
                int length;

                var span = bytes.Span;
                AsnDecoder.ReadSequence(span, AsnEncodingRules.DER, out offset, out length, out bytesUsed);
                int sequenceEnd = offset + length;

                var integerR = AsnDecoder.ReadIntegerBytes(span.Slice(offset), AsnEncodingRules.DER, out length);
                offset += length;

                var integerS = AsnDecoder.ReadIntegerBytes(span.Slice(offset), AsnEncodingRules.DER, out length);
                offset += length;

                if (offset != sequenceEnd)
                    throw new InvalidDataException("Excess data in ASN.1 sequence.");

                int fieldSizeBits = curve.GetFieldSizeBits();
                int fieldElementLength = MPInt.SizeBitsToLength(fieldSizeBits);

                Span<byte> r = stackalloc byte[fieldElementLength];
                AsnIntegerToFieldElementBytes(integerR, r);
                Span<byte> s = stackalloc byte[fieldElementLength];
                AsnIntegerToFieldElementBytes(integerS, s);

                try
                {
                    return new CoseEcdsaSignature(algorithm, curve, r.ToImmutableArray(), s.ToImmutableArray());
                }
                catch (ArgumentOutOfRangeException ex)
                    when (ex.ParamName == "r" || ex.ParamName == "s")
                {
                    throw new InvalidDataException("Invalid EC field element.");
                }
            }
            catch (AsnContentException ex)
            {
                throw new InvalidDataException("Invalid ASN.1 DER data.", ex);
            }
        }

        /// <exception cref="InvalidDataException"/>
        private static void AsnIntegerToFieldElementBytes(ReadOnlySpan<byte> bytes, Span<byte> fieldElementBytes)
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
    }
}
