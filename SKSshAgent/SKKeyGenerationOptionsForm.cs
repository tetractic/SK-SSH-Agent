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

        private const string _applicationIdPrefix = "ssh:";

        private string _previousApplicationIdText = string.Empty;

        public SKKeyGenerationOptionsForm()
        {
            InitializeComponent();

            _ = _typeComboBox.Items.Add(SshKeyTypeInfo.SKEcdsaSha2NistP256);

            _keyTypeInfo = SshKeyTypeInfo.SKEcdsaSha2NistP256;
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

        private void ApplicationIdTextBoxKeyDown(object sender, KeyEventArgs e)
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

            if (_applicationIdTextBox.TextLength > 0 && !_applicationIdTextBox.Text.StartsWith(_applicationIdPrefix))
                _applicationIdTextBox.Text = string.Empty;

            string applicationId = _applicationIdTextBox.TextLength > 0
                ? _applicationIdTextBox.Text
                : _applicationIdPrefix;

            Result = new FormResult(
                keyTypeInfo: _keyTypeInfo,
                userVerificationRequired: _requireUserVerificationCheckBox.Checked,
                userName: userName,
                userId: userId,
                applicationId: applicationId,
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

            internal FormResult(SshKeyTypeInfo keyTypeInfo, bool userVerificationRequired, string userName, byte[] userId, string applicationId, string comment)
            {
                KeyTypeInfo = keyTypeInfo;
                UserVerificationRequired = userVerificationRequired;
                UserName = userName;
                UserId = userId;
                ApplicationId = applicationId;
                Comment = comment;
            }

            public SshKeyTypeInfo KeyTypeInfo { get; private set; }

            public bool UserVerificationRequired { get; private set; }

            public string UserName { get; private set; }

            public byte[] UserId { get; } = new byte[UserIdLength];

            public string ApplicationId { get; private set; }

            public string Comment { get; private set; }
        }
    }
}
