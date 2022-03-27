using System.Windows.Forms;

namespace SKSshAgent
{
    partial class KeyListForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator _keyMenuSeparator1;
            System.Windows.Forms.ToolStripSeparator _keyMenuSeparator2;
            System.Windows.Forms.ToolStripSeparator _notifyIconContextMenuSeparator1;
            this._keyListTypeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._keyListFingerprintColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._keyListCommentColumnHeader = new System.Windows.Forms.ColumnHeader();
            this._keyListView = new System.Windows.Forms.ListView();
            this._keyListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._decryptContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyOpenSshKeyAuthorizationContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyOpenSshPublicKeyContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._removeKeyContextMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._keyListImageList = new System.Windows.Forms.ImageList(this.components);
            this._keyMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._loadFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._generateInSecurityKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._decryptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyOpenSshKeyAuthorizationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._copyOpenSshPublicKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._removeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._helpMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._openSshStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._pageantStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._notifyIconContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._loadFileNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._showNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._exitNotifyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _keyMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _keyMenuSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            _notifyIconContextMenuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._keyListContextMenu.SuspendLayout();
            this._menuStrip.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._notifyIconContextMenu.SuspendLayout();
            this.SuspendLayout();
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
            this._keyListTypeColumnHeader.Text = "Type";
            this._keyListTypeColumnHeader.Width = 180;
            // 
            // _keyListFingerprintColumnHeader
            // 
            this._keyListFingerprintColumnHeader.Text = "Fingerprint";
            this._keyListFingerprintColumnHeader.Width = 360;
            // 
            // _keyListCommentColumnHeader
            // 
            this._keyListCommentColumnHeader.Text = "Comment";
            this._keyListCommentColumnHeader.Width = 220;
            // 
            // _keyListView
            // 
            this._keyListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._keyListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._keyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._keyListTypeColumnHeader,
            this._keyListFingerprintColumnHeader,
            this._keyListCommentColumnHeader});
            this._keyListView.ContextMenuStrip = this._keyListContextMenu;
            this._keyListView.FullRowSelect = true;
            this._keyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._keyListView.Location = new System.Drawing.Point(0, 23);
            this._keyListView.Margin = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this._keyListView.Name = "_keyListView";
            this._keyListView.Size = new System.Drawing.Size(784, 315);
            this._keyListView.SmallImageList = this._keyListImageList;
            this._keyListView.TabIndex = 0;
            this._keyListView.UseCompatibleStateImageBehavior = false;
            this._keyListView.View = System.Windows.Forms.View.Details;
            this._keyListView.SelectedIndexChanged += new System.EventHandler(this.HandleKeyListViewSelectedIndexChanged);
            // 
            // _keyListContextMenu
            // 
            this._keyListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._decryptContextMenuItem,
            this._copyOpenSshKeyAuthorizationContextMenuItem,
            this._copyOpenSshPublicKeyContextMenuItem,
            this._removeKeyContextMenuItem});
            this._keyListContextMenu.Name = "_keyListContextMenuStrip";
            this._keyListContextMenu.Size = new System.Drawing.Size(253, 92);
            this._keyListContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.HandleKeyListContextMenuOpening);
            // 
            // _decryptContextMenuItem
            // 
            this._decryptContextMenuItem.Image = global::SKSshAgent.Properties.Resources.lock_go;
            this._decryptContextMenuItem.Name = "_decryptContextMenuItem";
            this._decryptContextMenuItem.Size = new System.Drawing.Size(252, 22);
            this._decryptContextMenuItem.Text = "&Decrypt";
            this._decryptContextMenuItem.Click += new System.EventHandler(this.HandleDecryptMenuItemClicked);
            // 
            // _copyOpenSshKeyAuthorizationContextMenuItem
            // 
            this._copyOpenSshKeyAuthorizationContextMenuItem.Image = global::SKSshAgent.Properties.Resources.page_white_copy;
            this._copyOpenSshKeyAuthorizationContextMenuItem.Name = "_copyOpenSshKeyAuthorizationContextMenuItem";
            this._copyOpenSshKeyAuthorizationContextMenuItem.Size = new System.Drawing.Size(252, 22);
            this._copyOpenSshKeyAuthorizationContextMenuItem.Text = "&Copy OpenSSH Key Authorization";
            this._copyOpenSshKeyAuthorizationContextMenuItem.Click += new System.EventHandler(this.HandleCopyOpenSshKeyAuthorizationMenuItemClicked);
            // 
            // _copyOpenSshPublicKeyContextMenuItem
            // 
            this._copyOpenSshPublicKeyContextMenuItem.Image = global::SKSshAgent.Properties.Resources.page_white_copy;
            this._copyOpenSshPublicKeyContextMenuItem.Name = "_copyOpenSshPublicKeyContextMenuItem";
            this._copyOpenSshPublicKeyContextMenuItem.Size = new System.Drawing.Size(252, 22);
            this._copyOpenSshPublicKeyContextMenuItem.Text = "Copy OpenSSH &Public Key";
            this._copyOpenSshPublicKeyContextMenuItem.Click += new System.EventHandler(this.HandleCopyOpenSshPublicKeyMenuItemClicked);
            // 
            // _removeKeyContextMenuItem
            // 
            this._removeKeyContextMenuItem.Image = global::SKSshAgent.Properties.Resources.key_delete;
            this._removeKeyContextMenuItem.Name = "_removeKeyContextMenuItem";
            this._removeKeyContextMenuItem.Size = new System.Drawing.Size(252, 22);
            this._removeKeyContextMenuItem.Text = "&Remove";
            this._removeKeyContextMenuItem.Click += new System.EventHandler(this.HandleRemoveMenuItemClicked);
            // 
            // _keyListImageList
            // 
            this._keyListImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._keyListImageList.ImageSize = new System.Drawing.Size(16, 16);
            this._keyListImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // _keyMenu
            // 
            this._keyMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loadFileMenuItem,
            _keyMenuSeparator1,
            this._generateInSecurityKeyMenuItem,
            _keyMenuSeparator2,
            this._exitMenuItem});
            this._keyMenu.Name = "_keyMenu";
            this._keyMenu.Size = new System.Drawing.Size(38, 22);
            this._keyMenu.Text = "&Key";
            // 
            // _loadFileMenuItem
            // 
            this._loadFileMenuItem.Image = global::SKSshAgent.Properties.Resources.folder_key;
            this._loadFileMenuItem.Name = "_loadFileMenuItem";
            this._loadFileMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this._loadFileMenuItem.Size = new System.Drawing.Size(210, 22);
            this._loadFileMenuItem.Text = "&Load File...";
            this._loadFileMenuItem.Click += new System.EventHandler(this.HandleLoadFileMenuItemClicked);
            // 
            // _generateInSecurityKeyMenuItem
            // 
            this._generateInSecurityKeyMenuItem.Image = global::SKSshAgent.Properties.Resources.key_go;
            this._generateInSecurityKeyMenuItem.Name = "_generateInSecurityKeyMenuItem";
            this._generateInSecurityKeyMenuItem.Size = new System.Drawing.Size(210, 22);
            this._generateInSecurityKeyMenuItem.Text = "&Generate in Security Key...";
            this._generateInSecurityKeyMenuItem.Click += new System.EventHandler(this.HandleGenerateInSecurityKeyMenuItemClicked);
            // 
            // _exitMenuItem
            // 
            this._exitMenuItem.Image = global::SKSshAgent.Properties.Resources.door_open;
            this._exitMenuItem.Name = "_exitMenuItem";
            this._exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this._exitMenuItem.Size = new System.Drawing.Size(210, 22);
            this._exitMenuItem.Text = "E&xit";
            this._exitMenuItem.Click += new System.EventHandler(this.HandleExitMenuItemClicked);
            // 
            // _editMenu
            // 
            this._editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._decryptMenuItem,
            this._copyOpenSshKeyAuthorizationMenuItem,
            this._copyOpenSshPublicKeyMenuItem,
            this._removeMenuItem});
            this._editMenu.Name = "_editMenu";
            this._editMenu.Size = new System.Drawing.Size(39, 22);
            this._editMenu.Text = "&Edit";
            // 
            // _decryptMenuItem
            // 
            this._decryptMenuItem.Image = global::SKSshAgent.Properties.Resources.lock_go;
            this._decryptMenuItem.Name = "_decryptMenuItem";
            this._decryptMenuItem.Size = new System.Drawing.Size(294, 22);
            this._decryptMenuItem.Text = "&Decrypt";
            this._decryptMenuItem.Click += new System.EventHandler(this.HandleDecryptMenuItemClicked);
            // 
            // _copyOpenSshKeyAuthorizationMenuItem
            // 
            this._copyOpenSshKeyAuthorizationMenuItem.Enabled = false;
            this._copyOpenSshKeyAuthorizationMenuItem.Image = global::SKSshAgent.Properties.Resources.page_white_copy;
            this._copyOpenSshKeyAuthorizationMenuItem.Name = "_copyOpenSshKeyAuthorizationMenuItem";
            this._copyOpenSshKeyAuthorizationMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this._copyOpenSshKeyAuthorizationMenuItem.Size = new System.Drawing.Size(294, 22);
            this._copyOpenSshKeyAuthorizationMenuItem.Text = "&Copy OpenSSH Key Authorization";
            this._copyOpenSshKeyAuthorizationMenuItem.Click += new System.EventHandler(this.HandleCopyOpenSshKeyAuthorizationMenuItemClicked);
            // 
            // _copyOpenSshPublicKeyMenuItem
            // 
            this._copyOpenSshPublicKeyMenuItem.Enabled = false;
            this._copyOpenSshPublicKeyMenuItem.Image = global::SKSshAgent.Properties.Resources.page_white_copy;
            this._copyOpenSshPublicKeyMenuItem.Name = "_copyOpenSshPublicKeyMenuItem";
            this._copyOpenSshPublicKeyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this._copyOpenSshPublicKeyMenuItem.Size = new System.Drawing.Size(294, 22);
            this._copyOpenSshPublicKeyMenuItem.Text = "Copy OpenSSH &Public Key";
            this._copyOpenSshPublicKeyMenuItem.Click += new System.EventHandler(this.HandleCopyOpenSshPublicKeyMenuItemClicked);
            // 
            // _removeMenuItem
            // 
            this._removeMenuItem.Enabled = false;
            this._removeMenuItem.Image = global::SKSshAgent.Properties.Resources.key_delete;
            this._removeMenuItem.Name = "_removeMenuItem";
            this._removeMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this._removeMenuItem.Size = new System.Drawing.Size(294, 22);
            this._removeMenuItem.Text = "&Remove";
            this._removeMenuItem.Click += new System.EventHandler(this.HandleRemoveMenuItemClicked);
            // 
            // _helpMenu
            // 
            this._helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._aboutMenuItem});
            this._helpMenu.Name = "_helpMenu";
            this._helpMenu.Size = new System.Drawing.Size(44, 22);
            this._helpMenu.Text = "&Help";
            // 
            // _aboutMenuItem
            // 
            this._aboutMenuItem.Image = global::SKSshAgent.Properties.Resources.information;
            this._aboutMenuItem.Name = "_aboutMenuItem";
            this._aboutMenuItem.Size = new System.Drawing.Size(107, 22);
            this._aboutMenuItem.Text = "&About";
            this._aboutMenuItem.Click += new System.EventHandler(this.HandleAboutMenuItemClicked);
            // 
            // _menuStrip
            // 
            this._menuStrip.AutoSize = false;
            this._menuStrip.GripMargin = new System.Windows.Forms.Padding(2);
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._keyMenu,
            this._editMenu,
            this._helpMenu});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this._menuStrip.Size = new System.Drawing.Size(784, 22);
            this._menuStrip.TabIndex = 1;
            // 
            // _statusLabel
            // 
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(584, 17);
            this._statusLabel.Spring = true;
            this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _openSshStatusLabel
            // 
            this._openSshStatusLabel.Image = global::SKSshAgent.Properties.Resources.disconnect;
            this._openSshStatusLabel.Name = "_openSshStatusLabel";
            this._openSshStatusLabel.Padding = new System.Windows.Forms.Padding(1);
            this._openSshStatusLabel.Size = new System.Drawing.Size(96, 17);
            this._openSshStatusLabel.Text = "OpenSSH IPC";
            // 
            // _pageantStatusLabel
            // 
            this._pageantStatusLabel.Image = global::SKSshAgent.Properties.Resources.disconnect;
            this._pageantStatusLabel.Name = "_pageantStatusLabel";
            this._pageantStatusLabel.Padding = new System.Windows.Forms.Padding(1);
            this._pageantStatusLabel.Size = new System.Drawing.Size(89, 17);
            this._pageantStatusLabel.Text = "Pageant IPC";
            // 
            // _statusStrip
            // 
            this._statusStrip.AutoSize = false;
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._openSshStatusLabel,
            this._pageantStatusLabel});
            this._statusStrip.Location = new System.Drawing.Point(0, 339);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this._statusStrip.ShowItemToolTips = true;
            this._statusStrip.Size = new System.Drawing.Size(784, 22);
            this._statusStrip.TabIndex = 2;
            // 
            // _notifyIcon
            // 
            this._notifyIcon.ContextMenuStrip = this._notifyIconContextMenu;
            this._notifyIcon.Icon = global::SKSshAgent.Properties.Resources.application_key_icon;
            this._notifyIcon.Text = "SK SSH Agent";
            this._notifyIcon.Visible = true;
            this._notifyIcon.Click += new System.EventHandler(this.HandleNotifyIconClicked);
            // 
            // _notifyIconContextMenu
            // 
            this._notifyIconContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loadFileNotifyMenuItem,
            _notifyIconContextMenuSeparator1,
            this._showNotifyMenuItem,
            this._exitNotifyMenuItem});
            this._notifyIconContextMenu.Name = "_notifyIconContextMenu";
            this._notifyIconContextMenu.Size = new System.Drawing.Size(131, 76);
            // 
            // _loadFileNotifyMenuItem
            // 
            this._loadFileNotifyMenuItem.Image = global::SKSshAgent.Properties.Resources.folder_key;
            this._loadFileNotifyMenuItem.Name = "_loadFileNotifyMenuItem";
            this._loadFileNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
            this._loadFileNotifyMenuItem.Text = "&Load File...";
            this._loadFileNotifyMenuItem.Click += new System.EventHandler(this.HandleLoadFileMenuItemClicked);
            // 
            // _showNotifyMenuItem
            // 
            this._showNotifyMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this._showNotifyMenuItem.Name = "_showNotifyMenuItem";
            this._showNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
            this._showNotifyMenuItem.Text = "Show";
            this._showNotifyMenuItem.Click += new System.EventHandler(this.HandleShowMenuItemClicked);
            // 
            // _exitNotifyMenuItem
            // 
            this._exitNotifyMenuItem.Image = global::SKSshAgent.Properties.Resources.door_open;
            this._exitNotifyMenuItem.Name = "_exitNotifyMenuItem";
            this._exitNotifyMenuItem.Size = new System.Drawing.Size(130, 22);
            this._exitNotifyMenuItem.Text = "Exit";
            this._exitNotifyMenuItem.Click += new System.EventHandler(this.HandleExitMenuItemClicked);
            // 
            // KeyListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this._keyListView);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._menuStrip);
            this.Icon = global::SKSshAgent.Properties.Resources.application_key_icon;
            this.MainMenuStrip = this._menuStrip;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "KeyListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SK SSH Agent";
            this._keyListContextMenu.ResumeLayout(false);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._notifyIconContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ColumnHeader _keyListTypeColumnHeader;
        private ColumnHeader _keyListFingerprintColumnHeader;
        private ColumnHeader _keyListCommentColumnHeader;
        private ListView _keyListView;
        private ToolStripMenuItem _keyMenu;
        private ToolStripMenuItem _loadFileMenuItem;
        private ToolStripMenuItem _generateInSecurityKeyMenuItem;
        private ToolStripMenuItem _exitMenuItem;
        private ToolStripMenuItem _editMenu;
        private ToolStripMenuItem _decryptMenuItem;
        private ToolStripMenuItem _copyOpenSshKeyAuthorizationMenuItem;
        private ToolStripMenuItem _copyOpenSshPublicKeyMenuItem;
        private ToolStripMenuItem _removeMenuItem;
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
}
