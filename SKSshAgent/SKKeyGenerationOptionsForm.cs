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
        private SshKeyTypeInfo _keyTypeInfo;

        private const string _applicationPrefix = "ssh:";

        private string _previousApplicationText = string.Empty;

        public SKKeyGenerationOptionsForm()
        {
            InitializeComponent();

            _ = _typeComboBox.Items.Add(SshKeyTypeInfo.SKEcdsaSha2NistP256KeyType);

            _keyTypeInfo = SshKeyTypeInfo.SKEcdsaSha2NistP256KeyType;
            _typeComboBox.SelectedItem = _keyTypeInfo;

            HandleTypeComboBoxSelectedIndexChanged(_typeComboBox, new EventArgs());

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
        }

        public FormResult? Result { get; private set; }

        private void HandleTypeComboBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            _keyTypeInfo = (SshKeyTypeInfo)_typeComboBox.SelectedItem;
        }

        private void HandleApplicationTextBoxEnter(object sender, EventArgs e)
        {
            if (_applicationTextBox.TextLength == 0)
            {
                _applicationTextBox.Text = _applicationPrefix;
                _applicationTextBox.Select(_applicationPrefix.Length, 0);
            }

            _previousApplicationText = _applicationTextBox.Text;
        }

        private void HandleApplicationTextBoxLeave(object sender, EventArgs e)
        {
            _previousApplicationText = string.Empty;

            if (_applicationTextBox.Text == _applicationPrefix)
                _applicationTextBox.ResetText();
        }

        private void HandleApplicationTextBoxTextChanged(object sender, EventArgs e)
        {
            if (!_applicationTextBox.Text.StartsWith(_applicationPrefix) &&
                _previousApplicationText.Length != 0)
            {
                SystemSounds.Beep.Play();

                _applicationTextBox.Text = _previousApplicationText;
                _applicationTextBox.Select(_applicationPrefix.Length, 0);
            }

            _previousApplicationText = _applicationTextBox.Text;
        }

        private void ApplicationTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            // Try to prevent the user from accidentally deleting the prefix.
            if (e.KeyCode == Keys.Delete &&
                _applicationTextBox.SelectionStart < _applicationPrefix.Length)
            {
                SystemSounds.Beep.Play();

                e.Handled = true;
            }
        }

        private void HandleApplicationTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            // Try to prevent the user from accidentally deleting the prefix.
            if (e.KeyChar == 8 &&
                (_applicationTextBox.SelectionStart < _applicationPrefix.Length ||
                 (_applicationTextBox.SelectionStart == _applicationPrefix.Length &&
                  _applicationTextBox.SelectionLength == 0)))
            {
                SystemSounds.Beep.Play();

                e.Handled = true;
            }
        }

        private void HandleGenerateButonClicked(object sender, EventArgs e)
        {
            string userName = _userIdTextBox.Text;

            byte[] userId = new byte[FormResult.UserIdLength];
            Encoding.UTF8.GetEncoder().Convert(userName, userId, flush: true, out _, out int bytesUsed, out bool completed);
            if (bytesUsed >= userId.Length || !completed)
            {
                _ = MessageBox.Show(this, "User ID must be less than 32 bytes.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            userId[userId.Length - 1] = 0;

            if (_applicationTextBox.TextLength > 0 && !_applicationTextBox.Text.StartsWith(_applicationPrefix))
                _applicationTextBox.Text = string.Empty;

            string application = _applicationTextBox.TextLength > 0
                ? _applicationTextBox.Text
                : _applicationPrefix;

            Result = new FormResult(
                keyTypeInfo: _keyTypeInfo,
                userVerificationRequired: _requireUserVerificationCheckBox.Checked,
                userName: userName,
                userId: userId,
                application: application,
                comment: _commentTextBox.Text);

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

            internal FormResult(SshKeyTypeInfo keyTypeInfo, bool userVerificationRequired, string userName, byte[] userId, string application, string comment)
            {
                KeyTypeInfo = keyTypeInfo;
                UserVerificationRequired = userVerificationRequired;
                UserName = userName;
                UserId = userId;
                Application = application;
                Comment = comment;
            }

            public SshKeyTypeInfo KeyTypeInfo { get; private set; }

            public bool UserVerificationRequired { get; private set; }

            public string UserName { get; private set; }

            public byte[] UserId { get; } = new byte[UserIdLength];

            public string Application { get; private set; }

            public string Comment { get; private set; }
        }
    }
}
