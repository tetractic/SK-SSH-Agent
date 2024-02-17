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
    internal partial class KeyGenerationOptionsForm : Form
    {
        private SshKdfInfo _kdfInfo;

        private uint _kdfRounds;

        private SshCipherInfo _cipherInfo;

        public KeyGenerationOptionsForm()
        {
            InitializeComponent();

            _ = _keyTypeComboBox.Items.Add(SshKeyTypeInfo.EcdsaSha2NistP256);
            _ = _keyTypeComboBox.Items.Add(SshKeyTypeInfo.EcdsaSha2NistP384);
            _ = _keyTypeComboBox.Items.Add(SshKeyTypeInfo.EcdsaSha2NistP521);
            _ = _keyTypeComboBox.Items.Add(SshKeyTypeInfo.Ed25519);

            _keyTypeComboBox.SelectedItem = SshKeyTypeInfo.EcdsaSha2NistP256;

            string comment = Environment.UserName;
            try
            {
                comment += "@" + Environment.MachineName;
            }
            catch (InvalidOperationException)
            {
                // Nothing to be done about it.
            }
            _commentTextBox.Text = comment;

            _kdfInfo = SshKdfInfo.Bcrypt;

            _kdfRounds = SshKdfInfo.GetDefaultBcryptRounds();

            _cipherInfo = OpenSshAesGcmCipher.IsSupported
                ? SshCipherInfo.OpenSshAes256Gcm
                : SshCipherInfo.Aes256Ctr;
        }

        public FormResult? Result { get; private set; }

        private void HandleEncryptCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            bool encrypt = _encryptCheckBox.Checked;
            _passwordTextBox.Enabled = encrypt;
            _confirmPasswordTextBox.Enabled = encrypt;
            _encryptionOptionsButton.Enabled = encrypt;
        }

        private void HandleEncryptionOptionsButtonClicked(object sender, EventArgs e)
        {
            var dialog = new KeyEncryptionOptionsForm()
            {
                KdfInfo = _kdfInfo,
                KdfRounds = _kdfRounds,
                CipherInfo = _cipherInfo,
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            _kdfInfo = dialog.KdfInfo;
            _kdfRounds = dialog.KdfRounds;
            _cipherInfo = dialog.CipherInfo;
        }

        private void HandleGenerateButtonClicked(object sender, EventArgs e)
        {
            var keyTypeInfo = (SshKeyTypeInfo)_keyTypeComboBox.SelectedItem!;

            ShieldedImmutableBuffer password = ShieldedImmutableBuffer.Empty;
            SshKdfInfo kdfInfo = SshKdfInfo.None;
            uint kdfRounds = 0;
            SshCipherInfo cipherInfo = SshCipherInfo.None;

            if (_encryptCheckBox.Checked)
            {
                password = _passwordTextBox.GetPassword();
                var passwordConfirmation = _confirmPasswordTextBox.GetPassword();

                if (!password.ShieldedSpan.SequenceEqual(passwordConfirmation.ShieldedSpan))
                {
                    _ = _passwordTextBox.Focus();

                    _ = MessageBox.Show(this, "Password confirmation does not match.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_passwordTextBox.TextLength == 0)
                {
                    _ = _passwordTextBox.Focus();

                    _ = MessageBox.Show(this, "Password cannot be empty.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                kdfInfo = _kdfInfo;
                kdfRounds = _kdfRounds;
                cipherInfo = _cipherInfo;
            }

            Result = new FormResult(
                keyTypeInfo: keyTypeInfo,
                comment: _commentTextBox.Text,
                password: password,
                kdfInfo: kdfInfo,
                kdfRounds: kdfRounds,
                cipherInfo: cipherInfo);

            _passwordTextBox.ZeroMemory();
            _confirmPasswordTextBox.ZeroMemory();

            DialogResult = DialogResult.OK;
        }

        internal sealed class FormResult
        {
            internal FormResult(SshKeyTypeInfo keyTypeInfo, string comment, ShieldedImmutableBuffer password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
            {
                KeyTypeInfo = keyTypeInfo;
                Comment = comment;
                Password = password;
                KdfInfo = kdfInfo;
                KdfRounds = kdfRounds;
                CipherInfo = cipherInfo;
            }

            public SshKeyTypeInfo KeyTypeInfo { get; }

            public string Comment { get; }

            public ShieldedImmutableBuffer Password { get; }

            public SshKdfInfo KdfInfo { get; }

            public uint KdfRounds { get; }

            public SshCipherInfo CipherInfo { get; }
        }
    }
}
