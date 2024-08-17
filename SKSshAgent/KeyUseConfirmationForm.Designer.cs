// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class KeyUseConfirmationForm
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
        _fingerprintLabel = new Label();
        _fingerprintTextBox = new TextBox();
        _confirmButton = new Button();
        _cancelButton = new Button();
        _delayTimer = new Timer(components);
        SuspendLayout();
        // 
        // _fingerprintLabel
        // 
        _fingerprintLabel.AutoSize = true;
        _fingerprintLabel.Location = new System.Drawing.Point(13, 15);
        _fingerprintLabel.Name = "_fingerprintLabel";
        _fingerprintLabel.Size = new System.Drawing.Size(68, 15);
        _fingerprintLabel.TabIndex = 3;
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
        _fingerprintTextBox.TabIndex = 4;
        // 
        // _confirmButton
        // 
        _confirmButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _confirmButton.Location = new System.Drawing.Point(291, 46);
        _confirmButton.Name = "_confirmButton";
        _confirmButton.Size = new System.Drawing.Size(75, 23);
        _confirmButton.TabIndex = 1;
        _confirmButton.Text = "Delay";
        _confirmButton.UseVisualStyleBackColor = true;
        _confirmButton.UseWaitCursor = true;
        _confirmButton.Click += HandleAllowButtonClicked;
        _confirmButton.KeyPress += HandleConfirmButtonKeyPressed;
        // 
        // _cancelButton
        // 
        _cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        _cancelButton.Location = new System.Drawing.Point(372, 46);
        _cancelButton.Name = "_cancelButton";
        _cancelButton.Size = new System.Drawing.Size(75, 23);
        _cancelButton.TabIndex = 2;
        _cancelButton.Text = "Cancel";
        _cancelButton.UseVisualStyleBackColor = true;
        // 
        // _delayTimer
        // 
        _delayTimer.Enabled = true;
        _delayTimer.Tick += HandleDelayTimerTicked;
        // 
        // KeyUseConfirmationForm
        // 
        AcceptButton = _confirmButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = _cancelButton;
        ClientSize = new System.Drawing.Size(459, 81);
        Controls.Add(_fingerprintTextBox);
        Controls.Add(_fingerprintLabel);
        Controls.Add(_confirmButton);
        Controls.Add(_cancelButton);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = Properties.Resources.console_ssh_key_icon;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "KeyUseConfirmationForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Confirm Key Use";
        TopMost = true;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label _fingerprintLabel;
    private System.Windows.Forms.TextBox _fingerprintTextBox;
    private System.Windows.Forms.Button _confirmButton;
    private System.Windows.Forms.Button _cancelButton;
    private System.Windows.Forms.Timer _delayTimer;
}
