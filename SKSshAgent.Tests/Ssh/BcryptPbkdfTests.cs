﻿// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

/*
 * Copyright (c) 2013 Ted Unangst <tedu@openbsd.org>
 *
 * Permission to use, copy, modify, and distribute this software for any
 * purpose with or without fee is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
 * WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
 * ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
 * ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
 * OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
 */

using System.Text;
using Xunit;

namespace SKSshAgent.Ssh.Tests;

public static class BcryptPbkdfTests
{
    private static readonly Encoding _encoding = Encoding.GetEncoding(28591);

    public static readonly TheoryData<TestCase> Data = new()
    {
        /* basic */
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("password"),
            Salt = _encoding.GetBytes("salt"),
            KeyLength = 32,
            ExpectedKey = _encoding.GetBytes(
                "\x5b\xbf\x0c\xc2\x93\x58\x7f\x1c\x36\x35\x55\x5c\x27\x79\x65\x98" +
                "\xd4\x7e\x57\x90\x71\xbf\x42\x7e\x9d\x8f\xbe\x84\x2a\xba\x34\xd9"),
        },
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("password"),
            Salt = _encoding.GetBytes("\0"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\xc1\x2b\x56\x62\x35\xee\xe0\x4c\x21\x25\x98\x97\x0a\x57\x9a\x67"),
        },
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("\0"),
            Salt = _encoding.GetBytes("salt"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\x60\x51\xbe\x18\xc2\xf4\xf8\x2c\xbf\x0e\xfe\xe5\x47\x1b\x4b\xb9"),
        },
        /* nul bytes in password and string */
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("password\0"),
            Salt = _encoding.GetBytes("salt\0"),
            KeyLength = 32,
            ExpectedKey = _encoding.GetBytes(
                "\x74\x10\xe4\x4c\xf4\xfa\x07\xbf\xaa\xc8\xa9\x28\xb1\x72\x7f\xac" +
                "\x00\x13\x75\xe7\xbf\x73\x84\x37\x0f\x48\xef\xd1\x21\x74\x30\x50"),
        },
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("pass\0wor"),
            Salt = _encoding.GetBytes("sa\0l"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\xc2\xbf\xfd\x9d\xb3\x8f\x65\x69\xef\xef\x43\x72\xf4\xde\x83\xc0"),
        },
        new TestCase
        {
            Rounds = 4,
            Password = _encoding.GetBytes("pass\0word"),
            Salt = _encoding.GetBytes("sa\0lt"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\x4b\xa4\xac\x39\x25\xc0\xe8\xd7\xf0\xcd\xb6\xbb\x16\x84\xa5\x6f"),
        },
        /* bigger key */
        new TestCase
        {
            Rounds = 8,
            Password = _encoding.GetBytes("password"),
            Salt = _encoding.GetBytes("salt"),
            KeyLength = 64,
            ExpectedKey = _encoding.GetBytes(
                "\xe1\x36\x7e\xc5\x15\x1a\x33\xfa\xac\x4c\xc1\xc1\x44\xcd\x23\xfa" +
                "\x15\xd5\x54\x84\x93\xec\xc9\x9b\x9b\x5d\x9c\x0d\x3b\x27\xbe\xc7" +
                "\x62\x27\xea\x66\x08\x8b\x84\x9b\x20\xab\x7a\xa4\x78\x01\x02\x46" +
                "\xe7\x4b\xba\x51\x72\x3f\xef\xa9\xf9\x47\x4d\x65\x08\x84\x5e\x8d"),
        },
        /* more rounds */
        new TestCase
        {
            Rounds = 42,
            Password = _encoding.GetBytes("password"),
            Salt = _encoding.GetBytes("salt"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\x83\x3c\xf0\xdc\xf5\x6d\xb6\x56\x08\xe8\xf0\xdc\x0c\xe8\x82\xbd"),
        },
        /* longer password */
        new TestCase
        {
            Rounds = 8,
            Password = _encoding.GetBytes(
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do " +
                "eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut " +
                "enim ad minim veniam, quis nostrud exercitation ullamco laboris " +
                "nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor " +
                "in reprehenderit in voluptate velit esse cillum dolore eu fugiat " +
                "nulla pariatur. Excepteur sint occaecat cupidatat non proident, " +
                "sunt in culpa qui officia deserunt mollit anim id est laborum."),
            Salt = _encoding.GetBytes("salis\0"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\x10\x97\x8b\x07\x25\x3d\xf5\x7f\x71\xa1\x62\xeb\x0e\x8a\xd3\x0a"),
        },
        /* "unicode" */
        new TestCase
        {
            Rounds = 8,
            Password = _encoding.GetBytes(
                "\x0d\xb3\xac\x94\xb3\xee\x53\x28\x4f\x4a\x22\x89\x3b\x3c\x24\xae"),
            Salt = _encoding.GetBytes(
                "\x3a\x62\xf0\xf0\xdb\xce\xf8\x23\xcf\xcc\x85\x48\x56\xea\x10\x28"),
            KeyLength = 16,
            ExpectedKey = _encoding.GetBytes(
                "\x20\x44\x38\x17\x5e\xee\x7c\xe1\x36\xc9\x1b\x49\xa6\x79\x23\xff"),
        },
        /* very large key */
        new TestCase
        {
            Rounds = 8,
            Password = _encoding.GetBytes(
                "\x0d\xb3\xac\x94\xb3\xee\x53\x28\x4f\x4a\x22\x89\x3b\x3c\x24\xae"),
            Salt = _encoding.GetBytes(
                "\x3a\x62\xf0\xf0\xdb\xce\xf8\x23\xcf\xcc\x85\x48\x56\xea\x10\x28"),
            KeyLength = 256,
            ExpectedKey = _encoding.GetBytes(
                "\x20\x54\xb9\xff\xf3\x4e\x37\x21\x44\x03\x34\x74\x68\x28\xe9\xed" +
                "\x38\xde\x4b\x72\xe0\xa6\x9a\xdc\x17\x0a\x13\xb5\xe8\xd6\x46\x38" +
                "\x5e\xa4\x03\x4a\xe6\xd2\x66\x00\xee\x23\x32\xc5\xed\x40\xad\x55" +
                "\x7c\x86\xe3\x40\x3f\xbb\x30\xe4\xe1\xdc\x1a\xe0\x6b\x99\xa0\x71" +
                "\x36\x8f\x51\x8d\x2c\x42\x66\x51\xc9\xe7\xe4\x37\xfd\x6c\x91\x5b" +
                "\x1b\xbf\xc3\xa4\xce\xa7\x14\x91\x49\x0e\xa7\xaf\xb7\xdd\x02\x90" +
                "\xa6\x78\xa4\xf4\x41\x12\x8d\xb1\x79\x2e\xab\x27\x76\xb2\x1e\xb4" +
                "\x23\x8e\x07\x15\xad\xd4\x12\x7d\xff\x44\xe4\xb3\xe4\xcc\x4c\x4f" +
                "\x99\x70\x08\x3f\x3f\x74\xbd\x69\x88\x73\xfd\xf6\x48\x84\x4f\x75" +
                "\xc9\xbf\x7f\x9e\x0c\x4d\x9e\x5d\x89\xa7\x78\x39\x97\x49\x29\x66" +
                "\x61\x67\x07\x61\x1c\xb9\x01\xde\x31\xa1\x97\x26\xb6\xe0\x8c\x3a" +
                "\x80\x01\x66\x1f\x2d\x5c\x9d\xcc\x33\xb4\xaa\x07\x2f\x90\xdd\x0b" +
                "\x3f\x54\x8d\x5e\xeb\xa4\x21\x13\x97\xe2\xfb\x06\x2e\x52\x6e\x1d" +
                "\x68\xf4\x6a\x4c\xe2\x56\x18\x5b\x4b\xad\xc2\x68\x5f\xbe\x78\xe1" +
                "\xc7\x65\x7b\x59\xf8\x3a\xb9\xab\x80\xcf\x93\x18\xd6\xad\xd1\xf5" +
                "\x93\x3f\x12\xd6\xf3\x61\x82\xc8\xe8\x11\x5f\x68\x03\x0a\x12\x44"),
        },
    };

    [Theory]
    [MemberData(nameof(Data))]
    public static void Test(TestCase testCase)
    {
        byte[] key = new byte[testCase.KeyLength];
        BcryptPbkdf.DeriveKey(testCase.Password, testCase.Salt, key, testCase.Rounds);

        Assert.Equal(testCase.ExpectedKey, key);
    }

    public struct TestCase
    {
        public uint Rounds;
        public byte[] Password;
        public byte[] Salt;
        public int KeyLength;
        public byte[] ExpectedKey;
    }
}
