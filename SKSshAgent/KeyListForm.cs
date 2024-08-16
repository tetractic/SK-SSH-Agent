// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using SKSshAgent.Cose;
using SKSshAgent.Properties;
using SKSshAgent.Ssh;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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

        private Task? _closingTask;

        private int? _keyListViewColumnHeaderHeight;

        public KeyListForm()
        {
            InitializeComponent();

            HandleCreated += HandleHandleCreated;

            _confirmEachKeyUseMenuItem.Checked = Settings.ConfirmEachKeyUse;

            _keyListImageList.Images.Add(Resources.key);
            _keyListImageList.Images.Add(Resources._lock);

            _notifyIcon.Icon = new Icon(_notifyIcon.Icon!, SystemInformation.SmallIconSize);

            _showNotifyMenuItem.Font = new Font(_showNotifyMenuItem.Font, FontStyle.Bold);
        }

        public bool AllowVisible { get; set; }

        public bool AllowClose { get; set; }

        protected override void SetVisibleCore(bool value)
        {
            if (!AllowVisible)
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

            if (!e.Cancel)
            {
                if (_closingTask == null)
                {
                    _statusLabel.Text = "Shutting down...";

                    Enabled = false;

                    // There is a bit of async work that must be done before the form can close.
                    _closingTask = Task.Run(HandleClosingAsync);

                    // Afterward, the form can actually close.
                    _ = _closingTask.ContinueWith(task => Invoke(() => Close()));

                    e.Cancel = true;
                }
                else if (!_closingTask.IsCompleted)
                {
                    e.Cancel = true;
                }
                else
                {
                    _closingTask.GetAwaiter().GetResult();
                }
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

        internal async Task<SshKey?> DecryptPrivateKeyAsync(SshKey key, bool background = false, CancellationToken cancellationToken = default)
        {
            var owner = background ? null : this;

            while (true)
            {
                var form = new KeyDecryptionForm();
                form.Fingerprint = "SHA256:" + Convert.ToBase64String(key.GetSha256Fingerprint()).TrimEnd('=');
                if (background)
                {
                    form.ShowIcon = true;
                    form.ShowInTaskbar = true;
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.TopMost = true;
                    form.Text += " — " + Text;
                }
                using (cancellationToken.Register(() => form.Invoke(() => form.Close())))
                    if (form.ShowDialog(owner) != DialogResult.OK)
                        return null;

                var password = form.Result;

                try
                {
                    // Decryption could take a while by design, so let's not do it on the UI thread.
                    var result = await Task.Run<(SshKey Key, string Comment)?>(() =>
                    {
                        return key.TryDecryptPrivateKey(password, out var privateKey, out string? privateComment)
                            ? (privateKey, privateComment)
                            : null;
                    }).ConfigureAwait(true);

                    if (result is (SshKey privateKey, string privateComment))
                    {
                        _ = KeyList.Instance.TryUpgradeKey(privateKey, privateComment);

                        return privateKey;
                    }
                }
                catch (Exception ex)
                    when (ex is SshWireContentException ||
                          ex is InvalidDataException ||
                          ex is NotSupportedException ||
                          ex is CryptographicException)
                {
                    _ = MessageBox.Show(owner, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return null;
                }

                _ = MessageBox.Show(owner, "Incorrect password.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

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

                switch (key.KeyTypeInfo.KeyType)
                {
                    case SshKeyType.Ecdsa:
                    case SshKeyType.Ed25519:
                        break;

                    case SshKeyType.OpenSshEcdsaSK:
                    case SshKeyType.OpenSshEd25519SK:
                        if (!CheckWebAuthnVersion(WEBAUTHN_API_VERSION_1))
                            return;
                        break;

                    default:
                        throw new UnreachableException();
                }

                _statusLabel.Text = KeyList.Instance.AddOrUpgradeKey(key, comment)
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

            _openSshPipe = new OpenSshPipe(this);
            _openSshPipe.StatusChanged += HandlePipeStatusChanged;
            HandlePipeStatusChanged(_openSshPipe, _openSshPipe.Status);
            _openSshPipeStartTask = Task.Run(() => _openSshPipe.StartAsync(CancellationToken.None));

            _pageantPipe = new PageantPipe(this);
            _pageantPipe.StatusChanged += HandlePipeStatusChanged;
            HandlePipeStatusChanged(_pageantPipe, _pageantPipe.Status);
            _pageantPipeStartTask = Task.Run(() => _pageantPipe.StartAsync(CancellationToken.None));
        }

        private async Task HandleClosingAsync()
        {
            await _openSshPipeStartTask!.ConfigureAwait(false);
            await _openSshPipe!.StopAsync().ConfigureAwait(false);
            _openSshPipe.StatusChanged -= HandlePipeStatusChanged;

            await _pageantPipeStartTask!.ConfigureAwait(false);
            await _pageantPipe!.StopAsync().ConfigureAwait(false);
            _pageantPipe.StatusChanged -= HandlePipeStatusChanged;
        }

        // ExceptionAdjustment: M:System.Environment.GetFolderPath(System.Environment.SpecialFolder) -T:System.PlatformNotSupportedException
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

        private async void HandleGenerateInSecurityKeyMenuItemClicked(object sender, EventArgs e)
        {
            _statusLabel.Text = string.Empty;

            if (!CheckWebAuthnVersion(WEBAUTHN_API_VERSION_1))
                return;

            var optionsForm = new SKKeyGenerationOptionsForm();
            if (optionsForm.ShowDialog(this) != DialogResult.OK)
                return;
            var options = optionsForm.Result!;

            string rpId = options.ApplicationId;
            byte[] userId = options.UserId;
            string userName = options.UserName;
            var keyTypeInfo = options.KeyTypeInfo;

            var flags = OpenSshSKFlags.UserPresenceRequired;
            if (options.UserVerificationRequired)
                flags |= OpenSshSKFlags.UserVerificationRequired;

            byte[] challenge = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(challenge);

            string comment = options.Comment;

            _statusLabel.Text = "Initiating key generation...";

            SshKey key;
            try
            {
                var hWnd = new HWND(Handle);
                var result = await Task.Run(() => WebAuthnApi.MakeCredential(hWnd, rpId, userId, userName, keyTypeInfo, flags, challenge, CancellationToken.None)).ConfigureAwait(true);

                var attestedCredentialData = result.AuthenticatorData.AttestedCredentialData;
                if (attestedCredentialData == null)
                    throw new InvalidDataException("Security key response did not contain a credential.");

                var webAuthnKey = attestedCredentialData.CredentialPublicKey;

                // OpenSSH clears the UserVerificationRequired flag if authenticator info options
                // includes "uv", but we don't have access to authenticator info.  And we don't want
                // to clear it anyway since it's how we decide whether to require user verification
                // from the authenticator.

                var application = Encoding.UTF8.GetBytes(rpId).ToImmutableArray();
                var keyHandle = attestedCredentialData.CredentialId;

                switch (webAuthnKey.KeyType)
                {
                    case CoseKeyType.Okp:
                    {
                        var webAuthnOkpKey = (CoseOkpKey)webAuthnKey;

                        key = webAuthnOkpKey.ToOpenSshKey(application, flags, keyHandle);
                        break;
                    }
                    case CoseKeyType.EC2:
                    {
                        var webAuthnEC2Key = (CoseEC2Key)webAuthnKey;

                        key = webAuthnEC2Key.ToOpenSshKey(application, flags, keyHandle);
                        break;
                    }
                    default:
                        throw new UnreachableException();
                }

                if (!await TrySaveKeyFileAsync(key, comment, options.Password, options.KdfInfo, options.KdfRounds, options.CipherInfo))
                    return;
            }
            catch (OperationCanceledException)
            {
                _statusLabel.Text = "Key generation was canceled.";

                return;
            }
            catch (Exception ex)
            {
                _statusLabel.Text = "Key generation failed.";

                _ = MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _statusLabel.Text = "Key generation succeeded.";

            var queryResult = MessageBox.Show(this, "Load the generated key?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (queryResult == DialogResult.Yes)
                _ = KeyList.Instance.AddOrUpgradeKey(key, comment);
        }

        private async void HandleGenerateMenuItemClicked(object sender, EventArgs e)
        {
            _statusLabel.Text = string.Empty;

            var optionsForm = new KeyGenerationOptionsForm();
            if (optionsForm.ShowDialog(this) != DialogResult.OK)
                return;
            var options = optionsForm.Result!;

            var keyTypeInfo = options.KeyTypeInfo;

            string comment = options.Comment;

            SshKey key;
            try
            {
                switch (keyTypeInfo.KeyType)
                {
                    case SshKeyType.Ecdsa:
                        key = SshEcdsaKey.Generate(keyTypeInfo);
                        break;

                    case SshKeyType.Ed25519:
                        key = SshEd25519Key.Generate(keyTypeInfo);
                        break;

                    default:
                        throw new UnreachableException();
                }

                if (!await TrySaveKeyFileAsync(key, comment, options.Password, options.KdfInfo, options.KdfRounds, options.CipherInfo))
                    return;
            }
            catch (Exception ex)
            {
                _statusLabel.Text = "Key generation failed.";

                _ = MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _statusLabel.Text = "Key generation succeeded.";

            var queryResult = MessageBox.Show(this, "Load the generated key?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (queryResult == DialogResult.Yes)
                _ = KeyList.Instance.AddOrUpgradeKey(key, comment);
        }

        // ExceptionAdjustment: M:System.Environment.GetFolderPath(System.Environment.SpecialFolder) -T:System.PlatformNotSupportedException
        private async Task<bool> TrySaveKeyFileAsync(SshKey key, string comment, ShieldedImmutableBuffer password, SshKdfInfo kdfInfo, uint kdfRounds, SshCipherInfo cipherInfo)
        {
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
                          ex is NotSupportedException ||
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
                FileName = key.KeyTypeInfo.KeyType.GetDefaultFileName(),
                Filter = "OpenSSH Private Key|*.*",
                InitialDirectory = sshDirectoryPath,
                OverwritePrompt = true,
            };
            var saveResult = dialog.ShowDialog(this);
            if (saveResult != DialogResult.OK)
                return false;

            string filePath = dialog.FileName;

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
            {
                // Encryption could take a while by design, so let's not do it on the UI thread.
                char[] formattedPrivateKey = await Task.Run(() => key.FormatOpenSshPrivateKey(comment, password, kdfInfo, kdfRounds, cipherInfo)).ConfigureAwait(true);

                await fileWriter.WriteAsync(formattedPrivateKey).ConfigureAwait(true);
            }

            using (var fileStream = new FileStream(filePath + ".pub", FileMode.Create, FileAccess.Write))
            using (var fileWriter = new StreamWriter(fileStream))
                await fileWriter.WriteAsync(key.FormatOpenSshPublicKey(comment)).ConfigureAwait(true);

            return true;
        }

        private void HandleExitMenuItemClicked(object sender, EventArgs e)
        {
            AllowClose = true;

            Close();
        }

        private async void HandleDecryptMenuItemClicked(object sender, EventArgs e)
        {
            foreach (KeyListViewItem item in _keyListView.SelectedItems)
            {
                if (item.Key.EncryptedPrivateKey != null &&
                    await DecryptPrivateKeyAsync(item.Key).ConfigureAwait(true) is null)
                {
                    break;
                }
            }
        }

        private void HandleCopyOpenSshKeyAuthorizationMenuItemClicked(object sender, EventArgs e)
        {
            bool warnAboutMissingOptions = false;

            string text = "";
            foreach (KeyListViewItem item in _keyListView.SelectedItems)
            {
                var key = item.Key;

                if (!key.HasDecryptedPrivateKey)
                {
                    switch (key.KeyTypeInfo.KeyType)
                    {
                        case SshKeyType.Ecdsa:
                        case SshKeyType.Ed25519:
                            break;

                        case SshKeyType.OpenSshEcdsaSK:
                        case SshKeyType.OpenSshEd25519SK:
                            warnAboutMissingOptions = true;
                            break;

                        default:
                            throw new UnreachableException();
                    }
                }

                if (text.Length > 0)
                    text += "\n";
                text += key.GetOpenSshKeyAuthorization(item.Comment);
            }

            try
            {
                Clipboard.SetText(text);
            }
            catch (ExternalException)
            {
                _ = MessageBox.Show(this, "The clipboard is in use by another application.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (warnAboutMissingOptions)
                _ = MessageBox.Show(this, "The copied key authorization may be missing options that are necessary for it to work as intended.  Key authorization options cannot be determined while a key is encrypted.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void HandleCopyOpenSshPublicKeyMenuItemClicked(object sender, EventArgs e)
        {
            string text = "";
            foreach (KeyListViewItem item in _keyListView.SelectedItems)
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
            foreach (KeyListViewItem item in _keyListView.SelectedItems)
                _ = KeyList.Instance.RemoveKey(item.Key);
        }

        private void HandleConfirmEachKeyUseMenuItemCheckedChanged(object sender, EventArgs e)
        {
            Settings.ConfirmEachKeyUse = _confirmEachKeyUseMenuItem.Checked;
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

            bool hasEncryptedSelection = false;
            foreach (KeyListViewItem item in _keyListView.SelectedItems)
            {
                if (item.Key.EncryptedPrivateKey != null)
                {
                    hasEncryptedSelection = true;
                    break;
                }
            }
            _decryptMenuItem.Enabled = hasEncryptedSelection;
            _decryptContextMenuItem.Enabled = hasEncryptedSelection;
        }

        private void HandleKeyListChanged(KeyList keyList)
        {
            Invoke(() =>
            {
                var keyListItems = keyList.GetAllKeys();

                var keySet = new HashSet<SshKey>(SshPublicKeyEqualityComparer.Instance);
                foreach (var item in keyListItems)
                    _ = keySet.Add(item.Key);

                ListViewItem? focusedItem = _keyListView.FocusedItem;
                bool focus = focusedItem != null;
                if (focus)
                {
                    int focusedIndex = _keyListView.Items.IndexOf(focusedItem!);
                    if (focusedIndex >= 0)
                        focusedItem = Refocus(keySet, focusedIndex);
                }

                var listViewItems = new Dictionary<SshKey, KeyListViewItem>(SshPublicKeyEqualityComparer.Instance);
                foreach (KeyListViewItem item in _keyListView.Items)
                    listViewItems.Add(item.Key, item);

                _keyListView.Items.Clear();
                foreach (var item in keyListItems)
                {
                    KeyListViewItem? listViewItem;
                    if (listViewItems.TryGetValue(item.Key, out listViewItem))
                    {
                        listViewItem.Key = item.Key;
                        listViewItem.Comment = item.Comment;
                    }
                    else
                    {
                        listViewItem = new KeyListViewItem(item.Key, item.Comment);
                    }
                    _ = _keyListView.Items.Add(listViewItem);
                }

                if (focus)
                {
                    if (focusedItem == null && _keyListView.Items.Count > 0)
                        focusedItem = _keyListView.Items[0];

                    _keyListView.FocusedItem = focusedItem;
                }

                if (_keyListViewColumnHeaderHeight == null && _keyListView.Items.Count > 0)
                    _keyListViewColumnHeaderHeight = _keyListView.GetItemRect(0).Top;

                HandleKeyListViewSelectedIndexChanged(_keyListView, new EventArgs());
            });

            ListViewItem? Refocus(HashSet<SshKey> keySet, int focusedIndex)
            {
                for (int i = focusedIndex; i < _keyListView.Items.Count; ++i)
                {
                    var item = (KeyListViewItem)_keyListView.Items[i];
                    if (keySet.Contains(item.Key))
                        return item;
                }

                for (int i = focusedIndex - 1; i >= 0; --i)
                {
                    var item = (KeyListViewItem)_keyListView.Items[i];
                    if (keySet.Contains(item.Key))
                        return item;
                }

                return null;
            }
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
                AllowVisible = true;

                Show();
            }

            Activate();
        }

        private sealed class KeyListViewItem : ListViewItem
        {
            private SshKey _key;
            private string _comment;

            public KeyListViewItem(SshKey key, string comment)
            {
                _ = SubItems.Add(string.Empty);
                _ = SubItems.Add(string.Empty);

                Key = key;
                Comment = comment;
            }

            public SshKey Key
            {
                get => _key;
                [MemberNotNull(nameof(_key))]
                set
                {
                    _key = value;

                    bool isEncrypted = _key.EncryptedPrivateKey != null;

                    ImageIndex = isEncrypted ? 1 : 0;
                    Text = _key.KeyTypeInfo.Name;
                    SubItems[1].Text = "SHA256:" + Convert.ToBase64String(Key.GetSha256Fingerprint()).TrimEnd('=');
                }
            }

            public string Comment
            {
                get => _comment;
                [MemberNotNull(nameof(_comment))]
                set
                {
                    _comment = value;

                    SubItems[2].Text = _comment;
                }
            }
        }
    }
}
