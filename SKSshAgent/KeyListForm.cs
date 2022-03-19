// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Properties;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Win32.Foundation;
using static Windows.Win32.PInvoke;

namespace SKSshAgent
{
    internal partial class KeyListForm : Form
    {
        private OpenSshPipe? _openSshPipe;
        private Task? _openSshPipeStartTask;

        private PageantPipe? _pageantPipe;
        private Task? _pageantPipeStartTask;

        private int? _keyListViewColumnHeaderHeight;

        public KeyListForm()
        {
            InitializeComponent();

            HandleCreated += HandleHandleCreated;
        }

        public bool AllowVislble { get; set; }

        public bool AllowClose { get; set; }

        protected override void SetVisibleCore(bool value)
        {
            if (!AllowVislble)
            {
                value = false;

                if (!IsHandleCreated)
                    CreateHandle();
            }

            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!AllowClose)
            {
                e.Cancel = true;

                Hide();
            }

            base.OnFormClosing(e);
        }

        internal bool CheckWebAuthnVersion(uint minimumVersion)
        {
            if (WebAuthnApi.Version == null)
            {
                _ = MessageBox.Show(this, "WebAuthn is not available.  Please upgrade to a newer version of Windows.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (WebAuthnApi.Version < minimumVersion)
            {
                _ = MessageBox.Show(this, "The installed version of WebAuthn is too old.  Please upgrade to a newer version of Windows.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void LoadKeyFromFile(string filePath) => LoadKeyFromFileCore(filePath, async: false).GetAwaiter().GetResult();

        private async Task LoadKeyFromFileCore(string filePath, bool async)
        {
            try
            {
                string data = async
                    ? await File.ReadAllTextAsync(filePath).ConfigureAwait(true)
                    : File.ReadAllText(filePath);

                SshKey key;
                string comment;
                try
                {
                    (key, comment) = SshKey.ParseOpenSshPrivateKey(data);
                }
                catch (Exception ex)
                    when (ex is InvalidDataException ||
                          ex is SshWireContentException)
                {
                    throw new InvalidDataException($"Could not read an OpenSSH private key from file '{filePath}'.", ex);
                }

                switch (key.KeyTypeInfo.Type)
                {
                    case SshKeyType.OpenSshEcdsaSK:
                        if (!CheckWebAuthnVersion(WEBAUTHN_API_VERSION_1))
                            return;
                        break;

                    default:
                        throw new UnreachableException();
                }

                _statusLabel.Text = KeyList.Instance.AddKey(key, comment)
                    ? "Loaded key from file."
                    : "Key was already loaded.";
            }
            catch (Exception ex)
            {
                _statusLabel.Text = "Failed to load key.";

                _ = MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleHandleCreated(object? sender, EventArgs e)
        {
            KeyList.Instance.Changed += HandleKeyListChanged;
            HandleKeyListChanged(KeyList.Instance);

            _openSshPipe = new OpenSshPipe(new HWND(Handle));
            _openSshPipe.StatusChanged += HandlePipeStatusChanged;
            HandlePipeStatusChanged(_openSshPipe, _openSshPipe.Status);
            _openSshPipeStartTask = Task.Run(() => _openSshPipe.StartAsync(CancellationToken.None));

            _pageantPipe = new PageantPipe(new HWND(Handle));
            _pageantPipe.StatusChanged += HandlePipeStatusChanged;
            HandlePipeStatusChanged(_pageantPipe, _pageantPipe.Status);
            _pageantPipeStartTask = Task.Run(() => _pageantPipe.StartAsync(CancellationToken.None));
        }

        private void HandleFormClosed(object sender, FormClosedEventArgs e)
        {
            _openSshPipeStartTask!.GetAwaiter().GetResult();
            _openSshPipe!.StatusChanged -= HandlePipeStatusChanged;
            _openSshPipe.StopAsync().GetAwaiter().GetResult();

            _pageantPipeStartTask!.GetAwaiter().GetResult();
            _pageantPipe!.StatusChanged -= HandlePipeStatusChanged;
            _pageantPipe.StopAsync().GetAwaiter().GetResult();
        }

        private void HandleLoadFileMenuItemClicked(object sender, EventArgs e)
        {
            _statusLabel.Text = string.Empty;

            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string sshDirectoryPath = Path.Combine(userProfilePath, ".ssh");

            string initialDirectory = sshDirectoryPath;

            if (!Directory.Exists(initialDirectory))
                initialDirectory = userProfilePath;

            var dialog = new OpenFileDialog()
            {
                Filter = "OpenSSH Private Key|*.*",
                InitialDirectory = initialDirectory,
            };
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            string filePath = dialog.FileName;

            _ = LoadKeyFromFileCore(filePath, async: true);
        }

        private void HandleGenerateInSecurityKeyMenuItemClicked(object sender, EventArgs e)
        {
            _statusLabel.Text = string.Empty;

            if (!CheckWebAuthnVersion(WEBAUTHN_API_VERSION_1))
                return;

            if (_makeCredentialWorker.IsBusy)
            {
                _ = MessageBox.Show(this, "Key generation is currently in progress.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var optionsForm = new SKKeyGenerationOptionsForm();
            if (optionsForm.ShowDialog(this) != DialogResult.OK)
                return;
            var options = optionsForm.Result!;

            _statusLabel.Text = "Initiating key generation...";

            var flags = OpenSshSKFlags.UserPresenceRequired;
            if (options.UserVerificationRequired)
                flags |= OpenSshSKFlags.UserVerificationRequired;

            _makeCredentialWorker.RunWorkerAsync(new MakeCredentialParams(
                hWnd: new HWND(Handle),
                rpId: options.Application,
                userId: options.UserId,
                userName: options.UserName,
                keyTypeInfo: options.KeyTypeInfo,
                flags: flags,
                comment: options.Comment));
        }

        private void HandleExitMenuItemClicked(object sender, EventArgs e)
        {
            AllowClose = true;

            Close();
        }

        private void HandleCopyOpenSshKeyAuthorizationMenuItemClicked(object sender, EventArgs e)
        {
            string text = "";
            foreach (KeyListItem item in _keyListView.SelectedItems)
            {
                if (text.Length > 0)
                    text += "\n";
                text += item.Key.GetOpenSshKeyAuthorization(item.Comment);
            }

            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException)
            {
                _ = MessageBox.Show(this, "The clipboard is in use by another application.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleCopyOpenSshPublicKeyMenuItemClicked(object sender, EventArgs e)
        {
            string text = "";
            foreach (KeyListItem item in _keyListView.SelectedItems)
            {
                if (text.Length > 0)
                    text += "\n";
                text += new string(item.Key.FormatOpenSshPublicKey(item.Comment));
            }

            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException)
            {
                _ = MessageBox.Show(this, "The clipboard is in use by another application.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HandleRemoveMenuItemClicked(object sender, EventArgs e)
        {
            foreach (KeyListItem item in _keyListView.SelectedItems)
                _ = KeyList.Instance.RemoveKey(item.Key);
        }

        private void HandleAboutMenuItemClicked(object sender, EventArgs e)
        {
            _ = new AboutForm().ShowDialog(this);
        }

        private void HandleKeyListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = _keyListView.SelectedIndices.Count > 0;
            _copyOpenSshKeyAuthorizationMenuItem.Enabled = hasSelection;
            _copyOpenSshPublicKeyMenuItem.Enabled = hasSelection;
            _removeMenuItem.Enabled = hasSelection;
        }

        private void HandleKeyListChanged(KeyList keyList)
        {
            Invoke(() =>
            {
                var selectedKeys = new HashSet<SshKey>();
                foreach (KeyListItem item in _keyListView.SelectedItems)
                    _ = selectedKeys.Add(item.Key);

                _keyListView.Items.Clear();
                foreach (var item in keyList.GetAllKeys())
                {
                    var listItem = _keyListView.Items.Add(new KeyListItem(item.Key, item.Comment));
                    if (selectedKeys.Contains(item.Key))
                        _ = _keyListView.SelectedIndices.Add(listItem.Index);
                }

                if (_keyListViewColumnHeaderHeight == null && _keyListView.Items.Count > 0)
                    _keyListViewColumnHeaderHeight = _keyListView.GetItemRect(0).Top;

                HandleKeyListViewSelectedIndexChanged(_keyListView, new EventArgs());
            });
        }

        private void HandlePipeStatusChanged(SshAgentPipe pipe, SshAgentPipeStatus status)
        {
            Invoke(() =>
            {
                ToolStripStatusLabel label;
                string appName;
                if (pipe == _openSshPipe)
                {
                    label = _openSshStatusLabel;
                    appName = "OpenSSH";
                }
                else
                {
                    label = _pageantStatusLabel;
                    appName = "Pageant";
                }

                switch (status)
                {
                    case SshAgentPipeStatus.NotStarted:
                    case SshAgentPipeStatus.Starting:
                    case SshAgentPipeStatus.Stopped:
                        label.Image = Resources.disconnect;
                        label.ToolTipText = "Not ready";
                        break;
                    case SshAgentPipeStatus.RunningIdle:
                        label.Image = Resources.connect;
                        label.ToolTipText = "Ready";
                        break;
                    case SshAgentPipeStatus.RunningBusy:
                        label.Image = Resources.hourglass;
                        label.ToolTipText = "Transaction in progress";
                        break;
                    case SshAgentPipeStatus.AlreadyInUse:
                        label.Image = Resources.error;
                        label.ToolTipText = "Pipe already in use by " + appName;
                        break;
                }
            });
        }

        private void HandleKeyListContextMenuOpening(object sender, CancelEventArgs e)
        {
            var listLocation = _keyListView.PointToScreen(default);
            var contextMenuLocation = _keyListContextMenu.Location;
            int x = contextMenuLocation.X - listLocation.X;
            int y = contextMenuLocation.Y - listLocation.Y;
            var hit = _keyListView.HitTest(x, y);
            if (hit?.Item == null || y < _keyListViewColumnHeaderHeight)
            {
                e.Cancel = true;
                return;
            }
        }

        private void HandleNotifyIconClicked(object sender, EventArgs e)
        {
            var args = (MouseEventArgs)e;

            if (args.Button == MouseButtons.Left)
                HandleShowMenuItemClicked(sender, e);
        }

        private void HandleShowMenuItemClicked(object sender, EventArgs e)
        {
            if (!Visible)
            {
                AllowVislble = true;

                Show();
            }

            Activate();
        }

        private void HandleMakeCredentialWorkerWork(object sender, DoWorkEventArgs e)
        {
            var @params = (MakeCredentialParams)e.Argument!;
            HWND hWnd = @params.HWnd;
            string rpId = @params.RPId;
            byte[] userId = @params.UserId;
            string userName = @params.UserName;
            var keyTypeInfo = @params.KeyTypeInfo;
            var flags = @params.Flags;
            string comment = @params.Comment;

            byte[] challenge = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(challenge);

            var result = WebAuthnApi.MakeCredential(hWnd, rpId, userId, userName, keyTypeInfo, flags, challenge, CancellationToken.None);

            var attestedCredentialData = result.AuthenticatorData.AttestedCredentialData;
            if (attestedCredentialData == null)
                throw new InvalidDataException("Security key response did not contain a credential.");

            var webAuthnKey = attestedCredentialData.CredentialPublicKey;

            // OpenSSH clears the UserVerificationRequired flag if authenticator info options
            // includes "uv", but we don't have access to authenticator info.  And we don't want
            // to clear it anyway since it's how we decide whether to require user verification
            // from the authenticator.

            SshKey sshKey;
            switch (webAuthnKey.KeyType)
            {
                case CoseKeyType.EC2:
                {
                    var webAuthnEC2Key = (CoseEC2Key)webAuthnKey;

                    var application = Encoding.UTF8.GetBytes(rpId).ToImmutableArray();
                    var keyHandle = attestedCredentialData.CredentialId;
                    sshKey = webAuthnEC2Key.ToOpenSshKey(application, flags, keyHandle);
                    break;
                }
                default:
                    throw new UnreachableException();
            }

            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string sshDirectoryPath = Path.Combine(userProfilePath, ".ssh");

            if (!Directory.Exists(sshDirectoryPath))
            {
                try
                {
                    _ = Directory.CreateDirectory(sshDirectoryPath);
                }
                catch (Exception ex)
                    when (ex is ArgumentException ||
                          ex is PathTooLongException ||
                          ex is DirectoryNotFoundException ||
                          ex is UnauthorizedAccessException ||
                          ex is IOException)
                {
                    // Nothing to be done about it.
                }
            }

            var dialog = new SaveFileDialog()
            {
                FileName = GetDefaultKeyFileName(keyTypeInfo),
                Filter = "OpenSSH Private Key|*.*",
                InitialDirectory = sshDirectoryPath,
                OverwritePrompt = true,
            };
            var saveResult = Invoke(() => dialog.ShowDialog(this));
            if (saveResult != DialogResult.OK)
                return;

            using (var fileStream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
                fileWriter.Write(sshKey.FormatOpenSshPrivateKey(comment));

            using (var fileStream = new FileStream(dialog.FileName + ".pub", FileMode.Create, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
                fileWriter.Write(sshKey.FormatOpenSshPublicKey(comment));

            e.Result = new MakeCredentialResults(sshKey, comment);

            static string GetDefaultKeyFileName(SshKeyTypeInfo keyTypeInfo)
            {
                return keyTypeInfo.Type switch
                {
                    SshKeyType.OpenSshEcdsaSK => "id_ecdsa_sk",
                    _ => throw new UnreachableException(),
                };
            }
        }

        private void HandleMakeCredentialWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error is OperationCanceledException)
            {
                _statusLabel.Text = "Key generation was canceled.";
            }
            else if (e.Error != null)
            {
                _statusLabel.Text = "Key generation failed.";

                _ = MessageBox.Show(this, e.Error.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _statusLabel.Text = "Key generation succeeded.";

                var results = (MakeCredentialResults)e.Result!;

                var queryResult = MessageBox.Show(this, "Load the generated key?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (queryResult == DialogResult.Yes)
                    _ = KeyList.Instance.AddKey(results.Key, results.Comment);
            }
        }

        private sealed class MakeCredentialParams
        {
            public MakeCredentialParams(HWND hWnd, string rpId, byte[] userId, string userName, SshKeyTypeInfo keyTypeInfo, OpenSshSKFlags flags, string comment)
            {
                HWnd = hWnd;
                RPId = rpId;
                UserId = userId;
                UserName = userName;
                KeyTypeInfo = keyTypeInfo;
                Flags = flags;
                Comment = comment;
            }

            public HWND HWnd { get; }
            public string RPId { get; }
            public byte[] UserId { get; }
            public string UserName { get; }
            public SshKeyTypeInfo KeyTypeInfo { get; }
            public OpenSshSKFlags Flags { get; }
            public string Comment { get; internal set; }
        }

        private sealed class MakeCredentialResults
        {
            public MakeCredentialResults(SshKey key, string comment)
            {
                Key = key;
                Comment = comment;
            }

            public SshKey Key { get; }
            public string Comment { get; }
        }

        private sealed class KeyListItem : ListViewItem
        {
            public KeyListItem(SshKey key, string comment)
            {
                Key = key;
                Comment = comment;

                Text = key.KeyTypeInfo.Name;
                _ = SubItems.Add("SHA256:" + Convert.ToBase64String(Key.GetSha256Fingerprint()).TrimEnd('='));
                _ = SubItems.Add(comment);
            }

            public SshKey Key { get; }

            public string Comment { get; }
        }
    }
}
