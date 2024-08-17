// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class WebAuthnKeyUseConfirmationForm
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
        _fingerprintLabel = new Label();
        _fingerprintTextBox = new TextBox();
        SuspendLayout();
        // 
        // _fingerprintLabel
        // 
        _fingerprintLabel.AutoSize = true;
        _fingerprintLabel.Location = new System.Drawing.Point(13, 15);
        _fingerprintLabel.Name = "_fingerprintLabel";
        _fingerprintLabel.Size = new System.Drawing.Size(68, 15);
        _fingerprintLabel.TabIndex = 1;
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
        _fingerprintTextBox.TabIndex = 2;
        // 
        // WebAuthnKeyUseConfirmationForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(459, 46);
        Controls.Add(_fingerprintTextBox);
        Controls.Add(_fingerprintLabel);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = Properties.Resources.console_ssh_key_icon;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "WebAuthnKeyUseConfirmationForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Confirm Key Use";
        TopMost = true;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label _fingerprintLabel;
    private System.Windows.Forms.TextBox _fingerprintTextBox;
}
