// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

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
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._fingerprintLabel = new System.Windows.Forms.Label();
            this._fingerprintTextBox = new System.Windows.Forms.TextBox();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._passwordTextBox = new SKSshAgent.PasswordTextBox();
            this._decryptButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _fingerprintLabel
            // 
            this._fingerprintLabel.AutoSize = true;
            this._fingerprintLabel.Location = new System.Drawing.Point(13, 15);
            this._fingerprintLabel.Name = "_fingerprintLabel";
            this._fingerprintLabel.Size = new System.Drawing.Size(68, 15);
            this._fingerprintLabel.TabIndex = 4;
            this._fingerprintLabel.Text = "Fingerprint:";
            // 
            // _fingerprintTextBox
            // 
            this._fingerprintTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._fingerprintTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._fingerprintTextBox.Location = new System.Drawing.Point(87, 12);
            this._fingerprintTextBox.Name = "_fingerprintTextBox";
            this._fingerprintTextBox.ReadOnly = true;
            this._fingerprintTextBox.Size = new System.Drawing.Size(360, 22);
            this._fingerprintTextBox.TabIndex = 5;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(21, 44);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(60, 15);
            this._passwordLabel.TabIndex = 0;
            this._passwordLabel.Text = "&Password:";
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._passwordTextBox.Location = new System.Drawing.Point(87, 41);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.Size = new System.Drawing.Size(360, 23);
            this._passwordTextBox.TabIndex = 1;
            this._passwordTextBox.UseSystemPasswordChar = true;
            // 
            // _decryptButton
            // 
            this._decryptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._decryptButton.Location = new System.Drawing.Point(291, 76);
            this._decryptButton.Name = "_decryptButton";
            this._decryptButton.Size = new System.Drawing.Size(75, 23);
            this._decryptButton.TabIndex = 2;
            this._decryptButton.Text = "Decrypt";
            this._decryptButton.UseVisualStyleBackColor = true;
            this._decryptButton.Click += new System.EventHandler(this.HandleDecryptButtonClicked);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(372, 76);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 3;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // KeyDecryptionForm
            // 
            this.AcceptButton = this._decryptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(459, 111);
            this.Controls.Add(this._fingerprintTextBox);
            this.Controls.Add(this._fingerprintLabel);
            this.Controls.Add(this._decryptButton);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._passwordLabel);
            this.Controls.Add(this._passwordTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::SKSshAgent.Properties.Resources.console_ssh_key_icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyDecryptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Decrypt Key";
            this.ResumeLayout(false);
            this.PerformLayout();

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
