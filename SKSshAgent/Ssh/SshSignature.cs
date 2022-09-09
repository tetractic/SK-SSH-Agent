// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;

namespace SKSshAgent.Ssh
{
    internal abstract class SshSignature
    {
        /// <exception cref="ArgumentNullException"/>
        protected SshSignature(SshKeyTypeInfo keyTypeInfo)
        {
            if (keyTypeInfo is null)
                throw new ArgumentNullException(nameof(keyTypeInfo));

            KeyTypeInfo = keyTypeInfo;
        }

        public SshKeyTypeInfo KeyTypeInfo { get; }

        public abstract void WriteTo(ref SshWireWriter writer);
    }
}
