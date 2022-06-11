// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent
{
    partial class SKKeyGenerationOptionsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SKKeyGenerationOptionsForm));
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this._requireUserVerificationInfoIcon = new System.Windows.Forms.PictureBox();
            this._applicationIdInfoIcon = new System.Windows.Forms.PictureBox();
            this._keyTypeLabel = new System.Windows.Forms.Label();
            this._keyTypeComboBox = new System.Windows.Forms.ComboBox();
            this._requireUserVerificationCheckBox = new System.Windows.Forms.CheckBox();
            this._userIdLabel = new System.Windows.Forms.Label();
            this._userIdTextBox = new System.Windows.Forms.TextBox();
            this._applicationIdLabel = new System.Windows.Forms.Label();
            this._applicationIdTextBox = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this._requireUserVerificationInfoIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._applicationIdInfoIcon)).BeginInit();
            this._encryptionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolTip
            // 
            this._toolTip.IsBalloon = true;
            // 
            // _requireUserVerificationInfoIcon
            // 
            this._requireUserVerificationInfoIcon.Image = global::SKSshAgent.Properties.Resources.information;
            this._requireUserVerificationInfoIcon.Location = new System.Drawing.Point(254, 41);
            this._requireUserVerificationInfoIcon.Name = "_requireUserVerificationInfoIcon";
            this._requireUserVerificationInfoIcon.Size = new System.Drawing.Size(19, 19);
            this._requireUserVerificationInfoIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._requireUserVerificationInfoIcon.TabIndex = 0;
            this._requireUserVerificationInfoIcon.TabStop = false;
            this._toolTip.SetToolTip(this._requireUserVerificationInfoIcon, resources.GetString("_requireUserVerificationInfoIcon.ToolTip"));
            this._requireUserVerificationInfoIcon.MouseEnter += new System.EventHandler(this.ShowToolTip);
            this._requireUserVerificationInfoIcon.MouseLeave += new System.EventHandler(this.HideToolTip);
            // 
            // _applicationIdInfoIcon
            // 
            this._applicationIdInfoIcon.Image = global::SKSshAgent.Properties.Resources.information;
            this._applicationIdInfoIcon.Location = new System.Drawing.Point(349, 95);
            this._applicationIdInfoIcon.Name = "_applicationIdInfoIcon";
            this._applicationIdInfoIcon.Size = new System.Drawing.Size(23, 23);
            this._applicationIdInfoIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this._applicationIdInfoIcon.TabIndex = 0;
            this._applicationIdInfoIcon.TabStop = false;
            this._toolTip.SetToolTip(this._applicationIdInfoIcon, "The application ID must begin with \"ssh:\".");
            this._applicationIdInfoIcon.MouseEnter += new System.EventHandler(this.ShowToolTip);
            this._applicationIdInfoIcon.MouseLeave += new System.EventHandler(this.HideToolTip);
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
            // _requireUserVerificationCheckBox
            // 
            this._requireUserVerificationCheckBox.AutoSize = true;
            this._requireUserVerificationCheckBox.Location = new System.Drawing.Point(103, 41);
            this._requireUserVerificationCheckBox.Name = "_requireUserVerificationCheckBox";
            this._requireUserVerificationCheckBox.Size = new System.Drawing.Size(154, 19);
            this._requireUserVerificationCheckBox.TabIndex = 2;
            this._requireUserVerificationCheckBox.Text = "Require User &Verification";
            this._requireUserVerificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // _userIdLabel
            // 
            this._userIdLabel.AutoSize = true;
            this._userIdLabel.Location = new System.Drawing.Point(50, 69);
            this._userIdLabel.Name = "_userIdLabel";
            this._userIdLabel.Size = new System.Drawing.Size(47, 15);
            this._userIdLabel.TabIndex = 3;
            this._userIdLabel.Text = "&User ID:";
            // 
            // _userIdTextBox
            // 
            this._userIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._userIdTextBox.Location = new System.Drawing.Point(103, 66);
            this._userIdTextBox.Name = "_userIdTextBox";
            this._userIdTextBox.PlaceholderText = "(optional)";
            this._userIdTextBox.Size = new System.Drawing.Size(269, 23);
            this._userIdTextBox.TabIndex = 4;
            // 
            // _applicationIdLabel
            // 
            this._applicationIdLabel.AutoSize = true;
            this._applicationIdLabel.Location = new System.Drawing.Point(12, 98);
            this._applicationIdLabel.Name = "_applicationIdLabel";
            this._applicationIdLabel.Size = new System.Drawing.Size(85, 15);
            this._applicationIdLabel.TabIndex = 5;
            this._applicationIdLabel.Text = "&Application ID:";
            // 
            // _applicationIdTextBox
            // 
            this._applicationIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._applicationIdTextBox.Location = new System.Drawing.Point(103, 95);
            this._applicationIdTextBox.Name = "_applicationIdTextBox";
            this._applicationIdTextBox.PlaceholderText = "ssh:(optional)";
            this._applicationIdTextBox.Size = new System.Drawing.Size(246, 23);
            this._applicationIdTextBox.TabIndex = 6;
            this._applicationIdTextBox.TextChanged += new System.EventHandler(this.HandleApplicationIdTextBoxTextChanged);
            this._applicationIdTextBox.Enter += new System.EventHandler(this.HandleApplicationIdTextBoxEnter);
            this._applicationIdTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HandleApplicationIdTextBoxKeyDown);
            this._applicationIdTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleApplicationIdTextBoxKeyPress);
            this._applicationIdTextBox.Leave += new System.EventHandler(this.HandleApplicationIdTextBoxLeave);
            // 
            // _commentLabel
            // 
            this._commentLabel.AutoSize = true;
            this._commentLabel.Location = new System.Drawing.Point(36, 127);
            this._commentLabel.Name = "_commentLabel";
            this._commentLabel.Size = new System.Drawing.Size(61, 15);
            this._commentLabel.TabIndex = 7;
            this._commentLabel.Text = "&Comment";
            // 
            // _commentTextBox
            // 
            this._commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commentTextBox.Location = new System.Drawing.Point(103, 124);
            this._commentTextBox.Name = "_commentTextBox";
            this._commentTextBox.Size = new System.Drawing.Size(269, 23);
            this._commentTextBox.TabIndex = 8;
            // 
            // _encryptCheckBox
            // 
            this._encryptCheckBox.AutoSize = true;
            this._encryptCheckBox.Checked = true;
            this._encryptCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this._encryptCheckBox.Location = new System.Drawing.Point(21, 152);
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
            this._encryptionGroupBox.Location = new System.Drawing.Point(12, 153);
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
            this._generateButton.Location = new System.Drawing.Point(216, 271);
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
            this._cancelButton.Location = new System.Drawing.Point(297, 271);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 12;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // SKKeyGenerationOptionsForm
            // 
            this.AcceptButton = this._generateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 306);
            this.Controls.Add(this._encryptCheckBox);
            this.Controls.Add(this._encryptionGroupBox);
            this.Controls.Add(this._applicationIdInfoIcon);
            this.Controls.Add(this._requireUserVerificationInfoIcon);
            this.Controls.Add(this._requireUserVerificationCheckBox);
            this.Controls.Add(this._applicationIdTextBox);
            this.Controls.Add(this._applicationIdLabel);
            this.Controls.Add(this._userIdTextBox);
            this.Controls.Add(this._userIdLabel);
            this.Controls.Add(this._commentLabel);
            this.Controls.Add(this._commentTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._generateButton);
            this.Controls.Add(this._keyTypeComboBox);
            this.Controls.Add(this._keyTypeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SKKeyGenerationOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Generation Options";
            ((System.ComponentModel.ISupportInitialize)(this._requireUserVerificationInfoIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._applicationIdInfoIcon)).EndInit();
            this._encryptionGroupBox.ResumeLayout(false);
            this._encryptionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip _toolTip;
        private System.Windows.Forms.Label _keyTypeLabel;
        private System.Windows.Forms.ComboBox _keyTypeComboBox;
        private System.Windows.Forms.CheckBox _requireUserVerificationCheckBox;
        private System.Windows.Forms.PictureBox _requireUserVerificationInfoIcon;
        private System.Windows.Forms.Label _userIdLabel;
        private System.Windows.Forms.TextBox _userIdTextBox;
        private System.Windows.Forms.Label _applicationIdLabel;
        private System.Windows.Forms.TextBox _applicationIdTextBox;
        private System.Windows.Forms.PictureBox _applicationIdInfoIcon;
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
