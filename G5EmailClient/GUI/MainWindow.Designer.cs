namespace G5EmailClient.GUI
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.top_panel = new System.Windows.Forms.Panel();
            this.top_toolstrip = new System.Windows.Forms.ToolStrip();
            this.new_message_button = new System.Windows.Forms.ToolStripButton();
            this.open_inbox_button = new System.Windows.Forms.ToolStripButton();
            this.move_message_dropdown = new System.Windows.Forms.ToolStripDropDownButton();
            this.delete_button = new System.Windows.Forms.ToolStripButton();
            this.toggle_read_button = new System.Windows.Forms.ToolStripButton();
            this.settings_dropdown = new System.Windows.Forms.ToolStripDropDownButton();
            this.user_settings_button = new System.Windows.Forms.ToolStripMenuItem();
            this.add_user_button = new System.Windows.Forms.ToolStripMenuItem();
            this.select_user_button = new System.Windows.Forms.ToolStripMenuItem();
            this.logout_button = new System.Windows.Forms.ToolStripMenuItem();
            this.contacts_button = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.folders_panel = new System.Windows.Forms.Panel();
            this.folders_lisbox = new System.Windows.Forms.ListBox();
            this.account_info_panel = new System.Windows.Forms.Panel();
            this.connected_email_label = new System.Windows.Forms.Label();
            this.inbox_listview = new System.Windows.Forms.ListView();
            this.from_header = new System.Windows.Forms.ColumnHeader();
            this.subject_header = new System.Windows.Forms.ColumnHeader();
            this.main_panel = new System.Windows.Forms.Panel();
            this.main_tab = new System.Windows.Forms.TabControl();
            this.open_message_tab = new System.Windows.Forms.TabPage();
            this.compose_message_tab = new System.Windows.Forms.TabPage();
            this.search_folder_textbox = new System.Windows.Forms.TextBox();
            this.inbox_panel = new System.Windows.Forms.Panel();
            this.top_panel.SuspendLayout();
            this.top_toolstrip.SuspendLayout();
            this.folders_panel.SuspendLayout();
            this.account_info_panel.SuspendLayout();
            this.main_panel.SuspendLayout();
            this.main_tab.SuspendLayout();
            this.inbox_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // top_panel
            // 
            this.top_panel.Controls.Add(this.top_toolstrip);
            this.top_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.top_panel.Location = new System.Drawing.Point(0, 0);
            this.top_panel.Name = "top_panel";
            this.top_panel.Size = new System.Drawing.Size(1118, 30);
            this.top_panel.TabIndex = 0;
            // 
            // top_toolstrip
            // 
            this.top_toolstrip.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.top_toolstrip.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.top_toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.top_toolstrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.top_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.new_message_button,
            this.open_inbox_button,
            this.move_message_dropdown,
            this.delete_button,
            this.toggle_read_button,
            this.settings_dropdown,
            this.contacts_button,
            this.toolStripButton1,
            this.toolStripButton2});
            this.top_toolstrip.Location = new System.Drawing.Point(0, 0);
            this.top_toolstrip.Name = "top_toolstrip";
            this.top_toolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.top_toolstrip.ShowItemToolTips = false;
            this.top_toolstrip.Size = new System.Drawing.Size(1118, 32);
            this.top_toolstrip.TabIndex = 0;
            this.top_toolstrip.Text = "toolStrip1";
            // 
            // new_message_button
            // 
            this.new_message_button.Image = global::G5EmailClient.Properties.Resources.ComposeIcon;
            this.new_message_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.new_message_button.Name = "new_message_button";
            this.new_message_button.Size = new System.Drawing.Size(144, 29);
            this.new_message_button.Text = "New Message";
            // 
            // open_inbox_button
            // 
            this.open_inbox_button.Image = global::G5EmailClient.Properties.Resources.InboxIcon;
            this.open_inbox_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.open_inbox_button.Name = "open_inbox_button";
            this.open_inbox_button.Size = new System.Drawing.Size(82, 29);
            this.open_inbox_button.Text = "Inbox";
            // 
            // move_message_dropdown
            // 
            this.move_message_dropdown.Image = global::G5EmailClient.Properties.Resources.MoveMessageIcon;
            this.move_message_dropdown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.move_message_dropdown.Name = "move_message_dropdown";
            this.move_message_dropdown.Size = new System.Drawing.Size(162, 29);
            this.move_message_dropdown.Text = "Move Message";
            // 
            // delete_button
            // 
            this.delete_button.Image = global::G5EmailClient.Properties.Resources.DeleteIcon;
            this.delete_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(88, 29);
            this.delete_button.Text = "Delete";
            // 
            // toggle_read_button
            // 
            this.toggle_read_button.Image = global::G5EmailClient.Properties.Resources.ReadUnreadIcon;
            this.toggle_read_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggle_read_button.Name = "toggle_read_button";
            this.toggle_read_button.Size = new System.Drawing.Size(140, 29);
            this.toggle_read_button.Text = "Unread/Read";
            // 
            // settings_dropdown
            // 
            this.settings_dropdown.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.settings_dropdown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.user_settings_button,
            this.add_user_button,
            this.select_user_button,
            this.logout_button});
            this.settings_dropdown.Image = global::G5EmailClient.Properties.Resources.Settings_Icon;
            this.settings_dropdown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settings_dropdown.Name = "settings_dropdown";
            this.settings_dropdown.Size = new System.Drawing.Size(110, 29);
            this.settings_dropdown.Text = "Settings";
            // 
            // user_settings_button
            // 
            this.user_settings_button.Image = global::G5EmailClient.Properties.Resources.AccountSettingsIcon;
            this.user_settings_button.Name = "user_settings_button";
            this.user_settings_button.Size = new System.Drawing.Size(199, 32);
            this.user_settings_button.Text = "User Settings";
            // 
            // add_user_button
            // 
            this.add_user_button.Image = global::G5EmailClient.Properties.Resources.AddUserIcon;
            this.add_user_button.Name = "add_user_button";
            this.add_user_button.Size = new System.Drawing.Size(199, 32);
            this.add_user_button.Text = "Add User";
            // 
            // select_user_button
            // 
            this.select_user_button.Image = global::G5EmailClient.Properties.Resources.SelectUserIcon;
            this.select_user_button.Name = "select_user_button";
            this.select_user_button.Size = new System.Drawing.Size(199, 32);
            this.select_user_button.Text = "Select User";
            // 
            // logout_button
            // 
            this.logout_button.Image = global::G5EmailClient.Properties.Resources.LogoutIcon;
            this.logout_button.Name = "logout_button";
            this.logout_button.Size = new System.Drawing.Size(199, 32);
            this.logout_button.Text = "Log Out";
            // 
            // contacts_button
            // 
            this.contacts_button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.contacts_button.Image = global::G5EmailClient.Properties.Resources.ContactsIcon;
            this.contacts_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.contacts_button.Name = "contacts_button";
            this.contacts_button.Size = new System.Drawing.Size(106, 29);
            this.contacts_button.Text = "Contacts";
            this.contacts_button.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton1.Image = global::G5EmailClient.Properties.Resources.RefreshIcon;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(95, 29);
            this.toolStripButton1.Text = "Refresh";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = global::G5EmailClient.Properties.Resources.SpamIcon;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(125, 29);
            this.toolStripButton2.Text = "Mark Spam";
            // 
            // folders_panel
            // 
            this.folders_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.folders_panel.Controls.Add(this.folders_lisbox);
            this.folders_panel.Controls.Add(this.account_info_panel);
            this.folders_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.folders_panel.Location = new System.Drawing.Point(0, 30);
            this.folders_panel.Name = "folders_panel";
            this.folders_panel.Size = new System.Drawing.Size(200, 617);
            this.folders_panel.TabIndex = 1;
            // 
            // folders_lisbox
            // 
            this.folders_lisbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folders_lisbox.FormattingEnabled = true;
            this.folders_lisbox.ItemHeight = 20;
            this.folders_lisbox.Location = new System.Drawing.Point(0, 23);
            this.folders_lisbox.Name = "folders_lisbox";
            this.folders_lisbox.Size = new System.Drawing.Size(198, 592);
            this.folders_lisbox.TabIndex = 1;
            // 
            // account_info_panel
            // 
            this.account_info_panel.Controls.Add(this.connected_email_label);
            this.account_info_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.account_info_panel.Location = new System.Drawing.Point(0, 0);
            this.account_info_panel.Name = "account_info_panel";
            this.account_info_panel.Size = new System.Drawing.Size(198, 23);
            this.account_info_panel.TabIndex = 0;
            // 
            // connected_email_label
            // 
            this.connected_email_label.AutoSize = true;
            this.connected_email_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.connected_email_label.Location = new System.Drawing.Point(0, 1);
            this.connected_email_label.MaximumSize = new System.Drawing.Size(198, 0);
            this.connected_email_label.MinimumSize = new System.Drawing.Size(198, 0);
            this.connected_email_label.Name = "connected_email_label";
            this.connected_email_label.Size = new System.Drawing.Size(198, 20);
            this.connected_email_label.TabIndex = 0;
            this.connected_email_label.Text = "<active email>";
            this.connected_email_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inbox_listview
            // 
            this.inbox_listview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.from_header,
            this.subject_header});
            this.inbox_listview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inbox_listview.Location = new System.Drawing.Point(0, 23);
            this.inbox_listview.Name = "inbox_listview";
            this.inbox_listview.Size = new System.Drawing.Size(400, 592);
            this.inbox_listview.TabIndex = 5;
            this.inbox_listview.UseCompatibleStateImageBehavior = false;
            this.inbox_listview.View = System.Windows.Forms.View.Details;
            // 
            // from_header
            // 
            this.from_header.Text = "From";
            this.from_header.Width = 199;
            // 
            // subject_header
            // 
            this.subject_header.Text = "Subject";
            this.subject_header.Width = 199;
            // 
            // main_panel
            // 
            this.main_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.main_panel.Controls.Add(this.main_tab);
            this.main_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_panel.Location = new System.Drawing.Point(602, 30);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(516, 617);
            this.main_panel.TabIndex = 3;
            // 
            // main_tab
            // 
            this.main_tab.Controls.Add(this.open_message_tab);
            this.main_tab.Controls.Add(this.compose_message_tab);
            this.main_tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_tab.Location = new System.Drawing.Point(0, 0);
            this.main_tab.Name = "main_tab";
            this.main_tab.SelectedIndex = 0;
            this.main_tab.Size = new System.Drawing.Size(514, 615);
            this.main_tab.TabIndex = 0;
            // 
            // open_message_tab
            // 
            this.open_message_tab.Location = new System.Drawing.Point(4, 29);
            this.open_message_tab.Name = "open_message_tab";
            this.open_message_tab.Padding = new System.Windows.Forms.Padding(3);
            this.open_message_tab.Size = new System.Drawing.Size(506, 582);
            this.open_message_tab.TabIndex = 0;
            this.open_message_tab.Text = "Message";
            this.open_message_tab.UseVisualStyleBackColor = true;
            // 
            // compose_message_tab
            // 
            this.compose_message_tab.Location = new System.Drawing.Point(4, 29);
            this.compose_message_tab.Name = "compose_message_tab";
            this.compose_message_tab.Padding = new System.Windows.Forms.Padding(3);
            this.compose_message_tab.Size = new System.Drawing.Size(506, 582);
            this.compose_message_tab.TabIndex = 1;
            this.compose_message_tab.Text = "Compose";
            this.compose_message_tab.UseVisualStyleBackColor = true;
            // 
            // search_folder_textbox
            // 
            this.search_folder_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_folder_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.search_folder_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.search_folder_textbox.Location = new System.Drawing.Point(0, 0);
            this.search_folder_textbox.Name = "search_folder_textbox";
            this.search_folder_textbox.PlaceholderText = "Search..";
            this.search_folder_textbox.Size = new System.Drawing.Size(400, 23);
            this.search_folder_textbox.TabIndex = 4;
            // 
            // inbox_panel
            // 
            this.inbox_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inbox_panel.Controls.Add(this.inbox_listview);
            this.inbox_panel.Controls.Add(this.search_folder_textbox);
            this.inbox_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.inbox_panel.Location = new System.Drawing.Point(200, 30);
            this.inbox_panel.Name = "inbox_panel";
            this.inbox_panel.Size = new System.Drawing.Size(402, 617);
            this.inbox_panel.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1118, 647);
            this.Controls.Add(this.main_panel);
            this.Controls.Add(this.inbox_panel);
            this.Controls.Add(this.folders_panel);
            this.Controls.Add(this.top_panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1018, 647);
            this.Name = "MainWindow";
            this.Text = "Email Client";
            this.top_panel.ResumeLayout(false);
            this.top_panel.PerformLayout();
            this.top_toolstrip.ResumeLayout(false);
            this.top_toolstrip.PerformLayout();
            this.folders_panel.ResumeLayout(false);
            this.account_info_panel.ResumeLayout(false);
            this.account_info_panel.PerformLayout();
            this.main_panel.ResumeLayout(false);
            this.main_tab.ResumeLayout(false);
            this.inbox_panel.ResumeLayout(false);
            this.inbox_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel top_panel;
        private ToolStrip top_toolstrip;
        private ToolStripButton new_message_button;
        private ToolStripButton delete_button;
        private ToolStripButton toggle_read_button;
        private ToolStripDropDownButton move_message_dropdown;
        private ToolStripDropDownButton settings_dropdown;
        private Panel folders_panel;
        private Panel main_panel;
        private ToolStripButton open_inbox_button;
        private ToolStripButton contacts_button;
        private TabControl main_tab;
        private TabPage open_message_tab;
        private TabPage compose_message_tab;
        private ToolStripMenuItem user_settings_button;
        private ToolStripMenuItem logout_button;
        private ToolStripMenuItem select_user_button;
        private ToolStripMenuItem add_user_button;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ListView inbox_listview;
        private ColumnHeader from_header;
        private ColumnHeader subject_header;
        private ListBox folders_lisbox;
        private Panel account_info_panel;
        private Label connected_email_label;
        private TextBox search_folder_textbox;
        private Panel inbox_panel;
    }
}