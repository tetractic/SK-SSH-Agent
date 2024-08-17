// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Windows.Forms;

namespace SKSshAgent;

internal partial class KeyDecryptionForm : Form
{
    public KeyDecryptionForm()
    {
        InitializeComponent();
    }

    /// <exception cref="ArgumentNullException" accessor="set"/>
    public string Fingerprint
    {
        get => _fingerprintTextBox.Text;
        set
        {
            if (_fingerprintTextBox == null)
                throw new ArgumentNullException(nameof(value));

            _fingerprintTextBox.Text = value;
        }
    }

    public ShieldedImmutableBuffer Result { get; private set; }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);

        Activate();
    }

    private void HandleDecryptButtonClicked(object sender, EventArgs e)
    {
        if (_passwordTextBox.TextLength == 0)
        {
            _ = _passwordTextBox.Focus();

            _ = MessageBox.Show(this, "Password cannot be empty.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Result = _passwordTextBox.GetPassword();

        _passwordTextBox.ZeroMemory();

        DialogResult = DialogResult.OK;
    }
}
