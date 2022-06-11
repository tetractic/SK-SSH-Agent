// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent
{
    partial class KeyGenerationOptionsForm
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
            this._keyTypeLabel = new System.Windows.Forms.Label();
            this._keyTypeComboBox = new System.Windows.Forms.ComboBox();
            this._commentLabel = new System.Windows.Forms.Label();
            this._commentTextBox = new System.Windows.Forms.TextBox();
            this._encryptCheckBox = new System.Windows.Forms.CheckBox();
            this._encryptionGroupBox = new System.Windows.Forms.GroupBox();
            this._passwordLabel = new System.Windows.Forms.Label();
            this._passwordTextBox = new SKSshAgent.PasswordTextBox();
            this._confirmPasswordLabel = new System.Windows.Forms.Label();
            this._confirmPasswordTextBox = new SKSshAgent.PasswordTextBox();
            this._encryptionOptionsButton = new System.Windows.Forms.Button();
            this._generateButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._encryptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _keyTypeLabel
            // 
            this._keyTypeLabel.AutoSize = true;
            this._keyTypeLabel.Location = new System.Drawing.Point(41, 15);
            this._keyTypeLabel.Name = "_keyTypeLabel";
            this._keyTypeLabel.Size = new System.Drawing.Size(56, 15);
            this._keyTypeLabel.TabIndex = 0;
            this._keyTypeLabel.Text = "Key &Type:";
            this._keyTypeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _keyTypeComboBox
            // 
            this._keyTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyTypeComboBox.DisplayMember = "Name";
            this._keyTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._keyTypeComboBox.FormattingEnabled = true;
            this._keyTypeComboBox.Location = new System.Drawing.Point(103, 12);
            this._keyTypeComboBox.Name = "_keyTypeComboBox";
            this._keyTypeComboBox.Size = new System.Drawing.Size(269, 23);
            this._keyTypeComboBox.TabIndex = 1;
            // 
            // _commentLabel
            // 
            this._commentLabel.AutoSize = true;
            this._commentLabel.Location = new System.Drawing.Point(36, 44);
            this._commentLabel.Name = "_commentLabel";
            this._commentLabel.Size = new System.Drawing.Size(61, 15);
            this._commentLabel.TabIndex = 7;
            this._commentLabel.Text = "&Comment";
            // 
            // _commentTextBox
            // 
            this._commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commentTextBox.Location = new System.Drawing.Point(103, 41);
            this._commentTextBox.Name = "_commentTextBox";
            this._commentTextBox.Size = new System.Drawing.Size(269, 23);
            this._commentTextBox.TabIndex = 8;
            // 
            // _encryptCheckBox
            // 
            this._encryptCheckBox.AutoSize = true;
            this._encryptCheckBox.Checked = true;
            this._encryptCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._encryptCheckBox.Location = new System.Drawing.Point(21, 69);
            this._encryptCheckBox.Name = "_encryptCheckBox";
            this._encryptCheckBox.Size = new System.Drawing.Size(66, 19);
            this._encryptCheckBox.TabIndex = 9;
            this._encryptCheckBox.Text = "&Encrypt";
            this._encryptCheckBox.CheckedChanged += new System.EventHandler(this.HandleEncryptCheckBoxCheckedChanged);
            // 
            // _encryptionGroupBox
            // 
            this._encryptionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._encryptionGroupBox.Controls.Add(this._passwordLabel);
            this._encryptionGroupBox.Controls.Add(this._passwordTextBox);
            this._encryptionGroupBox.Controls.Add(this._confirmPasswordLabel);
            this._encryptionGroupBox.Controls.Add(this._confirmPasswordTextBox);
            this._encryptionGroupBox.Controls.Add(this._encryptionOptionsButton);
            this._encryptionGroupBox.Location = new System.Drawing.Point(12, 70);
            this._encryptionGroupBox.Name = "_encryptionGroupBox";
            this._encryptionGroupBox.Size = new System.Drawing.Size(360, 110);
            this._encryptionGroupBox.TabIndex = 10;
            this._encryptionGroupBox.TabStop = false;
            // 
            // _passwordLabel
            // 
            this._passwordLabel.AutoSize = true;
            this._passwordLabel.Location = new System.Drawing.Point(9, 25);
            this._passwordLabel.Name = "_passwordLabel";
            this._passwordLabel.Size = new System.Drawing.Size(60, 15);
            this._passwordLabel.TabIndex = 0;
            this._passwordLabel.Text = "&Password:";
            this._passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _passwordTextBox
            // 
            this._passwordTextBox.Location = new System.Drawing.Point(75, 22);
            this._passwordTextBox.Name = "_passwordTextBox";
            this._passwordTextBox.Size = new System.Drawing.Size(279, 23);
            this._passwordTextBox.TabIndex = 1;
            this._passwordTextBox.UseSystemPasswordChar = true;
            // 
            // _confirmPasswordLabel
            // 
            this._confirmPasswordLabel.AutoSize = true;
            this._confirmPasswordLabel.Location = new System.Drawing.Point(15, 54);
            this._confirmPasswordLabel.Name = "_confirmPasswordLabel";
            this._confirmPasswordLabel.Size = new System.Drawing.Size(54, 15);
            this._confirmPasswordLabel.TabIndex = 2;
            this._confirmPasswordLabel.Text = "Con&firm:";
            this._confirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _confirmPasswordTextBox
            // 
            this._confirmPasswordTextBox.Location = new System.Drawing.Point(75, 51);
            this._confirmPasswordTextBox.Name = "_confirmPasswordTextBox";
            this._confirmPasswordTextBox.Size = new System.Drawing.Size(279, 23);
            this._confirmPasswordTextBox.TabIndex = 3;
            this._confirmPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // _encryptionOptionsButton
            // 
            this._encryptionOptionsButton.Location = new System.Drawing.Point(279, 80);
            this._encryptionOptionsButton.Name = "_encryptionOptionsButton";
            this._encryptionOptionsButton.Size = new System.Drawing.Size(75, 23);
            this._encryptionOptionsButton.TabIndex = 4;
            this._encryptionOptionsButton.Text = "&Options...";
            this._encryptionOptionsButton.UseVisualStyleBackColor = true;
            this._encryptionOptionsButton.Click += new System.EventHandler(this.HandleEncryptionOptionsButtonClicked);
            // 
            // _generateButton
            // 
            this._generateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._generateButton.Location = new System.Drawing.Point(216, 188);
            this._generateButton.Name = "_generateButton";
            this._generateButton.Size = new System.Drawing.Size(75, 23);
            this._generateButton.TabIndex = 11;
            this._generateButton.Text = "&Generate";
            this._generateButton.UseVisualStyleBackColor = true;
            this._generateButton.Click += new System.EventHandler(this.HandleGenerateButtonClicked);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(297, 188);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // KeyGenerationOptionsForm
            // 
            this.AcceptButton = this._generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 223);
            this.Controls.Add(this._encryptCheckBox);
            this.Controls.Add(this._encryptionGroupBox);
            this.Controls.Add(this._commentLabel);
            this.Controls.Add(this._commentTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._generateButton);
            this.Controls.Add(this._keyTypeComboBox);
            this.Controls.Add(this._keyTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyGenerationOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Generation Options";
            this._encryptionGroupBox.ResumeLayout(false);
            this._encryptionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _keyTypeLabel;
        private System.Windows.Forms.ComboBox _keyTypeComboBox;
        private System.Windows.Forms.Label _commentLabel;
        private System.Windows.Forms.TextBox _commentTextBox;
        private System.Windows.Forms.CheckBox _encryptCheckBox;
        private System.Windows.Forms.GroupBox _encryptionGroupBox;
        private System.Windows.Forms.Label _passwordLabel;
        private SKSshAgent.PasswordTextBox _passwordTextBox;
        private System.Windows.Forms.Label _confirmPasswordLabel;
        private SKSshAgent.PasswordTextBox _confirmPasswordTextBox;
        private System.Windows.Forms.Button _encryptionOptionsButton;
        private System.Windows.Forms.Button _generateButton;
        private System.Windows.Forms.Button _cancelButton;
    }
}
