// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class KeyGenerationOptionsForm
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
    /// Required method for Designer support - do not modify the contents of this method with the
    /// code editor.
    /// </summary>
    private void InitializeComponent()
    {
        _keyTypeLabel = new Label();
        _keyTypeComboBox = new ComboBox();
        _commentLabel = new Label();
        _commentTextBox = new TextBox();
        _encryptCheckBox = new CheckBox();
        _encryptionGroupBox = new GroupBox();
        _passwordLabel = new Label();
        _passwordTextBox = new PasswordTextBox();
        _confirmPasswordLabel = new Label();
        _confirmPasswordTextBox = new PasswordTextBox();
        _encryptionOptionsButton = new Button();
        _generateButton = new Button();
        _cancelButton = new Button();
        _encryptionGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // _keyTypeLabel
        // 
        _keyTypeLabel.AutoSize = true;
        _keyTypeLabel.Location = new System.Drawing.Point(41, 15);
        _keyTypeLabel.Name = "_keyTypeLabel";
        _keyTypeLabel.Size = new System.Drawing.Size(56, 15);
        _keyTypeLabel.TabIndex = 0;
        _keyTypeLabel.Text = "Key &Type:";
        _keyTypeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
        // 
        // _keyTypeComboBox
        // 
        _keyTypeComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _keyTypeComboBox.DisplayMember = "Name";
        _keyTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _keyTypeComboBox.FormattingEnabled = true;
        _keyTypeComboBox.Location = new System.Drawing.Point(103, 12);
        _keyTypeComboBox.Name = "_keyTypeComboBox";
        _keyTypeComboBox.Size = new System.Drawing.Size(269, 23);
        _keyTypeComboBox.TabIndex = 1;
        // 
        // _commentLabel
        // 
        _commentLabel.AutoSize = true;
        _commentLabel.Location = new System.Drawing.Point(36, 44);
        _commentLabel.Name = "_commentLabel";
        _commentLabel.Size = new System.Drawing.Size(61, 15);
        _commentLabel.TabIndex = 7;
        _commentLabel.Text = "&Comment";
        // 
        // _commentTextBox
        // 
        _commentTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _commentTextBox.Location = new System.Drawing.Point(103, 41);
        _commentTextBox.Name = "_commentTextBox";
        _commentTextBox.Size = new System.Drawing.Size(269, 23);
        _commentTextBox.TabIndex = 8;
        // 
        // _encryptCheckBox
        // 
        _encryptCheckBox.AutoSize = true;
        _encryptCheckBox.Checked = true;
        _encryptCheckBox.CheckState = CheckState.Checked;
        _encryptCheckBox.Location = new System.Drawing.Point(21, 69);
        _encryptCheckBox.Name = "_encryptCheckBox";
        _encryptCheckBox.Size = new System.Drawing.Size(66, 19);
        _encryptCheckBox.TabIndex = 9;
        _encryptCheckBox.Text = "&Encrypt";
        _encryptCheckBox.CheckedChanged += HandleEncryptCheckBoxCheckedChanged;
        // 
        // _encryptionGroupBox
        // 
        _encryptionGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _encryptionGroupBox.Controls.Add(_passwordLabel);
        _encryptionGroupBox.Controls.Add(_passwordTextBox);
        _encryptionGroupBox.Controls.Add(_confirmPasswordLabel);
        _encryptionGroupBox.Controls.Add(_confirmPasswordTextBox);
        _encryptionGroupBox.Controls.Add(_encryptionOptionsButton);
        _encryptionGroupBox.Location = new System.Drawing.Point(12, 70);
        _encryptionGroupBox.Name = "_encryptionGroupBox";
        _encryptionGroupBox.Size = new System.Drawing.Size(360, 110);
        _encryptionGroupBox.TabIndex = 10;
        _encryptionGroupBox.TabStop = false;
        // 
        // _passwordLabel
        // 
        _passwordLabel.AutoSize = true;
        _passwordLabel.Location = new System.Drawing.Point(9, 25);
        _passwordLabel.Name = "_passwordLabel";
        _passwordLabel.Size = new System.Drawing.Size(60, 15);
        _passwordLabel.TabIndex = 0;
        _passwordLabel.Text = "&Password:";
        _passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // _passwordTextBox
        // 
        _passwordTextBox.Location = new System.Drawing.Point(75, 22);
        _passwordTextBox.Name = "_passwordTextBox";
        _passwordTextBox.Size = new System.Drawing.Size(279, 23);
        _passwordTextBox.TabIndex = 1;
        _passwordTextBox.UseSystemPasswordChar = true;
        // 
        // _confirmPasswordLabel
        // 
        _confirmPasswordLabel.AutoSize = true;
        _confirmPasswordLabel.Location = new System.Drawing.Point(15, 54);
        _confirmPasswordLabel.Name = "_confirmPasswordLabel";
        _confirmPasswordLabel.Size = new System.Drawing.Size(54, 15);
        _confirmPasswordLabel.TabIndex = 2;
        _confirmPasswordLabel.Text = "Con&firm:";
        _confirmPasswordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // _confirmPasswordTextBox
        // 
        _confirmPasswordTextBox.Location = new System.Drawing.Point(75, 51);
        _confirmPasswordTextBox.Name = "_confirmPasswordTextBox";
        _confirmPasswordTextBox.Size = new System.Drawing.Size(279, 23);
        _confirmPasswordTextBox.TabIndex = 3;
        _confirmPasswordTextBox.UseSystemPasswordChar = true;
        // 
        // _encryptionOptionsButton
        // 
        _encryptionOptionsButton.Location = new System.Drawing.Point(279, 80);
        _encryptionOptionsButton.Name = "_encryptionOptionsButton";
        _encryptionOptionsButton.Size = new System.Drawing.Size(75, 23);
        _encryptionOptionsButton.TabIndex = 4;
        _encryptionOptionsButton.Text = "&Options...";
        _encryptionOptionsButton.UseVisualStyleBackColor = true;
        _encryptionOptionsButton.Click += HandleEncryptionOptionsButtonClicked;
        // 
        // _generateButton
        // 
        _generateButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _generateButton.Location = new System.Drawing.Point(216, 188);
        _generateButton.Name = "_generateButton";
        _generateButton.Size = new System.Drawing.Size(75, 23);
        _generateButton.TabIndex = 11;
        _generateButton.Text = "&Generate";
        _generateButton.UseVisualStyleBackColor = true;
        _generateButton.Click += HandleGenerateButtonClicked;
        // 
        // _cancelButton
        // 
        _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _cancelButton.Location = new System.Drawing.Point(297, 188);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new System.Drawing.Size(75, 23);
        _cancelButton.TabIndex = 12;
        _cancelButton.Text = "Cancel";
        _cancelButton.UseVisualStyleBackColor = true;
        // 
        // KeyGenerationOptionsForm
        // 
        AcceptButton = _generateButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = _cancelButton;
        ClientSize = new System.Drawing.Size(384, 223);
        Controls.Add(_encryptCheckBox);
        Controls.Add(_encryptionGroupBox);
        Controls.Add(_commentLabel);
        Controls.Add(_commentTextBox);
        Controls.Add(_cancelButton);
        Controls.Add(_generateButton);
        Controls.Add(_keyTypeComboBox);
        Controls.Add(_keyTypeLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "KeyGenerationOptionsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Key Generation Options";
        _encryptionGroupBox.ResumeLayout(false);
        _encryptionGroupBox.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
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
