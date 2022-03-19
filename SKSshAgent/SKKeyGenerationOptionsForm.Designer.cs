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
            this._typeLabel = new System.Windows.Forms.Label();
            this._typeComboBox = new System.Windows.Forms.ComboBox();
            this._requireUserVerificationCheckBox = new System.Windows.Forms.CheckBox();
            this._userIdLabel = new System.Windows.Forms.Label();
            this._userIdTextBox = new System.Windows.Forms.TextBox();
            this._applicationLabel = new System.Windows.Forms.Label();
            this._applicationTextBox = new System.Windows.Forms.TextBox();
            this._commentLabel = new System.Windows.Forms.Label();
            this._commentTextBox = new System.Windows.Forms.TextBox();
            this._generateButon = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _typeLabel
            // 
            this._typeLabel.AutoSize = true;
            this._typeLabel.Location = new System.Drawing.Point(49, 15);
            this._typeLabel.Name = "_typeLabel";
            this._typeLabel.Size = new System.Drawing.Size(34, 15);
            this._typeLabel.TabIndex = 0;
            this._typeLabel.Text = "&Type:";
            this._typeLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // _typeComboBox
            // 
            this._typeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._typeComboBox.DisplayMember = "Name";
            this._typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._typeComboBox.FormattingEnabled = true;
            this._typeComboBox.Location = new System.Drawing.Point(89, 12);
            this._typeComboBox.Name = "_typeComboBox";
            this._typeComboBox.Size = new System.Drawing.Size(283, 23);
            this._typeComboBox.TabIndex = 1;
            this._typeComboBox.SelectedIndexChanged += new System.EventHandler(this.HandleTypeComboBoxSelectedIndexChanged);
            // 
            // _requireUserVerificationCheckBox
            // 
            this._requireUserVerificationCheckBox.AutoSize = true;
            this._requireUserVerificationCheckBox.Location = new System.Drawing.Point(89, 41);
            this._requireUserVerificationCheckBox.Name = "_requireUserVerificationCheckBox";
            this._requireUserVerificationCheckBox.Size = new System.Drawing.Size(206, 19);
            this._requireUserVerificationCheckBox.TabIndex = 2;
            this._requireUserVerificationCheckBox.Text = "Require User &Verification (e.g. PIN)";
            this._requireUserVerificationCheckBox.UseVisualStyleBackColor = true;
            // 
            // _userIdLabel
            // 
            this._userIdLabel.AutoSize = true;
            this._userIdLabel.Location = new System.Drawing.Point(36, 69);
            this._userIdLabel.Name = "_userIdLabel";
            this._userIdLabel.Size = new System.Drawing.Size(47, 15);
            this._userIdLabel.TabIndex = 3;
            this._userIdLabel.Text = "&User ID:";
            // 
            // _userIdTextBox
            // 
            this._userIdTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._userIdTextBox.Location = new System.Drawing.Point(89, 66);
            this._userIdTextBox.Name = "_userIdTextBox";
            this._userIdTextBox.PlaceholderText = "(optional)";
            this._userIdTextBox.Size = new System.Drawing.Size(283, 23);
            this._userIdTextBox.TabIndex = 4;
            // 
            // _applicationLabel
            // 
            this._applicationLabel.AutoSize = true;
            this._applicationLabel.Location = new System.Drawing.Point(12, 98);
            this._applicationLabel.Name = "_applicationLabel";
            this._applicationLabel.Size = new System.Drawing.Size(71, 15);
            this._applicationLabel.TabIndex = 5;
            this._applicationLabel.Text = "&Application:";
            // 
            // _applicationTextBox
            // 
            this._applicationTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._applicationTextBox.Location = new System.Drawing.Point(89, 95);
            this._applicationTextBox.Name = "_applicationTextBox";
            this._applicationTextBox.PlaceholderText = "ssh:(optional)";
            this._applicationTextBox.Size = new System.Drawing.Size(283, 23);
            this._applicationTextBox.TabIndex = 6;
            this._applicationTextBox.TextChanged += new System.EventHandler(this.HandleApplicationTextBoxTextChanged);
            this._applicationTextBox.Enter += new System.EventHandler(this.HandleApplicationTextBoxEnter);
            this._applicationTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ApplicationTextBoxKeyDown);
            this._applicationTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleApplicationTextBoxKeyPress);
            this._applicationTextBox.Leave += new System.EventHandler(this.HandleApplicationTextBoxLeave);
            // 
            // _commentLabel
            // 
            this._commentLabel.AutoSize = true;
            this._commentLabel.Location = new System.Drawing.Point(22, 127);
            this._commentLabel.Name = "_commentLabel";
            this._commentLabel.Size = new System.Drawing.Size(61, 15);
            this._commentLabel.TabIndex = 7;
            this._commentLabel.Text = "&Comment";
            // 
            // _commentTextBox
            // 
            this._commentTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._commentTextBox.Location = new System.Drawing.Point(89, 124);
            this._commentTextBox.Name = "_commentTextBox";
            this._commentTextBox.Size = new System.Drawing.Size(283, 23);
            this._commentTextBox.TabIndex = 8;
            // 
            // _generateButon
            // 
            this._generateButon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._generateButon.Location = new System.Drawing.Point(216, 156);
            this._generateButon.Name = "_generateButon";
            this._generateButon.Size = new System.Drawing.Size(75, 23);
            this._generateButon.TabIndex = 9;
            this._generateButon.Text = "&Generate";
            this._generateButon.UseVisualStyleBackColor = true;
            this._generateButon.Click += new System.EventHandler(this.HandleGenerateButonClicked);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(297, 156);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 10;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // GenerateOptionsForm
            // 
            this.AcceptButton = this._generateButon;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(384, 191);
            this.Controls.Add(this._requireUserVerificationCheckBox);
            this.Controls.Add(this._applicationTextBox);
            this.Controls.Add(this._applicationLabel);
            this.Controls.Add(this._userIdTextBox);
            this.Controls.Add(this._userIdLabel);
            this.Controls.Add(this._commentLabel);
            this.Controls.Add(this._commentTextBox);
            this.Controls.Add(this._cancelButton);
            this.Controls.Add(this._generateButon);
            this.Controls.Add(this._typeComboBox);
            this.Controls.Add(this._typeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateOptionsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Key Generation Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _typeLabel;
        private System.Windows.Forms.ComboBox _typeComboBox;
        private System.Windows.Forms.CheckBox _requireUserVerificationCheckBox;
        private System.Windows.Forms.Label _userIdLabel;
        private System.Windows.Forms.TextBox _userIdTextBox;
        private System.Windows.Forms.Label _applicationLabel;
        private System.Windows.Forms.TextBox _applicationTextBox;
        private System.Windows.Forms.Label _commentLabel;
        private System.Windows.Forms.TextBox _commentTextBox;
        private System.Windows.Forms.Button _generateButon;
        private System.Windows.Forms.Button _cancelButton;
    }
}
