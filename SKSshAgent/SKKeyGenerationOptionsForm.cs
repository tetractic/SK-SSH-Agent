// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Ssh;
using System;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace SKSshAgent
{
    internal partial class SKKeyGenerationOptionsForm : Form
    {
        private const string _applicationIdPrefix = "ssh:";

        private string _previousApplicationIdText = string.Empty;

        private SshKdfInfo _kdfInfo;

        private uint _kdfRounds;

        private SshCipherInfo _cipherInfo;

        public SKKeyGenerationOptionsForm()
        {
            InitializeComponent();

            _ = _keyTypeComboBox.Items.Add(SshKeyTypeInfo.SKEcdsaSha2NistP256);

            _keyTypeComboBox.SelectedItem = SshKeyTypeInfo.SKEcdsaSha2NistP256;

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

            _kdfRounds = 16;

            _cipherInfo = OpenSshAesGcmCipher.IsSupported
                ? SshCipherInfo.OpenSshAes256Gcm
                : SshCipherInfo.Aes256Ctr;
        }

        public FormResult? Result { get; private set; }

        private void HandleApplicationIdTextBoxEnter(object sender, EventArgs e)
        {
            if (_applicationIdTextBox.TextLength == 0)
            {
                _applicationIdTextBox.Text = _applicationIdPrefix;
                _applicationIdTextBox.Select(_applicationIdPrefix.Length, 0);
            }

            _previousApplicationIdText = _applicationIdTextBox.Text;
        }

        private void HandleApplicationIdTextBoxLeave(object sender, EventArgs e)
        {
            _previousApplicationIdText = string.Empty;

            if (_applicationIdTextBox.Text == _applicationIdPrefix)
                _applicationIdTextBox.ResetText();
        }

        private void HandleApplicationIdTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!_applicationIdTextBox.Text.StartsWith(_applicationIdPrefix) &&
                _previousApplicationIdText.Length != 0)
            {
                SystemSounds.Beep.Play();

                _applicationIdTextBox.Text = _previousApplicationIdText;
                _applicationIdTextBox.Select(_applicationIdPrefix.Length, 0);
            }

            _previousApplicationIdText = _applicationIdTextBox.Text;
        }

        private void HandleApplicationIdTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            // Try to prevent the user from accidentally deleting the prefix.
            if (e.KeyCode == Keys.Delete &&
                _applicationIdTextBox.SelectionStart < _applicationIdPrefix.Length)
            {
                SystemSounds.Beep.Play();

                e.Handled = true;
            }
        }

        private void HandleApplicationIdTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            // Try to prevent the user from accidentally deleting the prefix.
            if (e.KeyChar == 8 &&
                (_applicationIdTextBox.SelectionStart < _applicationIdPrefix.Length ||
                 (_applicationIdTextBox.SelectionStart == _applicationIdPrefix.Length &&
                  _applicationIdTextBox.SelectionLength == 0)))
            {
                SystemSounds.Beep.Play();

                e.Handled = true;
            }
        }

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

        private void HandleGenerateButonClicked(object sender, EventArgs e)
        {
            var keyTypeInfo = (SshKeyTypeInfo)_keyTypeComboBox.SelectedItem;

            string userName = _userIdTextBox.Text;

            byte[] userId = new byte[FormResult.UserIdLength];
            Encoding.UTF8.GetEncoder().Convert(userName, userId, flush: true, out _, out int bytesUsed, out bool completed);
            if (bytesUsed >= userId.Length || !completed)
            {
                _ = _userIdTextBox.Focus();

                _ = MessageBox.Show(this, "User ID must be less than 32 bytes.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            userId[userId.Length - 1] = 0;

            if (_applicationIdTextBox.TextLength > 0 && !_applicationIdTextBox.Text.StartsWith(_applicationIdPrefix))
                _applicationIdTextBox.Text = string.Empty;

            string applicationId = _applicationIdTextBox.TextLength > 0
                ? _applicationIdTextBox.Text
                : _applicationIdPrefix;

            ShieldedImmutableBuffer password = ShieldedImmutableBuffer.Empty;
            SshKdfInfo kdfInfo = SshKdfInfo.None;
            uint kdfRounds = 0;
            SshCipherInfo cipherInfo = SshCipherInfo.None;

            if (_encryptCheckBox.Checked)
            {
                if (_passwordTextBox.Text != _confirmPasswordTextBox.Text)
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

                int passwordLength = Encoding.UTF8.GetByteCount(_passwordTextBox.Text);
                password = ShieldedImmutableBuffer.Create(passwordLength, _passwordTextBox.Text.AsSpan(), (source, buffer) => Encoding.UTF8.GetBytes(source, buffer));

                kdfInfo = _kdfInfo;
                kdfRounds = _kdfRounds;
                cipherInfo = _cipherInfo;
            }

            Result = new FormResult(
                keyTypeInfo: keyTypeInfo,
                userVerificationRequired: _requireUserVerificationCheckBox.Checked,
                userName: userName,
                userId: userId,
                applicationId: applicationId,
                comment: _commentTextBox.Text,
                password: password,
                kdfInfo: kdfInfo,
                kdfRounds: kdfRounds,
                cipherInfo: cipherInfo);

            DialogResult = DialogResult.OK;
        }

        private void ShowToolTip(object sender, EventArgs e)
        {
            var control = (Control)sender;
            string text = _toolTip.GetToolTip(control);
            _toolTip.Show(string.Empty, control, 0);
            _toolTip.Show(text, control, 30000);
            _toolTip.SetToolTip(control, text);
        }

        private void HideToolTip(object sender, EventArgs e)
        {
            var control = (Control)sender;
            _toolTip.Hide(control);
        }

        internal sealed class FormResult
        {
            internal const int UserIdLength = 32;  // Including NUL terminator.

            internal FormResult(SshKeyTypeInfo keyTypeInfo, bool userVerificationRequired, string userName, byte[] userId, string applicationId, string comment, ShieldedImmutableBuffer password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
            {
                KeyTypeInfo = keyTypeInfo;
                UserVerificationRequired = userVerificationRequired;
                UserName = userName;
                UserId = userId;
                ApplicationId = applicationId;
                Comment = comment;
                Password = password;
                KdfInfo = kdfInfo;
                KdfRounds = kdfRounds;
                CipherInfo = cipherInfo;
            }

            public SshKeyTypeInfo KeyTypeInfo { get; }

            public bool UserVerificationRequired { get; }

            public string UserName { get; }

            public byte[] UserId { get; } = new byte[UserIdLength];

            public string ApplicationId { get; }

            public string Comment { get; }

            public ShieldedImmutableBuffer Password { get; }

            public SshKdfInfo KdfInfo { get; }

            public uint KdfRounds { get; }

            public SshCipherInfo CipherInfo { get; }
        }
    }
}
