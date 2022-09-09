// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent
{
    partial class KeyEncryptionOptionsForm
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
            this._kdfLabel = new System.Windows.Forms.Label();
            this._kdfComboBox = new System.Windows.Forms.ComboBox();
            this._kdfRoundsLabel = new System.Windows.Forms.Label();
            this._kdfRoundsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this._cipherLabel = new System.Windows.Forms.Label();
            this._cipherComboBox = new System.Windows.Forms.ComboBox();
            this._okayButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._kdfRoundsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // _kdfLabel
            // 
            this._kdfLabel.AutoSize = true;
            this._kdfLabel.Location = new System.Drawing.Point(55, 15);
            this._kdfLabel.Name = "_kdfLabel";
            this._kdfLabel.Size = new System.Drawing.Size(31, 15);
            this._kdfLabel.TabIndex = 0;
            this._kdfLabel.Text = "&KDF:";
            this._kdfLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _kdfComboBox
            // 
            this._kdfComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._kdfComboBox.DisplayMember = "Name";
            this._kdfComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._kdfComboBox.FormattingEnabled = true;
            this._kdfComboBox.Location = new System.Drawing.Point(92, 12);
            this._kdfComboBox.Name = "_kdfComboBox";
            this._kdfComboBox.Size = new System.Drawing.Size(180, 23);
            this._kdfComboBox.TabIndex = 1;
            // 
            // _kdfRoundsLabel
            // 
            this._kdfRoundsLabel.AutoSize = true;
            this._kdfRoundsLabel.Location = new System.Drawing.Point(12, 43);
            this._kdfRoundsLabel.Name = "_kdfRoundsLabel";
            this._kdfRoundsLabel.Size = new System.Drawing.Size(74, 15);
            this._kdfRoundsLabel.TabIndex = 2;
            this._kdfRoundsLabel.Text = "KDF &Rounds:";
            this._kdfRoundsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _kdfRoundsNumericUpDown
            // 
            this._kdfRoundsNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._kdfRoundsNumericUpDown.Location = new System.Drawing.Point(92, 41);
            this._kdfRoundsNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this._kdfRoundsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._kdfRoundsNumericUpDown.Name = "_kdfRoundsNumericUpDown";
            this._kdfRoundsNumericUpDown.Size = new System.Drawing.Size(179, 23);
            this._kdfRoundsNumericUpDown.TabIndex = 3;
            this._kdfRoundsNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this._kdfRoundsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // _cipherLabel
            // 
            this._cipherLabel.AutoSize = true;
            this._cipherLabel.Location = new System.Drawing.Point(41, 73);
            this._cipherLabel.Name = "_cipherLabel";
            this._cipherLabel.Size = new System.Drawing.Size(45, 15);
            this._cipherLabel.TabIndex = 4;
            this._cipherLabel.Text = "&Cipher:";
            this._cipherLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _cipherComboBox
            // 
            this._cipherComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cipherComboBox.DisplayMember = "Name";
            this._cipherComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cipherComboBox.FormattingEnabled = true;
            this._cipherComboBox.Location = new System.Drawing.Point(91, 70);
            this._cipherComboBox.Name = "_cipherComboBox";
            this._cipherComboBox.Size = new System.Drawing.Size(180, 23);
            this._cipherComboBox.TabIndex = 5;
            // 
            // _okayButton
            // 
            this._okayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okayButton.Location = new System.Drawing.Point(116, 105);
            this._okayButton.Name = "_okayButton";
            this._okayButton.Size = new System.Drawing.Size(75, 23);
            this._okayButton.TabIndex = 6;
            this._okayButton.Text = "Okay";
            this._okayButton.UseVisualStyleBackColor = true;
            this._okayButton.Click += new System.EventHandler(this.HandleOkayButtonClicked);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(197, 105);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 7;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // KeyEncryptionOptionsForm
            // 
            this.AcceptButton = this._okayButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(284, 140);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._okayButton);
            this.Controls.Add(this._cipherComboBox);
            this.Controls.Add(this._kdfRoundsNumericUpDown);
            this.Controls.Add(this._kdfComboBox);
            this.Controls.Add(this._kdfLabel);
            this.Controls.Add(this._cipherLabel);
            this.Controls.Add(this._kdfRoundsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyEncryptionOptionsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Encryption Options";
            ((System.ComponentModel.ISupportInitialize)(this._kdfRoundsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
}
