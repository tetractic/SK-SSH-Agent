// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class SKKeyGenerationOptionsForm
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
        components = new System.ComponentModel.Container();
        var resources = new System.ComponentModel.ComponentResourceManager(typeof(SKKeyGenerationOptionsForm));
        _toolTip = new ToolTip(components);
        _requireUserVerificationInfoIcon = new PictureBox();
        _applicationIdInfoIcon = new PictureBox();
        _keyTypeLabel = new Label();
        _keyTypeComboBox = new ComboBox();
        _requireUserVerificationCheckBox = new CheckBox();
        _userIdLabel = new Label();
        _userIdTextBox = new TextBox();
        _applicationIdLabel = new Label();
        _applicationIdTextBox = new TextBox();
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
        ((System.ComponentModel.ISupportInitialize)_requireUserVerificationInfoIcon).BeginInit();
        ((System.ComponentModel.ISupportInitialize)_applicationIdInfoIcon).BeginInit();
        _encryptionGroupBox.SuspendLayout();
        SuspendLayout();
        // 
        // _toolTip
        // 
        _toolTip.IsBalloon = true;
        // 
        // _requireUserVerificationInfoIcon
        // 
        _requireUserVerificationInfoIcon.Image = Properties.Resources.information;
        _requireUserVerificationInfoIcon.Location = new System.Drawing.Point(254, 41);
        _requireUserVerificationInfoIcon.Name = "_requireUserVerificationInfoIcon";
        _requireUserVerificationInfoIcon.Size = new System.Drawing.Size(19, 19);
        _requireUserVerificationInfoIcon.SizeMode = PictureBoxSizeMode.CenterImage;
        _requireUserVerificationInfoIcon.TabIndex = 0;
        _requireUserVerificationInfoIcon.TabStop = false;
        _toolTip.SetToolTip(_requireUserVerificationInfoIcon, resources.GetString("_requireUserVerificationInfoIcon.ToolTip"));
        _requireUserVerificationInfoIcon.MouseEnter += ShowToolTip;
        _requireUserVerificationInfoIcon.MouseLeave += HideToolTip;
        // 
        // _applicationIdInfoIcon
        // 
        _applicationIdInfoIcon.Image = Properties.Resources.information;
        _applicationIdInfoIcon.Location = new System.Drawing.Point(349, 95);
        _applicationIdInfoIcon.Name = "_applicationIdInfoIcon";
        _applicationIdInfoIcon.Size = new System.Drawing.Size(23, 23);
        _applicationIdInfoIcon.SizeMode = PictureBoxSizeMode.CenterImage;
        _applicationIdInfoIcon.TabIndex = 0;
        _applicationIdInfoIcon.TabStop = false;
        _toolTip.SetToolTip(_applicationIdInfoIcon, "The application ID must begin with \"ssh:\".");
        _applicationIdInfoIcon.MouseEnter += ShowToolTip;
        _applicationIdInfoIcon.MouseLeave += HideToolTip;
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
        // _requireUserVerificationCheckBox
        // 
        _requireUserVerificationCheckBox.AutoSize = true;
        _requireUserVerificationCheckBox.Location = new System.Drawing.Point(103, 41);
        _requireUserVerificationCheckBox.Name = "_requireUserVerificationCheckBox";
        _requireUserVerificationCheckBox.Size = new System.Drawing.Size(154, 19);
        _requireUserVerificationCheckBox.TabIndex = 2;
        _requireUserVerificationCheckBox.Text = "Require User &Verification";
        _requireUserVerificationCheckBox.UseVisualStyleBackColor = true;
        // 
        // _userIdLabel
        // 
        _userIdLabel.AutoSize = true;
        _userIdLabel.Location = new System.Drawing.Point(50, 69);
        _userIdLabel.Name = "_userIdLabel";
        _userIdLabel.Size = new System.Drawing.Size(47, 15);
        _userIdLabel.TabIndex = 3;
        _userIdLabel.Text = "&User ID:";
        // 
        // _userIdTextBox
        // 
        _userIdTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _userIdTextBox.Location = new System.Drawing.Point(103, 66);
        _userIdTextBox.Name = "_userIdTextBox";
        _userIdTextBox.PlaceholderText = "(optional)";
        _userIdTextBox.Size = new System.Drawing.Size(269, 23);
        _userIdTextBox.TabIndex = 4;
        // 
        // _applicationIdLabel
        // 
        _applicationIdLabel.AutoSize = true;
        _applicationIdLabel.Location = new System.Drawing.Point(12, 98);
        _applicationIdLabel.Name = "_applicationIdLabel";
        _applicationIdLabel.Size = new System.Drawing.Size(85, 15);
        _applicationIdLabel.TabIndex = 5;
        _applicationIdLabel.Text = "&Application ID:";
        // 
        // _applicationIdTextBox
        // 
        _applicationIdTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _applicationIdTextBox.Location = new System.Drawing.Point(103, 95);
        _applicationIdTextBox.Name = "_applicationIdTextBox";
        _applicationIdTextBox.PlaceholderText = "ssh:(optional)";
        _applicationIdTextBox.Size = new System.Drawing.Size(246, 23);
        _applicationIdTextBox.TabIndex = 6;
        _applicationIdTextBox.TextChanged += HandleApplicationIdTextBoxTextChanged;
        _applicationIdTextBox.Enter += HandleApplicationIdTextBoxEnter;
        _applicationIdTextBox.KeyDown += HandleApplicationIdTextBoxKeyDown;
        _applicationIdTextBox.KeyPress += HandleApplicationIdTextBoxKeyPress;
        _applicationIdTextBox.Leave += HandleApplicationIdTextBoxLeave;
        // 
        // _commentLabel
        // 
        _commentLabel.AutoSize = true;
        _commentLabel.Location = new System.Drawing.Point(36, 127);
        _commentLabel.Name = "_commentLabel";
        _commentLabel.Size = new System.Drawing.Size(61, 15);
        _commentLabel.TabIndex = 7;
        _commentLabel.Text = "&Comment";
        // 
        // _commentTextBox
        // 
        _commentTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _commentTextBox.Location = new System.Drawing.Point(103, 124);
        _commentTextBox.Name = "_commentTextBox";
        _commentTextBox.Size = new System.Drawing.Size(269, 23);
        _commentTextBox.TabIndex = 8;
        // 
        // _encryptCheckBox
        // 
        _encryptCheckBox.AutoSize = true;
        _encryptCheckBox.Checked = true;
        _encryptCheckBox.CheckState = CheckState.Checked;
        _encryptCheckBox.Location = new System.Drawing.Point(21, 152);
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
        _encryptionGroupBox.Location = new System.Drawing.Point(12, 153);
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
        _generateButton.Location = new System.Drawing.Point(216, 271);
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
        _cancelButton.Location = new System.Drawing.Point(297, 271);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new System.Drawing.Size(75, 23);
        _cancelButton.TabIndex = 12;
        _cancelButton.Text = "Cancel";
        _cancelButton.UseVisualStyleBackColor = true;
        // 
        // SKKeyGenerationOptionsForm
        // 
        AcceptButton = _generateButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = _cancelButton;
        ClientSize = new System.Drawing.Size(384, 306);
        Controls.Add(_encryptCheckBox);
        Controls.Add(_encryptionGroupBox);
        Controls.Add(_applicationIdInfoIcon);
        Controls.Add(_requireUserVerificationInfoIcon);
        Controls.Add(_requireUserVerificationCheckBox);
        Controls.Add(_applicationIdTextBox);
        Controls.Add(_applicationIdLabel);
        Controls.Add(_userIdTextBox);
        Controls.Add(_userIdLabel);
        Controls.Add(_commentLabel);
        Controls.Add(_commentTextBox);
        Controls.Add(_cancelButton);
        Controls.Add(_generateButton);
        Controls.Add(_keyTypeComboBox);
        Controls.Add(_keyTypeLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SKKeyGenerationOptionsForm";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Key Generation Options";
        ((System.ComponentModel.ISupportInitialize)_requireUserVerificationInfoIcon).EndInit();
        ((System.ComponentModel.ISupportInitialize)_applicationIdInfoIcon).EndInit();
        _encryptionGroupBox.ResumeLayout(false);
        _encryptionGroupBox.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
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
