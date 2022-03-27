// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent
{
    internal class PasswordTextBox : TextBox
    {
        public PasswordTextBox()
        {
            UseSystemPasswordChar = true;
        }

        // TODO: Encrypt text or at least store it in a pinned array that can be cleared.
    }
}
