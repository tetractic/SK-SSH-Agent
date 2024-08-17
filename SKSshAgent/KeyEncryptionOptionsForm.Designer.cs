// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class KeyEncryptionOptionsForm
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
        _kdfLabel = new Label();
        _kdfComboBox = new ComboBox();
        _kdfRoundsLabel = new Label();
        _kdfRoundsNumericUpDown = new NumericUpDown();
        _cipherLabel = new Label();
        _cipherComboBox = new ComboBox();
        _okayButton = new Button();
        _cancelButton = new Button();
        ((System.ComponentModel.ISupportInitialize)_kdfRoundsNumericUpDown).BeginInit();
        SuspendLayout();
        // 
        // _kdfLabel
        // 
        _kdfLabel.AutoSize = true;
        _kdfLabel.Location = new System.Drawing.Point(55, 15);
        _kdfLabel.Name = "_kdfLabel";
        _kdfLabel.Size = new System.Drawing.Size(31, 15);
        _kdfLabel.TabIndex = 0;
        _kdfLabel.Text = "&KDF:";
        _kdfLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // _kdfComboBox
        // 
        _kdfComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _kdfComboBox.DisplayMember = "Name";
        _kdfComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _kdfComboBox.FormattingEnabled = true;
        _kdfComboBox.Location = new System.Drawing.Point(92, 12);
        _kdfComboBox.Name = "_kdfComboBox";
        _kdfComboBox.Size = new System.Drawing.Size(180, 23);
        _kdfComboBox.TabIndex = 1;
        // 
        // _kdfRoundsLabel
        // 
        _kdfRoundsLabel.AutoSize = true;
        _kdfRoundsLabel.Location = new System.Drawing.Point(12, 43);
        _kdfRoundsLabel.Name = "_kdfRoundsLabel";
        _kdfRoundsLabel.Size = new System.Drawing.Size(74, 15);
        _kdfRoundsLabel.TabIndex = 2;
        _kdfRoundsLabel.Text = "KDF &Rounds:";
        _kdfRoundsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // _kdfRoundsNumericUpDown
        // 
        _kdfRoundsNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _kdfRoundsNumericUpDown.Location = new System.Drawing.Point(92, 41);
        _kdfRoundsNumericUpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
        _kdfRoundsNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        _kdfRoundsNumericUpDown.Name = "_kdfRoundsNumericUpDown";
        _kdfRoundsNumericUpDown.Size = new System.Drawing.Size(179, 23);
        _kdfRoundsNumericUpDown.TabIndex = 3;
        _kdfRoundsNumericUpDown.TextAlign = HorizontalAlignment.Right;
        _kdfRoundsNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // _cipherLabel
        // 
        _cipherLabel.AutoSize = true;
        _cipherLabel.Location = new System.Drawing.Point(41, 73);
        _cipherLabel.Name = "_cipherLabel";
        _cipherLabel.Size = new System.Drawing.Size(45, 15);
        _cipherLabel.TabIndex = 4;
        _cipherLabel.Text = "&Cipher:";
        _cipherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // _cipherComboBox
        // 
        _cipherComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        _cipherComboBox.DisplayMember = "Name";
        _cipherComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _cipherComboBox.FormattingEnabled = true;
        _cipherComboBox.Location = new System.Drawing.Point(91, 70);
        _cipherComboBox.Name = "_cipherComboBox";
        _cipherComboBox.Size = new System.Drawing.Size(180, 23);
        _cipherComboBox.TabIndex = 5;
        // 
        // _okayButton
        // 
        _okayButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _okayButton.Location = new System.Drawing.Point(116, 105);
        _okayButton.Name = "_okayButton";
        _okayButton.Size = new System.Drawing.Size(75, 23);
        _okayButton.TabIndex = 6;
        _okayButton.Text = "Okay";
        _okayButton.UseVisualStyleBackColor = true;
        _okayButton.Click += HandleOkayButtonClicked;
        // 
        // _cancelButton
        // 
        _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _cancelButton.Location = new System.Drawing.Point(197, 105);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new System.Drawing.Size(75, 23);
        _cancelButton.TabIndex = 7;
        _cancelButton.Text = "Cancel";
        _cancelButton.UseVisualStyleBackColor = true;
        // 
        // KeyEncryptionOptionsForm
        // 
        AcceptButton = _okayButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = _cancelButton;
        ClientSize = new System.Drawing.Size(284, 140);
        Controls.Add(_cancelButton);
        Controls.Add(_okayButton);
        Controls.Add(_cipherComboBox);
        Controls.Add(_kdfRoundsNumericUpDown);
        Controls.Add(_kdfComboBox);
        Controls.Add(_kdfLabel);
        Controls.Add(_cipherLabel);
        Controls.Add(_kdfRoundsLabel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "KeyEncryptionOptionsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Key Encryption Options";
        ((System.ComponentModel.ISupportInitialize)_kdfRoundsNumericUpDown).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label _kdfLabel;
    private System.Windows.Forms.ComboBox _kdfComboBox;
    private System.Windows.Forms.Label _kdfRoundsLabel;
    private System.Windows.Forms.NumericUpDown _kdfRoundsNumericUpDown;
    private System.Windows.Forms.Label _cipherLabel;
    private System.Windows.Forms.ComboBox _cipherComboBox;
    private System.Windows.Forms.Button _okayButton;
    private System.Windows.Forms.Button _cancelButton;
}
