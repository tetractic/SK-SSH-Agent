// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Ssh;
using System;
using System.Windows.Forms;

namespace SKSshAgent
{
    internal partial class KeyEncryptionOptionsForm : Form
    {
        public KeyEncryptionOptionsForm()
        {
            InitializeComponent();

            foreach (var kdfInfo in SshKdfInfo.KdfInfos)
                if (kdfInfo != SshKdfInfo.None)
                    _ = _kdfComboBox.Items.Add(kdfInfo);

            _kdfComboBox.SelectedItem = _kdfComboBox.Items[0];

            foreach (var cipherInfo in SshCipherInfo.CipherInfos)
                if (cipherInfo != SshCipherInfo.None)
                    _ = _cipherComboBox.Items.Add(cipherInfo);

            _cipherComboBox.SelectedItem = _cipherComboBox.Items[0];
        }

        /// <exception cref="ArgumentNullException" accessor="set"/>
        /// <exception cref="ArgumentException" accessor="set"/>
        public SshKdfInfo KdfInfo
        {
            get => (SshKdfInfo)_kdfComboBox.SelectedItem!;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (!_kdfComboBox.Items.Contains(value))
                    throw new ArgumentException("Invalid KDF.", nameof(value));

                _kdfComboBox.SelectedItem = value;
            }
        }

        public uint KdfRounds
        {
            get => (uint)_kdfRoundsNumericUpDown.Value;
            set => _kdfRoundsNumericUpDown.Value = value;
        }

        /// <exception cref="ArgumentNullException" accessor="set"/>
        /// <exception cref="ArgumentException" accessor="set"/>
        public SshCipherInfo CipherInfo
        {
            get => (SshCipherInfo)_cipherComboBox.SelectedItem!;
            set
            {
                ArgumentNullException.ThrowIfNull(value);
                if (!_cipherComboBox.Items.Contains(value))
                    throw new ArgumentException("Invalid cipher.", nameof(value));

                _cipherComboBox.SelectedItem = value;
            }
        }

        private void HandleOkayButtonClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
