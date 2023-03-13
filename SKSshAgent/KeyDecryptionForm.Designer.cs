// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent
{
    partial class KeyDecryptionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.
        ///     </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with
        /// the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _fingerprintLabel = new Label();
            _fingerprintTextBox = new TextBox();
            _passwordLabel = new Label();
            _passwordTextBox = new PasswordTextBox();
            _decryptButton = new Button();
            _cancelButton = new Button();
            SuspendLayout();
            // 
            // _fingerprintLabel
            // 
            _fingerprintLabel.AutoSize = true;
            _fingerprintLabel.Location = new System.Drawing.Point(13, 15);
            _fingerprintLabel.Name = "_fingerprintLabel";
            _fingerprintLabel.Size = new System.Drawing.Size(68, 15);
            _fingerprintLabel.TabIndex = 4;
            _fingerprintLabel.Text = "Fingerprint:";
            // 
            // _fingerprintTextBox
            // 
            _fingerprintTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _fingerprintTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _fingerprintTextBox.Location = new System.Drawing.Point(87, 12);
            _fingerprintTextBox.Name = "_fingerprintTextBox";
            _fingerprintTextBox.ReadOnly = true;
            _fingerprintTextBox.Size = new System.Drawing.Size(360, 22);
            _fingerprintTextBox.TabIndex = 5;
            // 
            // _passwordLabel
            // 
            _passwordLabel.AutoSize = true;
            _passwordLabel.Location = new System.Drawing.Point(21, 44);
            _passwordLabel.Name = "_passwordLabel";
            _passwordLabel.Size = new System.Drawing.Size(60, 15);
            _passwordLabel.TabIndex = 0;
            _passwordLabel.Text = "&Password:";
            // 
            // _passwordTextBox
            // 
            _passwordTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _passwordTextBox.Location = new System.Drawing.Point(87, 41);
            _passwordTextBox.Name = "_passwordTextBox";
            _passwordTextBox.Size = new System.Drawing.Size(360, 23);
            _passwordTextBox.TabIndex = 1;
            _passwordTextBox.UseSystemPasswordChar = true;
            // 
            // _decryptButton
            // 
            _decryptButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _decryptButton.Location = new System.Drawing.Point(291, 76);
            _decryptButton.Name = "_decryptButton";
            _decryptButton.Size = new System.Drawing.Size(75, 23);
            _decryptButton.TabIndex = 2;
            _decryptButton.Text = "Decrypt";
            _decryptButton.UseVisualStyleBackColor = true;
            _decryptButton.Click += HandleDecryptButtonClicked;
            // 
            // _cancelButton
            // 
            _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _cancelButton.Location = new System.Drawing.Point(372, 76);
            _cancelButton.Name = "_cancelButton";
            _cancelButton.Size = new System.Drawing.Size(75, 23);
            _cancelButton.TabIndex = 3;
            _cancelButton.Text = "Cancel";
            _cancelButton.UseVisualStyleBackColor = true;
            // 
            // KeyDecryptionForm
            // 
            AcceptButton = _decryptButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _cancelButton;
            ClientSize = new System.Drawing.Size(459, 111);
            Controls.Add(_fingerprintTextBox);
            Controls.Add(_fingerprintLabel);
            Controls.Add(_decryptButton);
            Controls.Add(_cancelButton);
            Controls.Add(_passwordLabel);
            Controls.Add(_passwordTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = Properties.Resources.console_ssh_key_icon;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "KeyDecryptionForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Decrypt Key";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _fingerprintLabel;
        private System.Windows.Forms.TextBox _fingerprintTextBox;
        private System.Windows.Forms.Label _passwordLabel;
        private SKSshAgent.PasswordTextBox _passwordTextBox;
        private System.Windows.Forms.Button _decryptButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}
