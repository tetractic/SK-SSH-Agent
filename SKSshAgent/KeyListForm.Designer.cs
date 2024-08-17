// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Windows.Forms;

namespace SKSshAgent;

partial class KeyListForm
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
        ToolStripSeparator _keyMenuSeparator1;
        ToolStripSeparator _keyMenuSeparator2;
        ToolStripSeparator _notifyIconContextMenuSeparator1;
        _keyListTypeColumnHeader = new ColumnHeader();
        _keyListFingerprintColumnHeader = new ColumnHeader();
        _keyListCommentColumnHeader = new ColumnHeader();
        _keyListView = new ListView();
        _keyListContextMenu = new ContextMenuStrip(components);
        _decryptContextMenuItem = new ToolStripMenuItem();
        _copyOpenSshKeyAuthorizationContextMenuItem = new ToolStripMenuItem();
        _copyOpenSshPublicKeyContextMenuItem = new ToolStripMenuItem();
        _removeKeyContextMenuItem = new ToolStripMenuItem();
        _keyListImageList = new ImageList(components);
        _keyMenu = new ToolStripMenuItem();
        _loadFileMenuItem = new ToolStripMenuItem();
        _generateInSecurityKeyMenuItem = new ToolStripMenuItem();
        _generateMenuItem = new ToolStripMenuItem();
        _exitMenuItem = new ToolStripMenuItem();
        _editMenu = new ToolStripMenuItem();
        _decryptMenuItem = new ToolStripMenuItem();
        _copyOpenSshKeyAuthorizationMenuItem = new ToolStripMenuItem();
        _copyOpenSshPublicKeyMenuItem = new ToolStripMenuItem();
        _removeMenuItem = new ToolStripMenuItem();
        _settingsMenuItem = new ToolStripMenuItem();
        _confirmEachKeyUseMenuItem = new ToolStripMenuItem();
        _helpMenu = new ToolStripMenuItem();
        _aboutMenuItem = new ToolStripMenuItem();
        _menuStrip = new MenuStrip();
        _statusLabel = new ToolStripStatusLabel();
        _openSshStatusLabel = new ToolStripStatusLabel();
        _pageantStatusLabel = new ToolStripStatusLabel();
        _statusStrip = new StatusStrip();
        _notifyIcon = new NotifyIcon(components);
        _notifyIconContextMenu = new ContextMenuStrip(components);
        _loadFileNotifyMenuItem = new ToolStripMenuItem();
        _showNotifyMenuItem = new ToolStripMenuItem();
        _exitNotifyMenuItem = new ToolStripMenuItem();
        _keyMenuSeparator1 = new ToolStripSeparator();
        _keyMenuSeparator2 = new ToolStripSeparator();
        _notifyIconContextMenuSeparator1 = new ToolStripSeparator();
        _keyListContextMenu.SuspendLayout();
        _menuStrip.SuspendLayout();
        _statusStrip.SuspendLayout();
        _notifyIconContextMenu.SuspendLayout();
        SuspendLayout();
        // 
        // _keyMenuSeparator1
        // 
        _keyMenuSeparator1.Name = "_keyMenuSeparator1";
        _keyMenuSeparator1.Size = new System.Drawing.Size(207, 6);
        // 
        // _keyMenuSeparator2
        // 
        _keyMenuSeparator2.Name = "_keyMenuSeparator2";
        _keyMenuSeparator2.Size = new System.Drawing.Size(207, 6);
        // 
        // _notifyIconContextMenuSeparator1
        // 
        _notifyIconContextMenuSeparator1.Name = "_notifyIconContextMenuSeparator1";
        _notifyIconContextMenuSeparator1.Size = new System.Drawing.Size(127, 6);
        // 
        // _keyListTypeColumnHeader
        // 
        _keyListTypeColumnHeader.Text = "Type";
        _keyListTypeColumnHeader.Width = 180;
        // 
        // _keyListFingerprintColumnHeader
        // 
        _keyListFingerprintColumnHeader.Text = "Fingerprint";
        _keyListFingerprintColumnHeader.Width = 360;
        // 
        // _keyListCommentColumnHeader
        // 
        _keyListCommentColumnHeader.Text = "Comment";
        _keyListCommentColumnHeader.Width = 220;
        // 
        // _keyListView
        // 
        _keyListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _keyListView.BorderStyle = BorderStyle.None;
        _keyListView.Columns.AddRange(new ColumnHeader[] { _keyListTypeColumnHeader, _keyListFingerprintColumnHeader, _keyListCommentColumnHeader });
        _keyListView.ContextMenuStrip = _keyListContextMenu;
        _keyListView.FullRowSelect = true;
        _keyListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        _keyListView.Location = new System.Drawing.Point(0, 23);
        _keyListView.Margin = new Padding(0, 1, 0, 1);
        _keyListView.Name = "_keyListView";
        _keyListView.Size = new System.Drawing.Size(784, 315);
        _keyListView.SmallImageList = _keyListImageList;
        _keyListView.TabIndex = 0;
        _keyListView.UseCompatibleStateImageBehavior = false;
        _keyListView.View = View.Details;
        _keyListView.SelectedIndexChanged += HandleKeyListViewSelectedIndexChanged;
        // 
        // _keyListContextMenu
        // 
        _keyListContextMenu.Items.AddRange(new ToolStripItem[] { _decryptContextMenuItem, _copyOpenSshKeyAuthorizationContextMenuItem, _copyOpenSshPublicKeyContextMenuItem, _removeKeyContextMenuItem });
        _keyListContextMenu.Name = "_keyListContextMenuStrip";
        _keyListContextMenu.Size = new System.Drawing.Size(253, 92);
        _keyListContextMenu.Opening += HandleKeyListContextMenuOpening;
        // 
        // _decryptContextMenuItem
        // 
        _decryptContextMenuItem.Image = Properties.Resources.lock_go;
        _decryptContextMenuItem.Name = "_decryptContextMenuItem";
        _decryptContextMenuItem.Size = new System.Drawing.Size(252, 22);
        _decryptContextMenuItem.Text = "&Decrypt";
        _decryptContextMenuItem.Click += HandleDecryptMenuItemClicked;
        // 
        // _copyOpenSshKeyAuthorizationContextMenuItem
        // 
        _copyOpenSshKeyAuthorizationContextMenuItem.Image = Properties.Resources.page_white_copy;
        _copyOpenSshKeyAuthorizationContextMenuItem.Name = "_copyOpenSshKeyAuthorizationContextMenuItem";
        _copyOpenSshKeyAuthorizationContextMenuItem.Size = new System.Drawing.Size(252, 22);
        _copyOpenSshKeyAuthorizationContextMenuItem.Text = "&Copy OpenSSH Key Authorization";
        _copyOpenSshKeyAuthorizationContextMenuItem.Click += HandleCopyOpenSshKeyAuthorizationMenuItemClicked;
        // 
        // _copyOpenSshPublicKeyContextMenuItem
        // 
        _copyOpenSshPublicKeyContextMenuItem.Image = Properties.Resources.page_white_copy;
        _copyOpenSshPublicKeyContextMenuItem.Name = "_copyOpenSshPublicKeyContextMenuItem";
        _copyOpenSshPublicKeyContextMenuItem.Size = new System.Drawing.Size(252, 22);
        _copyOpenSshPublicKeyContextMenuItem.Text = "Copy OpenSSH &Public Key";
        _copyOpenSshPublicKeyContextMenuItem.Click += HandleCopyOpenSshPublicKeyMenuItemClicked;
        // 
        // _removeKeyContextMenuItem
        // 
        _removeKeyContextMenuItem.Image = Properties.Resources.key_delete;
        _removeKeyContextMenuItem.Name = "_removeKeyContextMenuItem";
        _removeKeyContextMenuItem.Size = new System.Drawing.Size(252, 22);
        _removeKeyContextMenuItem.Text = "&Remove";
        _removeKeyContextMenuItem.Click += HandleRemoveMenuItemClicked;
        // 
        // _keyListImageList
        // 
        _keyListImageList.ColorDepth = ColorDepth.Depth32Bit;
        _keyListImageList.ImageSize = new System.Drawing.Size(16, 16);
        _keyListImageList.TransparentColor = System.Drawing.Color.Transparent;
        // 
        // _keyMenu
        // 
        _keyMenu.DropDownItems.AddRange(new ToolStripItem[] { _loadFileMenuItem, _keyMenuSeparator1, _generateInSecurityKeyMenuItem, _generateMenuItem, _keyMenuSeparator2, _exitMenuItem });
        _keyMenu.Name = "_keyMenu";
        _keyMenu.Size = new System.Drawing.Size(38, 22);
        _keyMenu.Text = "&Key";
        // 
        // _loadFileMenuItem
        // 
        _loadFileMenuItem.Image = Properties.Resources.folder_key;
        _loadFileMenuItem.Name = "_loadFileMenuItem";
        _loadFileMenuItem.ShortcutKeys = Keys.Control | Keys.O;
        _loadFileMenuItem.Size = new System.Drawing.Size(210, 22);
        _loadFileMenuItem.Text = "&Load File...";
        _loadFileMenuItem.Click += HandleLoadFileMenuItemClicked;
        // 
        // _generateInSecurityKeyMenuItem
        // 
        _generateInSecurityKeyMenuItem.Image = Properties.Resources.key_go;
        _generateInSecurityKeyMenuItem.Name = "_generateInSecurityKeyMenuItem";
        _generateInSecurityKeyMenuItem.Size = new System.Drawing.Size(210, 22);
        _generateInSecurityKeyMenuItem.Text = "&Generate in Security Key...";
        _generateInSecurityKeyMenuItem.Click += HandleGenerateInSecurityKeyMenuItemClicked;
        // 
        // _generateMenuItem
        // 
        _generateMenuItem.Image = Properties.Resources.key_go;
        _generateMenuItem.Name = "_generateMenuItem";
        _generateMenuItem.Size = new System.Drawing.Size(210, 22);
        _generateMenuItem.Text = "Ge&nerate...";
        _generateMenuItem.Click += HandleGenerateMenuItemClicked;
        // 
        // _exitMenuItem
        // 
        _exitMenuItem.Image = Properties.Resources.door_open;
        _exitMenuItem.Name = "_exitMenuItem";
        _exitMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
        _exitMenuItem.Size = new System.Drawing.Size(210, 22);
        _exitMenuItem.Text = "E&xit";
        _exitMenuItem.Click += HandleExitMenuItemClicked;
        // 
        // _editMenu
        // 
        _editMenu.DropDownItems.AddRange(new ToolStripItem[] { _decryptMenuItem, _copyOpenSshKeyAuthorizationMenuItem, _copyOpenSshPublicKeyMenuItem, _removeMenuItem });
        _editMenu.Name = "_editMenu";
        _editMenu.Size = new System.Drawing.Size(39, 22);
        _editMenu.Text = "&Edit";
        // 
        // _decryptMenuItem
        // 
        _decryptMenuItem.Image = Properties.Resources.lock_go;
        _decryptMenuItem.Name = "_decryptMenuItem";
        _decryptMenuItem.Size = new System.Drawing.Size(294, 22);
        _decryptMenuItem.Text = "&Decrypt";
        _decryptMenuItem.Click += HandleDecryptMenuItemClicked;
        // 
        // _copyOpenSshKeyAuthorizationMenuItem
        // 
        _copyOpenSshKeyAuthorizationMenuItem.Enabled = false;
        _copyOpenSshKeyAuthorizationMenuItem.Image = Properties.Resources.page_white_copy;
        _copyOpenSshKeyAuthorizationMenuItem.Name = "_copyOpenSshKeyAuthorizationMenuItem";
        _copyOpenSshKeyAuthorizationMenuItem.ShortcutKeys = Keys.Control | Keys.C;
        _copyOpenSshKeyAuthorizationMenuItem.Size = new System.Drawing.Size(294, 22);
        _copyOpenSshKeyAuthorizationMenuItem.Text = "&Copy OpenSSH Key Authorization";
        _copyOpenSshKeyAuthorizationMenuItem.Click += HandleCopyOpenSshKeyAuthorizationMenuItemClicked;
        // 
        // _copyOpenSshPublicKeyMenuItem
        // 
        _copyOpenSshPublicKeyMenuItem.Enabled = false;
        _copyOpenSshPublicKeyMenuItem.Image = Properties.Resources.page_white_copy;
        _copyOpenSshPublicKeyMenuItem.Name = "_copyOpenSshPublicKeyMenuItem";
        _copyOpenSshPublicKeyMenuItem.ShortcutKeys = Keys.Control | Keys.P;
        _copyOpenSshPublicKeyMenuItem.Size = new System.Drawing.Size(294, 22);
        _copyOpenSshPublicKeyMenuItem.Text = "Copy OpenSSH &Public Key";
        _copyOpenSshPublicKeyMenuItem.Click += HandleCopyOpenSshPublicKeyMenuItemClicked;
        // 
        // _removeMenuItem
        // 
        _removeMenuItem.Enabled = false;
        _removeMenuItem.Image = Properties.Resources.key_delete;
        _removeMenuItem.Name = "_removeMenuItem";
        _removeMenuItem.ShortcutKeys = Keys.Delete;
        _removeMenuItem.Size = new System.Drawing.Size(294, 22);
        _removeMenuItem.Text = "&Remove";
        _removeMenuItem.Click += HandleRemoveMenuItemClicked;
        // 
        // _settingsMenuItem
        // 
        _settingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _confirmEachKeyUseMenuItem });
        _settingsMenuItem.Name = "_settingsMenuItem";
        _settingsMenuItem.Size = new System.Drawing.Size(61, 22);
        _settingsMenuItem.Text = "&Settings";
        // 
        // _confirmEachKeyUseMenuItem
        // 
        _confirmEachKeyUseMenuItem.CheckOnClick = true;
        _confirmEachKeyUseMenuItem.Name = "_confirmEachKeyUseMenuItem";
        _confirmEachKeyUseMenuItem.Size = new System.Drawing.Size(190, 22);
        _confirmEachKeyUseMenuItem.Text = "&Confirm Each Key Use";
        _confirmEachKeyUseMenuItem.CheckedChanged += HandleConfirmEachKeyUseMenuItemCheckedChanged;
        // 
        // _helpMenu
        // 
        _helpMenu.DropDownItems.AddRange(new ToolStripItem[] { _aboutMenuItem });
        _helpMenu.Name = "_helpMenu";
        _helpMenu.Size = new System.Drawing.Size(44, 22);
        _helpMenu.Text = "&Help";
        // 
        // _aboutMenuItem
        // 
        _aboutMenuItem.Image = Properties.Resources.information;
        _aboutMenuItem.Name = "_aboutMenuItem";
        _aboutMenuItem.Size = new System.Drawing.Size(107, 22);
        _aboutMenuItem.Text = "&About";
        _aboutMenuItem.Click += HandleAboutMenuItemClicked;
        // 
        // _menuStrip
        // 
        _menuStrip.AutoSize = false;
        _menuStrip.GripMargin = new Padding(2);
        _menuStrip.Items.AddRange(new ToolStripItem[] { _keyMenu, _editMenu, _settingsMenuItem, _helpMenu });
        _menuStrip.Location = new System.Drawing.Point(0, 0);
        _menuStrip.Name = "_menuStrip";
        _menuStrip.Padding = new Padding(0);
        _menuStrip.Size = new System.Drawing.Size(784, 22);
        _menuStrip.TabIndex = 1;
        // 
        // _statusLabel
        // 
        _statusLabel.Name = "_statusLabel";
        _statusLabel.Size = new System.Drawing.Size(584, 17);
        _statusLabel.Spring = true;
        _statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // _openSshStatusLabel
        // 
        _openSshStatusLabel.Image = Properties.Resources.disconnect;
        _openSshStatusLabel.Name = "_openSshStatusLabel";
        _openSshStatusLabel.Padding = new Padding(1);
        _openSshStatusLabel.Size = new System.Drawing.Size(96, 17);
        _openSshStatusLabel.Text = "OpenSSH IPC";
        // 
        // _pageantStatusLabel
        // 
        _pageantStatusLabel.Image = Properties.Resources.disconnect;
        _pageantStatusLabel.Name = "_pageantStatusLabel";
        _pageantStatusLabel.Padding = new Padding(1);
        _pageantStatusLabel.Size = new System.Drawing.Size(89, 17);
        _pageantStatusLabel.Text = "Pageant IPC";
        // 
        // _statusStrip
        // 
        _statusStrip.AutoSize = false;
        _statusStrip.Items.AddRange(new ToolStripItem[] { _statusLabel, _openSshStatusLabel, _pageantStatusLabel });
        _statusStrip.Location = new System.Drawing.Point(0, 339);
        _statusStrip.Name = "_statusStrip";
        _statusStrip.RenderMode = ToolStripRenderMode.ManagerRenderMode;
        _statusStrip.ShowItemToolTips = true;
        _statusStrip.Size = new System.Drawing.Size(784, 22);
        _statusStrip.TabIndex = 2;
        // 
        // _notifyIcon
        // 
        _notifyIcon.ContextMenuStrip = _notifyIconContextMenu;
        _notifyIcon.Icon = Properties.Resources.console_ssh_key_icon;
        _notifyIcon.Text = "SK SSH Agent";
        _notifyIcon.Visible = true;
        _notifyIcon.Click += HandleNotifyIconClicked;
        // 
        // _notifyIconContextMenu
        // 
        _notifyIconContextMenu.Items.AddRange(new ToolStripItem[] { _loadFileNotifyMenuItem, _notifyIconContextMenuSeparator1, _showNotifyMenuItem, _exitNotifyMenuItem });
        _notifyIconContextMenu.Name = "_notifyIconContextMenu";
        _notifyIconContextMenu.Size = new System.Drawing.Size(131, 76);
        // 
        // _loadFileNotifyMenuItem
        // 
        _loadFileNotifyMenuItem.Image = Properties.Resources.folder_key;
        _loadFileNotifyMenuItem.Name = "_loadFileNotifyMenuItem";
        _loadFileNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
        _loadFileNotifyMenuItem.Text = "&Load File...";
        _loadFileNotifyMenuItem.Click += HandleLoadFileMenuItemClicked;
        // 
        // _showNotifyMenuItem
        // 
        _showNotifyMenuItem.Name = "_showNotifyMenuItem";
        _showNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
        _showNotifyMenuItem.Text = "&Show";
        _showNotifyMenuItem.Click += HandleShowMenuItemClicked;
        // 
        // _exitNotifyMenuItem
        // 
        _exitNotifyMenuItem.Image = Properties.Resources.door_open;
        _exitNotifyMenuItem.Name = "_exitNotifyMenuItem";
        _exitNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
        _exitNotifyMenuItem.Text = "E&xit";
        _exitNotifyMenuItem.Click += HandleExitMenuItemClicked;
        // 
        // KeyListForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = System.Drawing.SystemColors.ControlDark;
        ClientSize = new System.Drawing.Size(784, 361);
        Controls.Add(_keyListView);
        Controls.Add(_statusStrip);
        Controls.Add(_menuStrip);
        Icon = Properties.Resources.console_ssh_key_icon;
        MainMenuStrip = _menuStrip;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new System.Drawing.Size(400, 200);
        Name = "KeyListForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SK SSH Agent";
        _keyListContextMenu.ResumeLayout(false);
        _menuStrip.ResumeLayout(false);
        _menuStrip.PerformLayout();
        _statusStrip.ResumeLayout(false);
        _statusStrip.PerformLayout();
        _notifyIconContextMenu.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private ColumnHeader _keyListTypeColumnHeader;
    private ColumnHeader _keyListFingerprintColumnHeader;
    private ColumnHeader _keyListCommentColumnHeader;
    private ListView _keyListView;
    private ToolStripMenuItem _keyMenu;
    private ToolStripMenuItem _loadFileMenuItem;
    private ToolStripMenuItem _generateInSecurityKeyMenuItem;
    private ToolStripMenuItem _generateMenuItem;
    private ToolStripMenuItem _exitMenuItem;
    private ToolStripMenuItem _editMenu;
    private ToolStripMenuItem _decryptMenuItem;
    private ToolStripMenuItem _copyOpenSshKeyAuthorizationMenuItem;
    private ToolStripMenuItem _copyOpenSshPublicKeyMenuItem;
    private ToolStripMenuItem _removeMenuItem;
    private ToolStripMenuItem _settingsMenuItem;
    private ToolStripMenuItem _confirmEachKeyUseMenuItem;
    private ToolStripMenuItem _helpMenu;
    private ToolStripMenuItem _aboutMenuItem;
    private MenuStrip _menuStrip;
    private ImageList _keyListImageList;
    private ContextMenuStrip _keyListContextMenu;
    private ToolStripMenuItem _decryptContextMenuItem;
    private ToolStripMenuItem _copyOpenSshKeyAuthorizationContextMenuItem;
    private ToolStripMenuItem _copyOpenSshPublicKeyContextMenuItem;
    private ToolStripMenuItem _removeKeyContextMenuItem;
    private ToolStripStatusLabel _statusLabel;
    private ToolStripStatusLabel _openSshStatusLabel;
    private ToolStripStatusLabel _pageantStatusLabel;
    private StatusStrip _statusStrip;
    private NotifyIcon _notifyIcon;
    private ContextMenuStrip _notifyIconContextMenu;
    private ToolStripMenuItem _loadFileNotifyMenuItem;
    private ToolStripMenuItem _showNotifyMenuItem;
    private ToolStripMenuItem _exitNotifyMenuItem;
}
