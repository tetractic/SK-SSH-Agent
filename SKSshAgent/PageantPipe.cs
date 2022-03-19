// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Windows.Win32.Foundation;
using static Windows.Win32.PInvoke;

namespace SKSshAgent
{
    internal sealed class PageantPipe : SshAgentPipe
    {
        private static readonly string _pipeName = GetPipeName();

        public PageantPipe(HWND hWnd)
            : base(hWnd)
        {
        }

        public override string PipeName => _pipeName;

        private static string GetPipeName()
        {
            string username = Environment.UserName;
            int index = username.IndexOf('@');
            if (index >= 0)
                username = username.Substring(0, index);

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
    }
}
