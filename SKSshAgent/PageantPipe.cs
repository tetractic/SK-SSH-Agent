// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using Windows.Win32.Security.Authentication.Identity;
using static Windows.Win32.PInvoke;

namespace SKSshAgent;

internal sealed class PageantPipe : SshAgentPipe
{
    private static readonly string _pipeName = GetPipeName();

    public PageantPipe(KeyListForm form)
        : base(form)
    {
    }

    public override string PipeName => _pipeName;

    private static string GetPipeName()
    {
        string? username = GetPrincipalUserNameOrUserName();

        string suffix = "Pageant";

        uint length = (uint)Encoding.UTF8.GetByteCount(suffix) + 1;
        const uint blockSize = CRYPTPROTECTMEMORY_BLOCK_SIZE;
        length += blockSize - 1;
        length -= length % blockSize;
        byte[] buffer = new byte[4 + length];
        BinaryPrimitives.WriteUInt32BigEndian(buffer, length);
        int bytesEncoded = Encoding.UTF8.GetBytes(suffix, buffer.AsSpan(4));
        Debug.Assert(bytesEncoded == 7);

        unsafe
        {
            fixed (byte* bufferPtr = &buffer[4])
            {
                bool success = CryptProtectMemory(bufferPtr, length, CRYPTPROTECTMEMORY_CROSS_PROCESS);
                Debug.Assert(success);
            }
        }

        Span<byte> hash = stackalloc byte[32];
        bool hashWritten = SHA256.TryHashData(buffer, hash, out _);
        Debug.Assert(hashWritten);

        suffix = Convert.ToHexString(hash);

        return $"pageant.{username}.{suffix}";
    }

    private static string? GetPrincipalUserNameOrUserName()
    {
        if (TryGetPrincipalUserName(out string? principalUserName))
            return principalUserName;

        // Pageant only allows up to 256 characters.
        uint length = 256;
        Span<char> buffer = stackalloc char[(int)length];
        unsafe
        {
            fixed (char* bufferPtr = buffer)
            {
                if (GetUserName(bufferPtr, ref length) &&
                    length > 0)
                {
                    return new(buffer.Slice(0, (int)length - 1));
                }
            }
        }

        return null;
    }

    private static bool TryGetPrincipalUserName([MaybeNullWhen(false)] out string userName)
    {
        uint length = 0;
        _ = GetUserNameEx(EXTENDED_NAME_FORMAT.NameUserPrincipal, null, ref length).Value;

        char[] buffer = new char[length];
        unsafe
        {
            fixed (char* bufferPtr = buffer)
            {
                if (GetUserNameEx(EXTENDED_NAME_FORMAT.NameUserPrincipal, bufferPtr, ref length) != 0)
                {
                    userName = new(buffer, 0, (int)length);

                    int index = userName.IndexOf('@');
                    if (index >= 0)
                        userName = userName.Substring(0, index);

                    return true;
                }
            }
        }

        userName = default;
        return false;
    }
}
