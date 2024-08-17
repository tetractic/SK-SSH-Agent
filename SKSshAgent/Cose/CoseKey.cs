// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent.Cose;

internal abstract class CoseKey
{
    private protected CoseKey(CoseKeyType keyType, CoseAlgorithm algorithm)
    {
        KeyType = keyType;
        Algorithm = algorithm;
    }

    public CoseKeyType KeyType { get; }

    public CoseAlgorithm Algorithm { get; }
}
