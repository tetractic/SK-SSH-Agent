// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers.Binary;
using System.Collections.Immutable;
using System.Formats.Cbor;
using System.IO;

namespace SKSshAgent.WebAuthn
{
    /// <seealso href="https://www.w3.org/TR/webauthn/#sctn-authenticator-data"/> 
    internal sealed class WebAuthnAuthenticatorData
    {
        private WebAuthnAuthenticatorData(ImmutableArray<byte> rpIdHash, WebAuthnAuthenticatorDataFlags flags, uint signCount, WebAuthnAttestedCredentialData? attestedCredentialData)
        {
            RPIdHash = rpIdHash;
            Flags = flags;
            SignCount = signCount;
            AttestedCredentialData = attestedCredentialData;
        }

        public ImmutableArray<byte> RPIdHash { get; }

        public WebAuthnAuthenticatorDataFlags Flags { get; }

        public uint SignCount { get; }

        public WebAuthnAttestedCredentialData? AttestedCredentialData { get; }

        /// <exception cref="InvalidDataException"/>
        public static WebAuthnAuthenticatorData Parse(ReadOnlySpan<byte> bytes, out int bytesUsed)
        {
            const int fixedLength = 37;

            if (bytes.Length < fixedLength)
                throw new InvalidDataException("Insufficient data.");

            var rpIdHash = bytes.Slice(0, 32).ToImmutableArray();
            var flags = (WebAuthnAuthenticatorDataFlags)bytes[32];
            uint signCount = BinaryPrimitives.ReadUInt32BigEndian(bytes.Slice(33));
            bytes = bytes.Slice(fixedLength);

            var memory = bytes.ToArray().AsMemory();

            int attestedCredentialDataBytesUsed = 0;
            var attestedCredentialData = flags.HasFlag(WebAuthnAuthenticatorDataFlags.AttestedCredentialDataIncluded)
                ? WebAuthnAttestedCredentialData.Parse(memory, out attestedCredentialDataBytesUsed)
                : null;
            memory = memory.Slice(attestedCredentialDataBytesUsed);

            int extensionsBytesUsed = 0;
            if (flags.HasFlag(WebAuthnAuthenticatorDataFlags.ExtensionsIncluded))
            {
                if (memory.Length < 1)
                    throw new InvalidDataException("Insufficient data.");

                var reader = new CborReader(memory, CborConformanceMode.Ctap2Canonical);

                try
                {
                    if (reader.PeekState() != CborReaderState.StartMap)
                        throw new InvalidDataException("Invalid extensions CBOR data.");
                    reader.SkipValue();
                }
                catch (CborContentException ex)
                {
                    throw new InvalidDataException("Invalid extensions CBOR data.", ex);
                }

                extensionsBytesUsed = memory.Length - reader.BytesRemaining;
            }

            bytesUsed = fixedLength + attestedCredentialDataBytesUsed + extensionsBytesUsed;
            return new WebAuthnAuthenticatorData(rpIdHash, flags, signCount, attestedCredentialData);
        }
    }
}
