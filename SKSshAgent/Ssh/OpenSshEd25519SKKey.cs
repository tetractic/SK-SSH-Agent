// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cryptography;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

namespace SKSshAgent.Ssh;

internal sealed class OpenSshEd25519SKKey : SshKey
{
    private readonly OpenSshSKFlags _flags;
    private readonly ShieldedImmutableBuffer _keyHandle;

    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public OpenSshEd25519SKKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk, ImmutableArray<byte> application)
        : this(keyTypeInfo, pk, application, hasDecryptedPrivateKey: false)
    {
    }

    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    public OpenSshEd25519SKKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk, ImmutableArray<byte> application, OpenSshSKFlags flags, ShieldedImmutableBuffer keyHandle)
        : this(keyTypeInfo, pk, application, hasDecryptedPrivateKey: true)
    {
        if (keyHandle == null)
            throw new ArgumentNullException(nameof(keyHandle));

        _flags = flags;
        _keyHandle = keyHandle;
    }

    /// <exception cref="ArgumentException"/>
    /// <exception cref="ArgumentNullException"/>
    private OpenSshEd25519SKKey(SshKeyTypeInfo keyTypeInfo, ImmutableArray<byte> pk, ImmutableArray<byte> application, bool hasDecryptedPrivateKey)
        : base(keyTypeInfo, hasDecryptedPrivateKey)
    {
        if (keyTypeInfo.KeyType != SshKeyType.OpenSshEd25519SK)
            throw new ArgumentException("Incompatible key type.", nameof(keyTypeInfo));
        if (pk == null)
            throw new ArgumentNullException(nameof(pk));
        if (pk.Length != Ed25519.PublicKeyLength)
            throw new ArgumentException("Invalid PK length.", nameof(pk));
        if (application == null)
            throw new ArgumentNullException(nameof(application));

        PK = pk;
        Application = application;
    }

    private OpenSshEd25519SKKey(OpenSshEd25519SKKey key, SshEncryptedPrivateKey encryptedPrivateKey)
        : base(key.KeyTypeInfo, encryptedPrivateKey)
    {
        PK = key.PK;
        Application = key.Application;
    }

    public ImmutableArray<byte> PK { get; }

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
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L83-L85
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L907-L919

        writer.WriteString(KeyTypeInfo.Name);
        writer.WriteByteString(PK.AsSpan());
        writer.WriteByteString(Application.AsSpan());
    }

    /// <exception cref="InvalidOperationException"/>
    public override void WritePrivateKeyTo(ref SshWireWriter writer)
    {
        if (!HasDecryptedPrivateKey)
            throw new InvalidOperationException("Private key is not present or is not decrypted.");

        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L89-L94
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3245
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3358-L3366

        writer.WriteString(KeyTypeInfo.Name);
        writer.WriteByteString(PK.AsSpan());
        writer.WriteByteString(Application.AsSpan());
        writer.WriteByte((byte)Flags);
        using (var keyHandleUnshieldScope = _keyHandle.Unshield())
            writer.WriteByteString(keyHandleUnshieldScope.UnshieldedSpan);
        writer.WriteByteString(Span<byte>.Empty);
    }

    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="ArgumentException"/>
    public bool Verify(ReadOnlySpan<byte> data, OpenSshEd25519SKSignature signature)
    {
        ArgumentNullException.ThrowIfNull(signature);
        if (signature.KeyTypeInfo != KeyTypeInfo)
            throw new ArgumentException("Incompatible signature.", nameof(signature));

        const int sha256HashLength = 32;
        Span<byte> signedData = stackalloc byte[sha256HashLength + 1 + 4 + sha256HashLength];
        _ = SHA256.HashData(Application.AsSpan(), signedData);
        signedData[sha256HashLength] = signature.Flags;
        BinaryPrimitives.WriteUInt32BigEndian(signedData.Slice(sha256HashLength + 1, 4), signature.Counter);
        _ = SHA256.HashData(data, signedData.Slice(sha256HashLength + 1 + 4));

        return Ed25519.VerifyData(PK.AsSpan(), signedData, signature.RS.AsSpan());
    }

    public bool Equals([NotNullWhen(true)] OpenSshEd25519SKKey? other, bool publicOnly)
    {
        if (other == null || !PublicEquals(other))
            return false;

        if (publicOnly)
            return true;

        return (HasDecryptedPrivateKey == other.HasDecryptedPrivateKey) &&
               (!HasDecryptedPrivateKey || PrivateEquals(other)) &&
               EqualityComparer<SshEncryptedPrivateKey>.Default.Equals(EncryptedPrivateKey, other.EncryptedPrivateKey);

        bool PublicEquals(OpenSshEd25519SKKey other)
        {
            return PK.SequenceEqual(other.PK) &&
                   Application.SequenceEqual(other.Application);
        }

        bool PrivateEquals(OpenSshEd25519SKKey other)
        {
            return _flags == other._flags &&
                   _keyHandle.ShieldedSpan.SequenceEqual(other._keyHandle.ShieldedSpan);
        }
    }

    public override bool Equals([NotNullWhen(true)] SshKey? other, bool publicOnly) => Equals(other as OpenSshEd25519SKKey, publicOnly);

    public override int GetHashCode(bool publicOnly)
    {
        var hashCode = new HashCode();
        hashCode.Add(PK);
        hashCode.AddBytes(Application.AsSpan());
        if (!publicOnly)
        {
            hashCode.Add(_flags);
            hashCode.AddBytes(_keyHandle.ShieldedSpan);
            hashCode.Add(EncryptedPrivateKey);
        }
        return hashCode.ToHashCode();
    }

    protected override SshKey WithEncryptedPrivateKey(SshEncryptedPrivateKey encryptedPrivateKey) => new OpenSshEd25519SKKey(this, encryptedPrivateKey);

    /// <exception cref="SshWireContentException"/>
    /// <exception cref="InvalidDataException"/>
    /// <exception cref="NotSupportedException"/>
    internal static OpenSshEd25519SKKey ReadPublicKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
    {
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L84-L85
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L2538-L2562

        var pk = reader.ReadByteString();
        var application = reader.ReadByteString();

        if (pk.Length != Ed25519.PublicKeyLength)
            throw new InvalidDataException("Invalid PK length.");

        return new OpenSshEd25519SKKey(keyTypeInfo, pk.ToImmutableArray(), application.ToImmutableArray());
    }

    /// <exception cref="SshWireContentException"/>
    /// <exception cref="InvalidDataException"/>
    internal static OpenSshEd25519SKKey ReadPrivateKey(SshKeyTypeInfo keyTypeInfo, ref SshWireReader reader)
    {
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/PROTOCOL.u2f#L90-L94
        // https://github.com/openssh/openssh-portable/blob/V_8_9_P1/sshkey.c#L3647-L3668

        var pk = reader.ReadByteString();
        var application = reader.ReadByteString();
        var flags = (OpenSshSKFlags)reader.ReadByte();
        var unshieldedKeyHandle = reader.ReadByteString();
        _ = reader.ReadByteString();

        if (pk.Length != Ed25519.PublicKeyLength)
            throw new InvalidDataException("Invalid PK length.");

        var keyHandle = ShieldedImmutableBuffer.Create(unshieldedKeyHandle);

        return new OpenSshEd25519SKKey(keyTypeInfo, pk.ToImmutableArray(), application.ToImmutableArray(), flags, keyHandle);
    }

    /// <exception cref="InvalidOperationException"/>
    private T GetPrivateKeyField<T>(in T field)
    {
        if (!HasDecryptedPrivateKey)
            throw new InvalidOperationException("Private key is not present or is not decrypted.");

        return field;
    }
}
