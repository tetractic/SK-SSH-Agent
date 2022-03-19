// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this._productNameLabel = new System.Windows.Forms.Label();
            this._versionLablel = new System.Windows.Forms.Label();
            this._copyrightLabel = new System.Windows.Forms.Label();
            this._licenseLabel = new System.Windows.Forms.LinkLabel();
            this._silkLicenseLinkLabel = new System.Windows.Forms.LinkLabel();
            this._okayButton = new System.Windows.Forms.Button();
            this._tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _tableLayoutPanel
            // 
            this._tableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tableLayoutPanel.ColumnCount = 1;
            this._tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tableLayoutPanel.Controls.Add(this._productNameLabel, 0, 0);
            this._tableLayoutPanel.Controls.Add(this._versionLablel, 0, 1);
            this._tableLayoutPanel.Controls.Add(this._copyrightLabel, 0, 2);
            this._tableLayoutPanel.Controls.Add(this._licenseLabel, 0, 3);
            this._tableLayoutPanel.Controls.Add(this._silkLicenseLinkLabel, 0, 4);
            this._tableLayoutPanel.Location = new System.Drawing.Point(13, 13);
            this._tableLayoutPanel.Name = "_tableLayoutPanel";
            this._tableLayoutPanel.RowCount = 5;
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._tableLayoutPanel.Size = new System.Drawing.Size(408, 142);
            this._tableLayoutPanel.TabIndex = 1;
            // 
            // _productNameLabel
            // 
            this._productNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._productNameLabel.Location = new System.Drawing.Point(3, 0);
            this._productNameLabel.Name = "_productNameLabel";
            this._productNameLabel.Size = new System.Drawing.Size(402, 20);
            this._productNameLabel.TabIndex = 1;
            this._productNameLabel.Text = "SK SSH Agent";
            this._productNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _versionLablel
            // 
            this._versionLablel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._versionLablel.Location = new System.Drawing.Point(3, 20);
            this._versionLablel.Name = "_versionLablel";
            this._versionLablel.Size = new System.Drawing.Size(402, 20);
            this._versionLablel.TabIndex = 1;
            this._versionLablel.Text = "Version";
            this._versionLablel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _copyrightLabel
            // 
            this._copyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._copyrightLabel.Location = new System.Drawing.Point(3, 40);
            this._copyrightLabel.Name = "_copyrightLabel";
            this._copyrightLabel.Size = new System.Drawing.Size(402, 20);
            this._copyrightLabel.TabIndex = 2;
            this._copyrightLabel.Text = "Copyright";
            this._copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _licenseLabel
            // 
            this._licenseLabel.AutoSize = true;
            this._licenseLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._licenseLabel.Location = new System.Drawing.Point(3, 66);
            this._licenseLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this._licenseLabel.MaximumSize = new System.Drawing.Size(400, 0);
            this._licenseLabel.Name = "_licenseLabel";
            this._licenseLabel.Size = new System.Drawing.Size(400, 30);
            this._licenseLabel.TabIndex = 3;
            this._licenseLabel.TabStop = true;
            this._licenseLabel.Text = "This program is licensed under the terms of the GNU General Public License versio" +
    "n 3 as published by the Free Software Foundation.";
            this._licenseLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleLinkClicked);
            // 
            // _silkLicenseLinkLabel
            // 
            this._silkLicenseLinkLabel.AutoSize = true;
            this._silkLicenseLinkLabel.Location = new System.Drawing.Point(3, 102);
            this._silkLicenseLinkLabel.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
            this._silkLicenseLinkLabel.MaximumSize = new System.Drawing.Size(400, 0);
            this._silkLicenseLinkLabel.Name = "_silkLicenseLinkLabel";
            this._silkLicenseLinkLabel.Size = new System.Drawing.Size(384, 30);
            this._silkLicenseLinkLabel.TabIndex = 4;
            this._silkLicenseLinkLabel.TabStop = true;
            this._silkLicenseLinkLabel.Text = "The \"Silk\" icon set was created by Mark James and is licensed under the terms of " +
    "the Creative Commons Attribution 2.5 license.";
            this._silkLicenseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.HandleLinkClicked);
            // 
            // _okayButton
            // 
            this._okayButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._okayButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._okayButton.Location = new System.Drawing.Point(333, 161);
            this._okayButton.Name = "_okayButton";
            this._okayButton.Size = new System.Drawing.Size(88, 27);
            this._okayButton.TabIndex = 0;
            this._okayButton.Text = "Okay";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._okayButton;
            this.ClientSize = new System.Drawing.Size(434, 201);
            this.Controls.Add(this._okayButton);
            this.Controls.Add(this._tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About SK SSH Agent";
            this._tableLayoutPanel.ResumeLayout(false);
            this._tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

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
