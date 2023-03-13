// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent
{
    partial class AboutForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support - do not modify the contents of this method with
        /// the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _tableLayoutPanel = new TableLayoutPanel();
            _productNameLabel = new Label();
            _versionLablel = new Label();
            _copyrightLabel = new Label();
            _licenseLabel = new LinkLabel();
            _silkLicenseLinkLabel = new LinkLabel();
            _okayButton = new Button();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            _tableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _tableLayoutPanel.ColumnCount = 1;
            _tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            _tableLayoutPanel.Controls.Add(_productNameLabel, 0, 0);
            _tableLayoutPanel.Controls.Add(_versionLablel, 0, 1);
            _tableLayoutPanel.Controls.Add(_copyrightLabel, 0, 2);
            _tableLayoutPanel.Controls.Add(_licenseLabel, 0, 3);
            _tableLayoutPanel.Controls.Add(_silkLicenseLinkLabel, 0, 4);
            _tableLayoutPanel.Location = new System.Drawing.Point(13, 13);
            _tableLayoutPanel.Name = "_tableLayoutPanel";
            _tableLayoutPanel.RowCount = 5;
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.RowStyles.Add(new RowStyle());
            _tableLayoutPanel.Size = new System.Drawing.Size(408, 142);
            _tableLayoutPanel.TabIndex = 1;
            // 
            // _productNameLabel
            // 
            _productNameLabel.Dock = DockStyle.Fill;
            _productNameLabel.Location = new System.Drawing.Point(3, 0);
            _productNameLabel.Name = "_productNameLabel";
            _productNameLabel.Size = new System.Drawing.Size(402, 20);
            _productNameLabel.TabIndex = 1;
            _productNameLabel.Text = "SK SSH Agent";
            _productNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _versionLablel
            // 
            _versionLablel.Dock = DockStyle.Fill;
            _versionLablel.Location = new System.Drawing.Point(3, 20);
            _versionLablel.Name = "_versionLablel";
            _versionLablel.Size = new System.Drawing.Size(402, 20);
            _versionLablel.TabIndex = 1;
            _versionLablel.Text = "Version";
            _versionLablel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _copyrightLabel
            // 
            _copyrightLabel.Dock = DockStyle.Fill;
            _copyrightLabel.Location = new System.Drawing.Point(3, 40);
            _copyrightLabel.Name = "_copyrightLabel";
            _copyrightLabel.Size = new System.Drawing.Size(402, 20);
            _copyrightLabel.TabIndex = 2;
            _copyrightLabel.Text = "Copyright";
            _copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _licenseLabel
            // 
            _licenseLabel.AutoSize = true;
            _licenseLabel.Dock = DockStyle.Fill;
            _licenseLabel.Location = new System.Drawing.Point(3, 66);
            _licenseLabel.Margin = new Padding(3, 6, 3, 0);
            _licenseLabel.MaximumSize = new System.Drawing.Size(400, 0);
            _licenseLabel.Name = "_licenseLabel";
            _licenseLabel.Size = new System.Drawing.Size(400, 30);
            _licenseLabel.TabIndex = 3;
            _licenseLabel.TabStop = true;
            _licenseLabel.Text = "This program is licensed under the terms of the GNU General Public License Version 3 as published by the Free Software Foundation.";
            _licenseLabel.LinkClicked += HandleLinkClicked;
            // 
            // _silkLicenseLinkLabel
            // 
            _silkLicenseLinkLabel.AutoSize = true;
            _silkLicenseLinkLabel.Location = new System.Drawing.Point(3, 102);
            _silkLicenseLinkLabel.Margin = new Padding(3, 6, 3, 0);
            _silkLicenseLinkLabel.MaximumSize = new System.Drawing.Size(400, 0);
            _silkLicenseLinkLabel.Name = "_silkLicenseLinkLabel";
            _silkLicenseLinkLabel.Size = new System.Drawing.Size(384, 30);
            _silkLicenseLinkLabel.TabIndex = 4;
            _silkLicenseLinkLabel.TabStop = true;
            _silkLicenseLinkLabel.Text = "The \"Silk\" icon set was created by Mark James and is licensed under the terms of the Creative Commons Attribution 2.5 license.";
            _silkLicenseLinkLabel.LinkClicked += HandleLinkClicked;
            // 
            // _okayButton
            // 
            _okayButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            _okayButton.DialogResult = DialogResult.Cancel;
            _okayButton.Location = new System.Drawing.Point(333, 161);
            _okayButton.Name = "_okayButton";
            _okayButton.Size = new System.Drawing.Size(88, 27);
            _okayButton.TabIndex = 0;
            _okayButton.Text = "Okay";
            // 
            // AboutForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = _okayButton;
            ClientSize = new System.Drawing.Size(434, 201);
            Controls.Add(_okayButton);
            Controls.Add(_tableLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AboutForm";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About SK SSH Agent";
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _productNameLabel;
        private System.Windows.Forms.Label _versionLablel;
        private System.Windows.Forms.Label _copyrightLabel;
        private System.Windows.Forms.LinkLabel _licenseLabel;
        private System.Windows.Forms.LinkLabel _silkLicenseLinkLabel;
        private System.Windows.Forms.Button _okayButton;
    }
}
