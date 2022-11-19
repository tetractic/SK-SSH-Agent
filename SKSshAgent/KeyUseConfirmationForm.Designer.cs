// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

namespace SKSshAgent
{
    partial class KeyUseConfirmationForm
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
            this._fingerprintLabel = new System.Windows.Forms.Label();
            this._fingerprintTextBox = new System.Windows.Forms.TextBox();
            this._confirmButton = new System.Windows.Forms.Button();
            this._cancelButton = new System.Windows.Forms.Button();
            this._delayTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // _fingerprintLabel
            // 
            this._fingerprintLabel.AutoSize = true;
            this._fingerprintLabel.Location = new System.Drawing.Point(13, 15);
            this._fingerprintLabel.Name = "_fingerprintLabel";
            this._fingerprintLabel.Size = new System.Drawing.Size(68, 15);
            this._fingerprintLabel.TabIndex = 3;
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
            this._fingerprintTextBox.TabIndex = 4;
            // 
            // _confirmButton
            // 
            this._confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._confirmButton.Location = new System.Drawing.Point(291, 46);
            this._confirmButton.Name = "_confirmButton";
            this._confirmButton.Size = new System.Drawing.Size(75, 23);
            this._confirmButton.TabIndex = 1;
            this._confirmButton.Text = "Delay";
            this._confirmButton.UseVisualStyleBackColor = true;
            this._confirmButton.UseWaitCursor = true;
            this._confirmButton.Click += new System.EventHandler(this.HandleAllowButtonClicked);
            this._confirmButton.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleConfirmButtonKeyPressed);
            // 
            // _cancelButton
            // 
            this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancelButton.Location = new System.Drawing.Point(372, 46);
            this._cancelButton.Name = "_cancelButton";
            this._cancelButton.Size = new System.Drawing.Size(75, 23);
            this._cancelButton.TabIndex = 2;
            this._cancelButton.Text = "Cancel";
            this._cancelButton.UseVisualStyleBackColor = true;
            // 
            // _delayTimer
            // 
            this._delayTimer.Enabled = true;
            this._delayTimer.Tick += new System.EventHandler(this.HandleDelayTimerTicked);
            // 
            // KeyUseConfirmationForm
            // 
            this.AcceptButton = this._confirmButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._cancelButton;
            this.ClientSize = new System.Drawing.Size(459, 81);
            this.Controls.Add(this._fingerprintTextBox);
            this.Controls.Add(this._fingerprintLabel);
            this.Controls.Add(this._confirmButton);
            this.Controls.Add(this._cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::SKSshAgent.Properties.Resources.console_ssh_key_icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyUseConfirmationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Confirm Key Use";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label _fingerprintLabel;
        private System.Windows.Forms.TextBox _fingerprintTextBox;
        private System.Windows.Forms.Button _confirmButton;
        private System.Windows.Forms.Button _cancelButton;
        private System.Windows.Forms.Timer _delayTimer;
    }
}
