// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent.Cose;

internal abstract class CoseSignature
{
    private protected CoseSignature(CoseAlgorithm algorithm)
    {
        Algorithm = algorithm;
    }

    public CoseAlgorithm Algorithm { get; }
}
