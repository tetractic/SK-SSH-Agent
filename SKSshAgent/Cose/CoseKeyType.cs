// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent.Cose;

/// <seealso href="https://datatracker.ietf.org/doc/html/rfc8152#section-13"/>
/// <seealso href="https://www.iana.org/assignments/cose/cose.xhtml#key-type"/>
internal enum CoseKeyType
{
    Okp = 1,
    EC2 = 2,
}
