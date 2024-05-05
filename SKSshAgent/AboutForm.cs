// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SKSshAgent
{
    internal partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            _versionLabel.Text = $"Version {AssemblyVersion}";
            _copyrightLabel.Text = AssemblyCopyright;

            _licenseLabel.Links.Clear();

            const string gplLinkText = "GNU General Public License Version 3";
            int gplLinkStart = _licenseLabel.Text.IndexOf(gplLinkText, StringComparison.Ordinal);
            _licenseLabel.Links.Add(gplLinkStart, gplLinkText.Length, "https://www.gnu.org/licenses/gpl-3.0-standalone.html");

            _silkLicenseLinkLabel.Links.Clear();

            const string silkLinkText = "Silk";
            int silkLinkStart = _silkLicenseLinkLabel.Text.IndexOf(silkLinkText, StringComparison.Ordinal);
            _silkLicenseLinkLabel.Links.Add(silkLinkStart, silkLinkText.Length, "http://www.famfamfam.com/lab/icons/silk/");

            const string ccLicenseLinkText = "Creative Commons Attribution 2.5";
            int ccLicenseLinkStart = _silkLicenseLinkLabel.Text.IndexOf(ccLicenseLinkText, StringComparison.Ordinal);
            _silkLicenseLinkLabel.Links.Add(ccLicenseLinkStart, ccLicenseLinkText.Length, "https://creativecommons.org/licenses/by/2.5/");
        }

        public static string AssemblyVersion
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                return attribute!.InformationalVersion;
            }
        }

        public static string AssemblyCopyright
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var attribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
                return attribute!.Copyright;
            }
        }

        private void HandleLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = (string)e.Link!.LinkData!;

            try
            {
                _ = Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true,
                });
            }
            catch (Win32Exception)
            {
                _ = MessageBox.Show(this, "The web browser failed to start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
