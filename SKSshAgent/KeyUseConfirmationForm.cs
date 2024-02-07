// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.Windows.Forms;

namespace SKSshAgent
{
    internal partial class KeyUseConfirmationForm : Form
    {
        private const int _ticksInitial = 8;

        private int _ticksRemaining = _ticksInitial;

        public KeyUseConfirmationForm()
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

        private void HandleConfirmButtonKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (_ticksRemaining > 0)
            {
                e.Handled = true;

                ResetDelay();
                return;
            }
        }

        private void HandleAllowButtonClicked(object sender, EventArgs e)
        {
            if (_ticksRemaining > 0)
            {
                ResetDelay();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void HandleDelayTimerTicked(object sender, EventArgs e)
        {
            if (_ticksRemaining > 0)
            {
                _confirmButton.Text = $"{_ticksRemaining / 10f:F1} s";

                _ticksRemaining -= 1;
            }
            else
            {
                _delayTimer.Enabled = false;

                _confirmButton.Text = "Confirm";
                _confirmButton.UseWaitCursor = false;
            }
        }

        private void ResetDelay()
        {
            _confirmButton.Text = "Delay";

            _ticksRemaining = _ticksInitial;

            _delayTimer.Stop();
            _delayTimer.Start();
        }
    }
}
