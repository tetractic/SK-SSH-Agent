// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Windows.Forms;

namespace SKSshAgent
{
    public partial class WebAuthnKeyUseConfirmationForm : Form
    {
        public WebAuthnKeyUseConfirmationForm()
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
    }
}
