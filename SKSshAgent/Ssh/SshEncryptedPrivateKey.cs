// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SKSshAgent.Ssh;

internal abstract class SshEncryptedPrivateKey : IEquatable<SshEncryptedPrivateKey>
{
    /// <exception cref="ArgumentException"/>
    /// <exception cref="SshWireContentException"/>
    /// <exception cref="InvalidDataException"/>
    /// <exception cref="NotSupportedException"/>
    /// <exception cref="System.Security.Cryptography.CryptographicException"/>
    public abstract bool TryDecrypt(ShieldedImmutableBuffer password, [MaybeNullWhen(false)] out SshKey privateKey, [MaybeNullWhen(false)] out string comment);

    public abstract bool Equals([NotNullWhen(true)] SshEncryptedPrivateKey? other);

    public sealed override bool Equals([NotNullWhen(true)] object? obj) => Equals(obj as SshEncryptedPrivateKey);

    public abstract override int GetHashCode();
}
