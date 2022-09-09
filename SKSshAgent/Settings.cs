// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using Microsoft.Win32;
using System.Diagnostics;

namespace SKSshAgent
{
    internal static class Settings
    {
        private const string _registryPath = @"HKEY_CURRENT_USER\SOFTWARE\SK SSH Agent";

        private const string _confirmEachKeyUseName = "ConfirmEachKeyUse";

        private static bool? _confirmEachKeyUse;

        public static bool ConfirmEachKeyUse
        {
            get
            {
                try
                {
                    _confirmEachKeyUse ??= Registry.GetValue(_registryPath, _confirmEachKeyUseName, null) is int value ? value != 0 : null;
                }
                catch
                {
                    Debug.Assert(false);
                }

                return _confirmEachKeyUse.GetValueOrDefault(true);
            }
            set
            {
                _confirmEachKeyUse = value;

                try
                {
                    Registry.SetValue(_registryPath, _confirmEachKeyUseName, value ? 1 : 0);
                }
                catch
                {
                    Debug.Assert(false);
                }
            }
        }
    }
}
