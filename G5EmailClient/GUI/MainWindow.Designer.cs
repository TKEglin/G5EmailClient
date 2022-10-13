﻿namespace G5EmailClient.GUI
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
            this.components = new System.ComponentModel.Container();
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
            this.refresh_button = new System.Windows.Forms.ToolStripButton();
            this.markspam_panel = new System.Windows.Forms.ToolStripButton();
            this.folders_panel = new System.Windows.Forms.Panel();
            this.folders_lisbox = new System.Windows.Forms.ListBox();
            this.account_info_panel = new System.Windows.Forms.Panel();
            this.active_email_label = new System.Windows.Forms.Label();
            this.main_panel = new System.Windows.Forms.Panel();
            this.main_tab = new System.Windows.Forms.TabControl();
            this.intro_tab = new System.Windows.Forms.TabPage();
            this.welcome_picture_box = new System.Windows.Forms.PictureBox();
            this.open_message_tab = new System.Windows.Forms.TabPage();
            this.msg_mailbody_panel = new System.Windows.Forms.Panel();
            this.msg_body_rtextbox = new System.Windows.Forms.RichTextBox();
            this.msg_padding_panel2 = new System.Windows.Forms.Panel();
            this.msg_padding_panel1 = new System.Windows.Forms.Panel();
            this.msg_buttons_panel = new System.Windows.Forms.Panel();
            this.msg_forward_button = new System.Windows.Forms.Button();
            this.msg_replyall_button = new System.Windows.Forms.Button();
            this.msg_reply_button = new System.Windows.Forms.Button();
            this.msg_senderinfo_panel = new System.Windows.Forms.Panel();
            this.msg_subject_label = new System.Windows.Forms.Label();
            this.msg_senderinfo_padding_panel = new System.Windows.Forms.Panel();
            this.msg_from_label = new System.Windows.Forms.Label();
            this.compose_message_tab = new System.Windows.Forms.TabPage();
            this.cmp_body_panel = new System.Windows.Forms.Panel();
            this.cmp_mailbody_rtextbox = new System.Windows.Forms.RichTextBox();
            this.cmp_padding_panel2 = new System.Windows.Forms.Panel();
            this.cmp_padding_panel1 = new System.Windows.Forms.Panel();
            this.cmp_bottom_panel = new System.Windows.Forms.Panel();
            this.cmp_add_button = new System.Windows.Forms.Button();
            this.cmp_send_button = new System.Windows.Forms.Button();
            this.cmp_info_panel = new System.Windows.Forms.Panel();
            this.cmp_subject_panel = new System.Windows.Forms.Panel();
            this.cmp_subject_underscore_panel = new System.Windows.Forms.Panel();
            this.cmp_subject_textbox = new System.Windows.Forms.TextBox();
            this.cmp_subject_left_panel = new System.Windows.Forms.Panel();
            this.cmp_subject_label = new System.Windows.Forms.Label();
            this.cmp_info_padding_panel3 = new System.Windows.Forms.Panel();
            this.cmp_bcc_panel = new System.Windows.Forms.Panel();
            this.cmp_bcc_underscore_panel = new System.Windows.Forms.Panel();
            this.cmp_bcc_textbox = new System.Windows.Forms.TextBox();
            this.cmp_bcc_left_panel = new System.Windows.Forms.Panel();
            this.cmp_bcc_label = new System.Windows.Forms.Label();
            this.cmp_info_padding_panel2 = new System.Windows.Forms.Panel();
            this.cmp_cc_panel = new System.Windows.Forms.Panel();
            this.cmp_cc_underscore_panel = new System.Windows.Forms.Panel();
            this.cmp_cc_textbox = new System.Windows.Forms.TextBox();
            this.cmp_cc_left_panel = new System.Windows.Forms.Panel();
            this.cmp_cc_label = new System.Windows.Forms.Label();
            this.cmp_info_padding_panel1 = new System.Windows.Forms.Panel();
            this.cmp_to_panel = new System.Windows.Forms.Panel();
            this.cmp_to_underline_panel = new System.Windows.Forms.Panel();
            this.cmp_to_textbox = new System.Windows.Forms.TextBox();
            this.cmp_to_left_panel = new System.Windows.Forms.Panel();
            this.cmp_to_label = new System.Windows.Forms.Label();
            this.message_flow_panel = new G5EmailClient.GUI.EnvelopeFlowPanel();
            this.search_folder_textbox = new System.Windows.Forms.TextBox();
            this.inbox_panel = new System.Windows.Forms.Panel();
            this.brief_control_explain_tooltop = new System.Windows.Forms.ToolTip(this.components);
            this.cmp_add_contextstrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.add_cc_menuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.add_bcc_menuitem = new System.Windows.Forms.ToolStripMenuItem();
            this.top_panel.SuspendLayout();
            this.top_toolstrip.SuspendLayout();
            this.folders_panel.SuspendLayout();
            this.account_info_panel.SuspendLayout();
            this.main_panel.SuspendLayout();
            this.main_tab.SuspendLayout();
            this.intro_tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.welcome_picture_box)).BeginInit();
            this.open_message_tab.SuspendLayout();
            this.msg_mailbody_panel.SuspendLayout();
            this.msg_buttons_panel.SuspendLayout();
            this.msg_senderinfo_panel.SuspendLayout();
            this.compose_message_tab.SuspendLayout();
            this.cmp_body_panel.SuspendLayout();
            this.cmp_bottom_panel.SuspendLayout();
            this.cmp_info_panel.SuspendLayout();
            this.cmp_subject_panel.SuspendLayout();
            this.cmp_subject_left_panel.SuspendLayout();
            this.cmp_bcc_panel.SuspendLayout();
            this.cmp_bcc_left_panel.SuspendLayout();
            this.cmp_cc_panel.SuspendLayout();
            this.cmp_cc_left_panel.SuspendLayout();
            this.cmp_to_panel.SuspendLayout();
            this.cmp_to_left_panel.SuspendLayout();
            this.inbox_panel.SuspendLayout();
            this.cmp_add_contextstrip.SuspendLayout();
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
            this.refresh_button,
            this.markspam_panel});
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
            this.new_message_button.Click += new System.EventHandler(this.new_message_button_Click);
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
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            this.delete_button.MouseEnter += new System.EventHandler(this.delete_button_MouseEnter);
            this.delete_button.MouseLeave += new System.EventHandler(this.delete_button_MouseLeave);
            // 
            // toggle_read_button
            // 
            this.toggle_read_button.Image = global::G5EmailClient.Properties.Resources.ReadUnreadIcon;
            this.toggle_read_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggle_read_button.Name = "toggle_read_button";
            this.toggle_read_button.Size = new System.Drawing.Size(140, 29);
            this.toggle_read_button.Text = "Unread/Read";
            this.toggle_read_button.Click += new System.EventHandler(this.toggle_read_button_Click);
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
            // refresh_button
            // 
            this.refresh_button.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.refresh_button.Image = global::G5EmailClient.Properties.Resources.RefreshIcon;
            this.refresh_button.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(95, 29);
            this.refresh_button.Text = "Refresh";
            // 
            // markspam_panel
            // 
            this.markspam_panel.Image = global::G5EmailClient.Properties.Resources.SpamIcon;
            this.markspam_panel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.markspam_panel.Name = "markspam_panel";
            this.markspam_panel.Size = new System.Drawing.Size(125, 29);
            this.markspam_panel.Text = "Mark Spam";
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
            this.folders_lisbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.folders_lisbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.folders_lisbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.folders_lisbox.FormattingEnabled = true;
            this.folders_lisbox.ItemHeight = 23;
            this.folders_lisbox.Location = new System.Drawing.Point(0, 30);
            this.folders_lisbox.Name = "folders_lisbox";
            this.folders_lisbox.Size = new System.Drawing.Size(198, 585);
            this.folders_lisbox.TabIndex = 2;
            this.folders_lisbox.TabStop = false;
            // 
            // account_info_panel
            // 
            this.account_info_panel.Controls.Add(this.active_email_label);
            this.account_info_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.account_info_panel.Location = new System.Drawing.Point(0, 0);
            this.account_info_panel.Name = "account_info_panel";
            this.account_info_panel.Size = new System.Drawing.Size(198, 30);
            this.account_info_panel.TabIndex = 0;
            // 
            // active_email_label
            // 
            this.active_email_label.AutoSize = true;
            this.active_email_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.active_email_label.Location = new System.Drawing.Point(0, 3);
            this.active_email_label.Margin = new System.Windows.Forms.Padding(0);
            this.active_email_label.MaximumSize = new System.Drawing.Size(198, 0);
            this.active_email_label.MinimumSize = new System.Drawing.Size(198, 0);
            this.active_email_label.Name = "active_email_label";
            this.active_email_label.Size = new System.Drawing.Size(198, 20);
            this.active_email_label.TabIndex = 0;
            this.active_email_label.Text = "<active email>";
            this.active_email_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // main_panel
            // 
            this.main_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.main_panel.Controls.Add(this.main_tab);
            this.main_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_panel.Location = new System.Drawing.Point(552, 30);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(566, 617);
            this.main_panel.TabIndex = 3;
            // 
            // main_tab
            // 
            this.main_tab.Controls.Add(this.intro_tab);
            this.main_tab.Controls.Add(this.open_message_tab);
            this.main_tab.Controls.Add(this.compose_message_tab);
            this.main_tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.main_tab.Location = new System.Drawing.Point(0, 0);
            this.main_tab.Name = "main_tab";
            this.main_tab.SelectedIndex = 0;
            this.main_tab.Size = new System.Drawing.Size(564, 615);
            this.main_tab.TabIndex = 0;
            // 
            // intro_tab
            // 
            this.intro_tab.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.intro_tab.Controls.Add(this.welcome_picture_box);
            this.intro_tab.Location = new System.Drawing.Point(4, 29);
            this.intro_tab.Name = "intro_tab";
            this.intro_tab.Size = new System.Drawing.Size(556, 582);
            this.intro_tab.TabIndex = 2;
            this.intro_tab.Text = "Welcome";
            // 
            // welcome_picture_box
            // 
            this.welcome_picture_box.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.welcome_picture_box.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.welcome_picture_box.Image = global::G5EmailClient.Properties.Resources.Group5Logo;
            this.welcome_picture_box.Location = new System.Drawing.Point(28, 28);
            this.welcome_picture_box.Name = "welcome_picture_box";
            this.welcome_picture_box.Size = new System.Drawing.Size(500, 200);
            this.welcome_picture_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.welcome_picture_box.TabIndex = 0;
            this.welcome_picture_box.TabStop = false;
            // 
            // open_message_tab
            // 
            this.open_message_tab.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.open_message_tab.Controls.Add(this.msg_mailbody_panel);
            this.open_message_tab.Controls.Add(this.msg_padding_panel2);
            this.open_message_tab.Controls.Add(this.msg_padding_panel1);
            this.open_message_tab.Controls.Add(this.msg_buttons_panel);
            this.open_message_tab.Controls.Add(this.msg_senderinfo_panel);
            this.open_message_tab.Location = new System.Drawing.Point(4, 29);
            this.open_message_tab.Name = "open_message_tab";
            this.open_message_tab.Padding = new System.Windows.Forms.Padding(10);
            this.open_message_tab.Size = new System.Drawing.Size(556, 582);
            this.open_message_tab.TabIndex = 0;
            this.open_message_tab.Text = "Message";
            // 
            // msg_mailbody_panel
            // 
            this.msg_mailbody_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.msg_mailbody_panel.Controls.Add(this.msg_body_rtextbox);
            this.msg_mailbody_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msg_mailbody_panel.Location = new System.Drawing.Point(10, 76);
            this.msg_mailbody_panel.Name = "msg_mailbody_panel";
            this.msg_mailbody_panel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.msg_mailbody_panel.Size = new System.Drawing.Size(536, 438);
            this.msg_mailbody_panel.TabIndex = 4;
            this.msg_mailbody_panel.Tag = "";
            // 
            // msg_body_rtextbox
            // 
            this.msg_body_rtextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.msg_body_rtextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msg_body_rtextbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.msg_body_rtextbox.Location = new System.Drawing.Point(3, 0);
            this.msg_body_rtextbox.Name = "msg_body_rtextbox";
            this.msg_body_rtextbox.Size = new System.Drawing.Size(528, 436);
            this.msg_body_rtextbox.TabIndex = 0;
            this.msg_body_rtextbox.Text = "<Message body goes here>";
            // 
            // msg_padding_panel2
            // 
            this.msg_padding_panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msg_padding_panel2.Location = new System.Drawing.Point(10, 514);
            this.msg_padding_panel2.Name = "msg_padding_panel2";
            this.msg_padding_panel2.Size = new System.Drawing.Size(536, 10);
            this.msg_padding_panel2.TabIndex = 3;
            // 
            // msg_padding_panel1
            // 
            this.msg_padding_panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.msg_padding_panel1.Location = new System.Drawing.Point(10, 66);
            this.msg_padding_panel1.Name = "msg_padding_panel1";
            this.msg_padding_panel1.Size = new System.Drawing.Size(536, 10);
            this.msg_padding_panel1.TabIndex = 2;
            // 
            // msg_buttons_panel
            // 
            this.msg_buttons_panel.Controls.Add(this.msg_forward_button);
            this.msg_buttons_panel.Controls.Add(this.msg_replyall_button);
            this.msg_buttons_panel.Controls.Add(this.msg_reply_button);
            this.msg_buttons_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.msg_buttons_panel.Location = new System.Drawing.Point(10, 524);
            this.msg_buttons_panel.Name = "msg_buttons_panel";
            this.msg_buttons_panel.Size = new System.Drawing.Size(536, 48);
            this.msg_buttons_panel.TabIndex = 1;
            // 
            // msg_forward_button
            // 
            this.msg_forward_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.msg_forward_button.BackgroundImage = global::G5EmailClient.Properties.Resources.ForwardIcon;
            this.msg_forward_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.msg_forward_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.msg_forward_button.Location = new System.Drawing.Point(488, 0);
            this.msg_forward_button.Margin = new System.Windows.Forms.Padding(0);
            this.msg_forward_button.Name = "msg_forward_button";
            this.msg_forward_button.Size = new System.Drawing.Size(48, 48);
            this.msg_forward_button.TabIndex = 2;
            this.msg_forward_button.UseVisualStyleBackColor = true;
            // 
            // msg_replyall_button
            // 
            this.msg_replyall_button.BackgroundImage = global::G5EmailClient.Properties.Resources.ReplyAllIcon;
            this.msg_replyall_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.msg_replyall_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.msg_replyall_button.Location = new System.Drawing.Point(58, 0);
            this.msg_replyall_button.Name = "msg_replyall_button";
            this.msg_replyall_button.Size = new System.Drawing.Size(48, 48);
            this.msg_replyall_button.TabIndex = 1;
            this.msg_replyall_button.UseVisualStyleBackColor = true;
            // 
            // msg_reply_button
            // 
            this.msg_reply_button.BackgroundImage = global::G5EmailClient.Properties.Resources.ReplyIcon1;
            this.msg_reply_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.msg_reply_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.msg_reply_button.Location = new System.Drawing.Point(0, 0);
            this.msg_reply_button.Name = "msg_reply_button";
            this.msg_reply_button.Size = new System.Drawing.Size(48, 48);
            this.msg_reply_button.TabIndex = 0;
            this.msg_reply_button.UseVisualStyleBackColor = true;
            // 
            // msg_senderinfo_panel
            // 
            this.msg_senderinfo_panel.AutoSize = true;
            this.msg_senderinfo_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.msg_senderinfo_panel.Controls.Add(this.msg_subject_label);
            this.msg_senderinfo_panel.Controls.Add(this.msg_senderinfo_padding_panel);
            this.msg_senderinfo_panel.Controls.Add(this.msg_from_label);
            this.msg_senderinfo_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.msg_senderinfo_panel.Location = new System.Drawing.Point(10, 10);
            this.msg_senderinfo_panel.Margin = new System.Windows.Forms.Padding(10);
            this.msg_senderinfo_panel.Name = "msg_senderinfo_panel";
            this.msg_senderinfo_panel.Size = new System.Drawing.Size(536, 56);
            this.msg_senderinfo_panel.TabIndex = 0;
            this.msg_senderinfo_panel.Resize += new System.EventHandler(this.msg_senderinfo_panel_Resize);
            // 
            // msg_subject_label
            // 
            this.msg_subject_label.AutoSize = true;
            this.msg_subject_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.msg_subject_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.msg_subject_label.Location = new System.Drawing.Point(0, 33);
            this.msg_subject_label.Margin = new System.Windows.Forms.Padding(10);
            this.msg_subject_label.Name = "msg_subject_label";
            this.msg_subject_label.Size = new System.Drawing.Size(120, 23);
            this.msg_subject_label.TabIndex = 1;
            this.msg_subject_label.Text = "<subject line>";
            // 
            // msg_senderinfo_padding_panel
            // 
            this.msg_senderinfo_padding_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.msg_senderinfo_padding_panel.Location = new System.Drawing.Point(0, 28);
            this.msg_senderinfo_padding_panel.Name = "msg_senderinfo_padding_panel";
            this.msg_senderinfo_padding_panel.Size = new System.Drawing.Size(536, 5);
            this.msg_senderinfo_padding_panel.TabIndex = 2;
            // 
            // msg_from_label
            // 
            this.msg_from_label.AutoSize = true;
            this.msg_from_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.msg_from_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.msg_from_label.Location = new System.Drawing.Point(0, 0);
            this.msg_from_label.Margin = new System.Windows.Forms.Padding(10);
            this.msg_from_label.Name = "msg_from_label";
            this.msg_from_label.Size = new System.Drawing.Size(121, 28);
            this.msg_from_label.TabIndex = 0;
            this.msg_from_label.Text = "<from info>";
            // 
            // compose_message_tab
            // 
            this.compose_message_tab.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.compose_message_tab.Controls.Add(this.cmp_body_panel);
            this.compose_message_tab.Controls.Add(this.cmp_padding_panel2);
            this.compose_message_tab.Controls.Add(this.cmp_padding_panel1);
            this.compose_message_tab.Controls.Add(this.cmp_bottom_panel);
            this.compose_message_tab.Controls.Add(this.cmp_info_panel);
            this.compose_message_tab.Location = new System.Drawing.Point(4, 29);
            this.compose_message_tab.Name = "compose_message_tab";
            this.compose_message_tab.Padding = new System.Windows.Forms.Padding(10);
            this.compose_message_tab.Size = new System.Drawing.Size(556, 582);
            this.compose_message_tab.TabIndex = 3;
            this.compose_message_tab.Text = "Compose";
            // 
            // cmp_body_panel
            // 
            this.cmp_body_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cmp_body_panel.Controls.Add(this.cmp_mailbody_rtextbox);
            this.cmp_body_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_body_panel.Location = new System.Drawing.Point(10, 164);
            this.cmp_body_panel.Name = "cmp_body_panel";
            this.cmp_body_panel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.cmp_body_panel.Size = new System.Drawing.Size(536, 350);
            this.cmp_body_panel.TabIndex = 4;
            this.cmp_body_panel.Tag = "";
            // 
            // cmp_mailbody_rtextbox
            // 
            this.cmp_mailbody_rtextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmp_mailbody_rtextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_mailbody_rtextbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_mailbody_rtextbox.Location = new System.Drawing.Point(3, 0);
            this.cmp_mailbody_rtextbox.Name = "cmp_mailbody_rtextbox";
            this.cmp_mailbody_rtextbox.Size = new System.Drawing.Size(528, 348);
            this.cmp_mailbody_rtextbox.TabIndex = 0;
            this.cmp_mailbody_rtextbox.Text = "";
            // 
            // cmp_padding_panel2
            // 
            this.cmp_padding_panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmp_padding_panel2.Location = new System.Drawing.Point(10, 514);
            this.cmp_padding_panel2.Name = "cmp_padding_panel2";
            this.cmp_padding_panel2.Size = new System.Drawing.Size(536, 10);
            this.cmp_padding_panel2.TabIndex = 3;
            // 
            // cmp_padding_panel1
            // 
            this.cmp_padding_panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_padding_panel1.Location = new System.Drawing.Point(10, 154);
            this.cmp_padding_panel1.Name = "cmp_padding_panel1";
            this.cmp_padding_panel1.Size = new System.Drawing.Size(536, 10);
            this.cmp_padding_panel1.TabIndex = 2;
            // 
            // cmp_bottom_panel
            // 
            this.cmp_bottom_panel.Controls.Add(this.cmp_add_button);
            this.cmp_bottom_panel.Controls.Add(this.cmp_send_button);
            this.cmp_bottom_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmp_bottom_panel.Location = new System.Drawing.Point(10, 524);
            this.cmp_bottom_panel.Name = "cmp_bottom_panel";
            this.cmp_bottom_panel.Size = new System.Drawing.Size(536, 48);
            this.cmp_bottom_panel.TabIndex = 1;
            // 
            // cmp_add_button
            // 
            this.cmp_add_button.BackgroundImage = global::G5EmailClient.Properties.Resources.AddIcon;
            this.cmp_add_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmp_add_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmp_add_button.Location = new System.Drawing.Point(0, 0);
            this.cmp_add_button.Name = "cmp_add_button";
            this.cmp_add_button.Size = new System.Drawing.Size(48, 48);
            this.cmp_add_button.TabIndex = 1;
            this.cmp_add_button.UseVisualStyleBackColor = true;
            this.cmp_add_button.Click += new System.EventHandler(this.cmp_add_button_Click);
            // 
            // cmp_send_button
            // 
            this.cmp_send_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.cmp_send_button.BackgroundImage = global::G5EmailClient.Properties.Resources.SendIcon;
            this.cmp_send_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.cmp_send_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmp_send_button.Location = new System.Drawing.Point(488, 0);
            this.cmp_send_button.Name = "cmp_send_button";
            this.cmp_send_button.Size = new System.Drawing.Size(48, 48);
            this.cmp_send_button.TabIndex = 0;
            this.cmp_send_button.UseVisualStyleBackColor = true;
            // 
            // cmp_info_panel
            // 
            this.cmp_info_panel.AutoSize = true;
            this.cmp_info_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmp_info_panel.Controls.Add(this.cmp_subject_panel);
            this.cmp_info_panel.Controls.Add(this.cmp_info_padding_panel3);
            this.cmp_info_panel.Controls.Add(this.cmp_bcc_panel);
            this.cmp_info_panel.Controls.Add(this.cmp_info_padding_panel2);
            this.cmp_info_panel.Controls.Add(this.cmp_cc_panel);
            this.cmp_info_panel.Controls.Add(this.cmp_info_padding_panel1);
            this.cmp_info_panel.Controls.Add(this.cmp_to_panel);
            this.cmp_info_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_info_panel.Location = new System.Drawing.Point(10, 10);
            this.cmp_info_panel.Margin = new System.Windows.Forms.Padding(10);
            this.cmp_info_panel.Name = "cmp_info_panel";
            this.cmp_info_panel.Size = new System.Drawing.Size(536, 144);
            this.cmp_info_panel.TabIndex = 0;
            // 
            // cmp_subject_panel
            // 
            this.cmp_subject_panel.AutoSize = true;
            this.cmp_subject_panel.Controls.Add(this.cmp_subject_underscore_panel);
            this.cmp_subject_panel.Controls.Add(this.cmp_subject_textbox);
            this.cmp_subject_panel.Controls.Add(this.cmp_subject_left_panel);
            this.cmp_subject_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_subject_panel.Location = new System.Drawing.Point(0, 113);
            this.cmp_subject_panel.Name = "cmp_subject_panel";
            this.cmp_subject_panel.Size = new System.Drawing.Size(536, 31);
            this.cmp_subject_panel.TabIndex = 7;
            // 
            // cmp_subject_underscore_panel
            // 
            this.cmp_subject_underscore_panel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cmp_subject_underscore_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_subject_underscore_panel.Location = new System.Drawing.Point(75, 30);
            this.cmp_subject_underscore_panel.Name = "cmp_subject_underscore_panel";
            this.cmp_subject_underscore_panel.Size = new System.Drawing.Size(461, 1);
            this.cmp_subject_underscore_panel.TabIndex = 3;
            // 
            // cmp_subject_textbox
            // 
            this.cmp_subject_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmp_subject_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_subject_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_subject_textbox.Location = new System.Drawing.Point(75, 0);
            this.cmp_subject_textbox.MinimumSize = new System.Drawing.Size(0, 20);
            this.cmp_subject_textbox.Multiline = true;
            this.cmp_subject_textbox.Name = "cmp_subject_textbox";
            this.cmp_subject_textbox.Size = new System.Drawing.Size(461, 30);
            this.cmp_subject_textbox.TabIndex = 4;
            // 
            // cmp_subject_left_panel
            // 
            this.cmp_subject_left_panel.AutoSize = true;
            this.cmp_subject_left_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmp_subject_left_panel.Controls.Add(this.cmp_subject_label);
            this.cmp_subject_left_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmp_subject_left_panel.Location = new System.Drawing.Point(0, 0);
            this.cmp_subject_left_panel.Name = "cmp_subject_left_panel";
            this.cmp_subject_left_panel.Size = new System.Drawing.Size(75, 31);
            this.cmp_subject_left_panel.TabIndex = 2;
            // 
            // cmp_subject_label
            // 
            this.cmp_subject_label.AutoSize = true;
            this.cmp_subject_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_subject_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_subject_label.Location = new System.Drawing.Point(0, 0);
            this.cmp_subject_label.Margin = new System.Windows.Forms.Padding(3);
            this.cmp_subject_label.Name = "cmp_subject_label";
            this.cmp_subject_label.Size = new System.Drawing.Size(75, 23);
            this.cmp_subject_label.TabIndex = 0;
            this.cmp_subject_label.Text = "Subject: ";
            // 
            // cmp_info_padding_panel3
            // 
            this.cmp_info_padding_panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_info_padding_panel3.Location = new System.Drawing.Point(0, 103);
            this.cmp_info_padding_panel3.Name = "cmp_info_padding_panel3";
            this.cmp_info_padding_panel3.Size = new System.Drawing.Size(536, 10);
            this.cmp_info_padding_panel3.TabIndex = 6;
            // 
            // cmp_bcc_panel
            // 
            this.cmp_bcc_panel.AutoSize = true;
            this.cmp_bcc_panel.Controls.Add(this.cmp_bcc_underscore_panel);
            this.cmp_bcc_panel.Controls.Add(this.cmp_bcc_textbox);
            this.cmp_bcc_panel.Controls.Add(this.cmp_bcc_left_panel);
            this.cmp_bcc_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_bcc_panel.Location = new System.Drawing.Point(0, 72);
            this.cmp_bcc_panel.Name = "cmp_bcc_panel";
            this.cmp_bcc_panel.Size = new System.Drawing.Size(536, 31);
            this.cmp_bcc_panel.TabIndex = 5;
            this.cmp_bcc_panel.Visible = false;
            // 
            // cmp_bcc_underscore_panel
            // 
            this.cmp_bcc_underscore_panel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cmp_bcc_underscore_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_bcc_underscore_panel.Location = new System.Drawing.Point(45, 30);
            this.cmp_bcc_underscore_panel.Name = "cmp_bcc_underscore_panel";
            this.cmp_bcc_underscore_panel.Size = new System.Drawing.Size(491, 1);
            this.cmp_bcc_underscore_panel.TabIndex = 3;
            // 
            // cmp_bcc_textbox
            // 
            this.cmp_bcc_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmp_bcc_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_bcc_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_bcc_textbox.Location = new System.Drawing.Point(45, 0);
            this.cmp_bcc_textbox.MinimumSize = new System.Drawing.Size(0, 30);
            this.cmp_bcc_textbox.Name = "cmp_bcc_textbox";
            this.cmp_bcc_textbox.Size = new System.Drawing.Size(491, 30);
            this.cmp_bcc_textbox.TabIndex = 4;
            // 
            // cmp_bcc_left_panel
            // 
            this.cmp_bcc_left_panel.AutoSize = true;
            this.cmp_bcc_left_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmp_bcc_left_panel.Controls.Add(this.cmp_bcc_label);
            this.cmp_bcc_left_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmp_bcc_left_panel.Location = new System.Drawing.Point(0, 0);
            this.cmp_bcc_left_panel.Name = "cmp_bcc_left_panel";
            this.cmp_bcc_left_panel.Size = new System.Drawing.Size(45, 31);
            this.cmp_bcc_left_panel.TabIndex = 2;
            // 
            // cmp_bcc_label
            // 
            this.cmp_bcc_label.AutoSize = true;
            this.cmp_bcc_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_bcc_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_bcc_label.Location = new System.Drawing.Point(0, 0);
            this.cmp_bcc_label.Margin = new System.Windows.Forms.Padding(3);
            this.cmp_bcc_label.Name = "cmp_bcc_label";
            this.cmp_bcc_label.Size = new System.Drawing.Size(45, 23);
            this.cmp_bcc_label.TabIndex = 0;
            this.cmp_bcc_label.Text = "Bcc: ";
            // 
            // cmp_info_padding_panel2
            // 
            this.cmp_info_padding_panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_info_padding_panel2.Location = new System.Drawing.Point(0, 67);
            this.cmp_info_padding_panel2.Name = "cmp_info_padding_panel2";
            this.cmp_info_padding_panel2.Size = new System.Drawing.Size(536, 5);
            this.cmp_info_padding_panel2.TabIndex = 4;
            // 
            // cmp_cc_panel
            // 
            this.cmp_cc_panel.AutoSize = true;
            this.cmp_cc_panel.Controls.Add(this.cmp_cc_underscore_panel);
            this.cmp_cc_panel.Controls.Add(this.cmp_cc_textbox);
            this.cmp_cc_panel.Controls.Add(this.cmp_cc_left_panel);
            this.cmp_cc_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_cc_panel.Location = new System.Drawing.Point(0, 36);
            this.cmp_cc_panel.Name = "cmp_cc_panel";
            this.cmp_cc_panel.Size = new System.Drawing.Size(536, 31);
            this.cmp_cc_panel.TabIndex = 2;
            this.cmp_cc_panel.Visible = false;
            // 
            // cmp_cc_underscore_panel
            // 
            this.cmp_cc_underscore_panel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cmp_cc_underscore_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_cc_underscore_panel.Location = new System.Drawing.Point(45, 30);
            this.cmp_cc_underscore_panel.Name = "cmp_cc_underscore_panel";
            this.cmp_cc_underscore_panel.Size = new System.Drawing.Size(491, 1);
            this.cmp_cc_underscore_panel.TabIndex = 3;
            // 
            // cmp_cc_textbox
            // 
            this.cmp_cc_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmp_cc_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_cc_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_cc_textbox.Location = new System.Drawing.Point(45, 0);
            this.cmp_cc_textbox.MinimumSize = new System.Drawing.Size(0, 30);
            this.cmp_cc_textbox.Name = "cmp_cc_textbox";
            this.cmp_cc_textbox.Size = new System.Drawing.Size(491, 30);
            this.cmp_cc_textbox.TabIndex = 4;
            // 
            // cmp_cc_left_panel
            // 
            this.cmp_cc_left_panel.AutoSize = true;
            this.cmp_cc_left_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmp_cc_left_panel.Controls.Add(this.cmp_cc_label);
            this.cmp_cc_left_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmp_cc_left_panel.Location = new System.Drawing.Point(0, 0);
            this.cmp_cc_left_panel.Name = "cmp_cc_left_panel";
            this.cmp_cc_left_panel.Size = new System.Drawing.Size(45, 31);
            this.cmp_cc_left_panel.TabIndex = 2;
            // 
            // cmp_cc_label
            // 
            this.cmp_cc_label.AutoSize = true;
            this.cmp_cc_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_cc_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_cc_label.Location = new System.Drawing.Point(0, 0);
            this.cmp_cc_label.Margin = new System.Windows.Forms.Padding(3);
            this.cmp_cc_label.MinimumSize = new System.Drawing.Size(45, 0);
            this.cmp_cc_label.Name = "cmp_cc_label";
            this.cmp_cc_label.Size = new System.Drawing.Size(45, 23);
            this.cmp_cc_label.TabIndex = 0;
            this.cmp_cc_label.Text = "cc: ";
            // 
            // cmp_info_padding_panel1
            // 
            this.cmp_info_padding_panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_info_padding_panel1.Location = new System.Drawing.Point(0, 31);
            this.cmp_info_padding_panel1.Name = "cmp_info_padding_panel1";
            this.cmp_info_padding_panel1.Size = new System.Drawing.Size(536, 5);
            this.cmp_info_padding_panel1.TabIndex = 3;
            // 
            // cmp_to_panel
            // 
            this.cmp_to_panel.AutoSize = true;
            this.cmp_to_panel.Controls.Add(this.cmp_to_underline_panel);
            this.cmp_to_panel.Controls.Add(this.cmp_to_textbox);
            this.cmp_to_panel.Controls.Add(this.cmp_to_left_panel);
            this.cmp_to_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_to_panel.Location = new System.Drawing.Point(0, 0);
            this.cmp_to_panel.Name = "cmp_to_panel";
            this.cmp_to_panel.Size = new System.Drawing.Size(536, 31);
            this.cmp_to_panel.TabIndex = 1;
            // 
            // cmp_to_underline_panel
            // 
            this.cmp_to_underline_panel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cmp_to_underline_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_to_underline_panel.Location = new System.Drawing.Point(45, 30);
            this.cmp_to_underline_panel.Name = "cmp_to_underline_panel";
            this.cmp_to_underline_panel.Size = new System.Drawing.Size(491, 1);
            this.cmp_to_underline_panel.TabIndex = 3;
            // 
            // cmp_to_textbox
            // 
            this.cmp_to_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cmp_to_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmp_to_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_to_textbox.Location = new System.Drawing.Point(45, 0);
            this.cmp_to_textbox.MinimumSize = new System.Drawing.Size(0, 30);
            this.cmp_to_textbox.Name = "cmp_to_textbox";
            this.cmp_to_textbox.Size = new System.Drawing.Size(491, 30);
            this.cmp_to_textbox.TabIndex = 4;
            // 
            // cmp_to_left_panel
            // 
            this.cmp_to_left_panel.AutoSize = true;
            this.cmp_to_left_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.cmp_to_left_panel.Controls.Add(this.cmp_to_label);
            this.cmp_to_left_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmp_to_left_panel.Location = new System.Drawing.Point(0, 0);
            this.cmp_to_left_panel.Name = "cmp_to_left_panel";
            this.cmp_to_left_panel.Size = new System.Drawing.Size(45, 31);
            this.cmp_to_left_panel.TabIndex = 2;
            // 
            // cmp_to_label
            // 
            this.cmp_to_label.AutoSize = true;
            this.cmp_to_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmp_to_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmp_to_label.Location = new System.Drawing.Point(0, 0);
            this.cmp_to_label.Margin = new System.Windows.Forms.Padding(3);
            this.cmp_to_label.MinimumSize = new System.Drawing.Size(45, 0);
            this.cmp_to_label.Name = "cmp_to_label";
            this.cmp_to_label.Size = new System.Drawing.Size(45, 23);
            this.cmp_to_label.TabIndex = 0;
            this.cmp_to_label.Text = "To: ";
            // 
            // message_flow_panel
            // 
            this.message_flow_panel.AutoScroll = true;
            this.message_flow_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.message_flow_panel.Location = new System.Drawing.Point(0, 23);
            this.message_flow_panel.Name = "message_flow_panel";
            this.message_flow_panel.Size = new System.Drawing.Size(350, 592);
            this.message_flow_panel.TabIndex = 2;
            // 
            // search_folder_textbox
            // 
            this.search_folder_textbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.search_folder_textbox.Dock = System.Windows.Forms.DockStyle.Top;
            this.search_folder_textbox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.search_folder_textbox.Location = new System.Drawing.Point(0, 0);
            this.search_folder_textbox.Name = "search_folder_textbox";
            this.search_folder_textbox.PlaceholderText = " Search...";
            this.search_folder_textbox.Size = new System.Drawing.Size(350, 23);
            this.search_folder_textbox.TabIndex = 4;
            this.search_folder_textbox.TabStop = false;
            // 
            // inbox_panel
            // 
            this.inbox_panel.AutoScroll = true;
            this.inbox_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inbox_panel.Controls.Add(this.message_flow_panel);
            this.inbox_panel.Controls.Add(this.search_folder_textbox);
            this.inbox_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.inbox_panel.Location = new System.Drawing.Point(200, 30);
            this.inbox_panel.Name = "inbox_panel";
            this.inbox_panel.Size = new System.Drawing.Size(352, 617);
            this.inbox_panel.TabIndex = 2;
            // 
            // brief_control_explain_tooltop
            // 
            this.brief_control_explain_tooltop.AutomaticDelay = 50;
            this.brief_control_explain_tooltop.AutoPopDelay = 5000;
            this.brief_control_explain_tooltop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.brief_control_explain_tooltop.InitialDelay = 50;
            this.brief_control_explain_tooltop.ReshowDelay = 10;
            // 
            // cmp_add_contextstrip
            // 
            this.cmp_add_contextstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmp_add_contextstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add_cc_menuitem,
            this.add_bcc_menuitem});
            this.cmp_add_contextstrip.Name = "cmp_add_contextstrip";
            this.cmp_add_contextstrip.Size = new System.Drawing.Size(142, 60);
            // 
            // add_cc_menuitem
            // 
            this.add_cc_menuitem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.add_cc_menuitem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.add_cc_menuitem.Name = "add_cc_menuitem";
            this.add_cc_menuitem.Size = new System.Drawing.Size(141, 28);
            this.add_cc_menuitem.Text = "Add cc";
            this.add_cc_menuitem.Click += new System.EventHandler(this.add_cc_menuitem_Click);
            // 
            // add_bcc_menuitem
            // 
            this.add_bcc_menuitem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.add_bcc_menuitem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.add_bcc_menuitem.Name = "add_bcc_menuitem";
            this.add_bcc_menuitem.Size = new System.Drawing.Size(141, 28);
            this.add_bcc_menuitem.Text = "Add Bcc";
            this.add_bcc_menuitem.Click += new System.EventHandler(this.add_bcc_menuitem_Click);
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
            this.intro_tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.welcome_picture_box)).EndInit();
            this.open_message_tab.ResumeLayout(false);
            this.open_message_tab.PerformLayout();
            this.msg_mailbody_panel.ResumeLayout(false);
            this.msg_buttons_panel.ResumeLayout(false);
            this.msg_senderinfo_panel.ResumeLayout(false);
            this.msg_senderinfo_panel.PerformLayout();
            this.compose_message_tab.ResumeLayout(false);
            this.compose_message_tab.PerformLayout();
            this.cmp_body_panel.ResumeLayout(false);
            this.cmp_bottom_panel.ResumeLayout(false);
            this.cmp_info_panel.ResumeLayout(false);
            this.cmp_info_panel.PerformLayout();
            this.cmp_subject_panel.ResumeLayout(false);
            this.cmp_subject_panel.PerformLayout();
            this.cmp_subject_left_panel.ResumeLayout(false);
            this.cmp_subject_left_panel.PerformLayout();
            this.cmp_bcc_panel.ResumeLayout(false);
            this.cmp_bcc_panel.PerformLayout();
            this.cmp_bcc_left_panel.ResumeLayout(false);
            this.cmp_bcc_left_panel.PerformLayout();
            this.cmp_cc_panel.ResumeLayout(false);
            this.cmp_cc_panel.PerformLayout();
            this.cmp_cc_left_panel.ResumeLayout(false);
            this.cmp_cc_left_panel.PerformLayout();
            this.cmp_to_panel.ResumeLayout(false);
            this.cmp_to_panel.PerformLayout();
            this.cmp_to_left_panel.ResumeLayout(false);
            this.cmp_to_left_panel.PerformLayout();
            this.inbox_panel.ResumeLayout(false);
            this.inbox_panel.PerformLayout();
            this.cmp_add_contextstrip.ResumeLayout(false);
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
        private ToolStripMenuItem user_settings_button;
        private ToolStripMenuItem logout_button;
        private ToolStripMenuItem select_user_button;
        private ToolStripMenuItem add_user_button;
        private ToolStripButton refresh_button;
        private ToolStripButton markspam_panel;
        private ListBox folders_lisbox;
        private Panel account_info_panel;
        private Label active_email_label;
        private TextBox search_folder_textbox;
        private Panel inbox_panel;
        private TabPage intro_tab;
        private PictureBox welcome_picture_box;
        private EnvelopeFlowPanel message_flow_panel;
        private Panel msg_senderinfo_panel;
        private Label msg_from_label;
        private Label msg_subject_label;
        private Panel msg_mailbody_panel;
        private RichTextBox msg_body_rtextbox;
        private Panel msg_padding_panel2;
        private Panel msg_padding_panel1;
        private Panel msg_buttons_panel;
        private Panel msg_senderinfo_padding_panel;
        private Button msg_reply_button;
        private Button msg_forward_button;
        private Button msg_replyall_button;
        private ToolTip brief_control_explain_tooltop;
        private TabPage compose_message_tab;
        private Panel cmp_body_panel;
        private RichTextBox cmp_mailbody_rtextbox;
        private Panel cmp_padding_panel2;
        private Panel cmp_padding_panel1;
        private Panel cmp_bottom_panel;
        private Button cmp_send_button;
        private Panel cmp_info_panel;
        private Panel cmp_to_panel;
        private TextBox cmp_to_textbox;
        private Panel cmp_to_underline_panel;
        private Panel cmp_to_left_panel;
        private Label cmp_to_label;
        private Panel cmp_cc_panel;
        private Panel cmp_cc_underscore_panel;
        private TextBox cmp_cc_textbox;
        private Panel cmp_cc_left_panel;
        private Label cmp_cc_label;
        private Panel cmp_bcc_panel;
        private Panel cmp_bcc_underscore_panel;
        private TextBox cmp_bcc_textbox;
        private Panel cmp_bcc_left_panel;
        private Label cmp_bcc_label;
        private Panel cmp_info_padding_panel2;
        private Panel cmp_info_padding_panel1;
        private Panel cmp_subject_panel;
        private Panel cmp_subject_underscore_panel;
        private TextBox cmp_subject_textbox;
        private Panel cmp_subject_left_panel;
        private Label cmp_subject_label;
        private Panel cmp_info_padding_panel3;
        private Button cmp_add_button;
        private ContextMenuStrip cmp_add_contextstrip;
        private ToolStripMenuItem add_cc_menuitem;
        private ToolStripMenuItem add_bcc_menuitem;
    }
}