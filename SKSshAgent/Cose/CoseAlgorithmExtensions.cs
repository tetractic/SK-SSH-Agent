// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Security.Cryptography;

namespace SKSshAgent.Cose
{
    internal static class CoseAlgorithmExtensions
    {
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static HashAlgorithmName GetHashAlgorithmName(this CoseAlgorithm algorithm)
        {
            return algorithm switch
            {
                CoseAlgorithm.ES256 => HashAlgorithmName.SHA256,
                CoseAlgorithm.ES384 => HashAlgorithmName.SHA384,
                CoseAlgorithm.ES512 => HashAlgorithmName.SHA512,
                _ => throw new ArgumentOutOfRangeException(nameof(algorithm)),
            };
        }
    }
}
