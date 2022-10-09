namespace G5EmailClient.GUI
{
    partial class ConnectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.server_connect_panel = new System.Windows.Forms.Panel();
            this.SMTP_port_box = new System.Windows.Forms.NumericUpDown();
            this.SMTP_port_label = new System.Windows.Forms.Label();
            this.IMAP_port_box = new System.Windows.Forms.NumericUpDown();
            this.IMAP_port_lable = new System.Windows.Forms.Label();
            this.SMTP_hostname_label = new System.Windows.Forms.Label();
            this.SMTP_hostname_textbox = new System.Windows.Forms.TextBox();
            this.IMAP_hostname_textbox = new System.Windows.Forms.TextBox();
            this.IMAP_hostname_label = new System.Windows.Forms.Label();
            this.connect_button = new System.Windows.Forms.Button();
            this.big_logo_box = new System.Windows.Forms.PictureBox();
            this.close_button = new System.Windows.Forms.Button();
            this.message_label = new System.Windows.Forms.Label();
            this.login_button = new System.Windows.Forms.Button();
            this.login_panel = new System.Windows.Forms.Panel();
            this.password_label = new System.Windows.Forms.Label();
            this.password_textbox = new System.Windows.Forms.TextBox();
            this.username_textbox = new System.Windows.Forms.TextBox();
            this.username_label = new System.Windows.Forms.Label();
            this.message_label_tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.new_host_button = new System.Windows.Forms.Button();
            this.users_button = new System.Windows.Forms.Button();
            this.saved_users_panel = new System.Windows.Forms.Panel();
            this.saved_uers_label = new System.Windows.Forms.Label();
            this.saved_users_list = new System.Windows.Forms.ListBox();
            this.delete_user_button = new System.Windows.Forms.Button();
            this.load_user_button = new System.Windows.Forms.Button();
            this.save_user_button = new System.Windows.Forms.Button();
            this.save_button_tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.server_connect_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SMTP_port_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IMAP_port_box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.big_logo_box)).BeginInit();
            this.login_panel.SuspendLayout();
            this.saved_users_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // server_connect_panel
            // 
            this.server_connect_panel.Controls.Add(this.SMTP_port_box);
            this.server_connect_panel.Controls.Add(this.SMTP_port_label);
            this.server_connect_panel.Controls.Add(this.IMAP_port_box);
            this.server_connect_panel.Controls.Add(this.IMAP_port_lable);
            this.server_connect_panel.Controls.Add(this.SMTP_hostname_label);
            this.server_connect_panel.Controls.Add(this.SMTP_hostname_textbox);
            this.server_connect_panel.Controls.Add(this.IMAP_hostname_textbox);
            this.server_connect_panel.Controls.Add(this.IMAP_hostname_label);
            this.server_connect_panel.Location = new System.Drawing.Point(150, 300);
            this.server_connect_panel.Name = "server_connect_panel";
            this.server_connect_panel.Size = new System.Drawing.Size(500, 150);
            this.server_connect_panel.TabIndex = 0;
            // 
            // SMTP_port_box
            // 
            this.SMTP_port_box.Location = new System.Drawing.Point(375, 110);
            this.SMTP_port_box.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.SMTP_port_box.Name = "SMTP_port_box";
            this.SMTP_port_box.Size = new System.Drawing.Size(115, 27);
            this.SMTP_port_box.TabIndex = 4;
            this.SMTP_port_box.ValueChanged += new System.EventHandler(this.SMTP_port_box_ValueChanged);
            // 
            // SMTP_port_label
            // 
            this.SMTP_port_label.AutoSize = true;
            this.SMTP_port_label.Location = new System.Drawing.Point(375, 80);
            this.SMTP_port_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.SMTP_port_label.MinimumSize = new System.Drawing.Size(115, 0);
            this.SMTP_port_label.Name = "SMTP_port_label";
            this.SMTP_port_label.Size = new System.Drawing.Size(115, 20);
            this.SMTP_port_label.TabIndex = 6;
            this.SMTP_port_label.Text = "SMTP port";
            this.SMTP_port_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IMAP_port_box
            // 
            this.IMAP_port_box.Location = new System.Drawing.Point(10, 110);
            this.IMAP_port_box.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.IMAP_port_box.Name = "IMAP_port_box";
            this.IMAP_port_box.Size = new System.Drawing.Size(115, 27);
            this.IMAP_port_box.TabIndex = 2;
            this.IMAP_port_box.ValueChanged += new System.EventHandler(this.IMAP_port_box_ValueChanged);
            // 
            // IMAP_port_lable
            // 
            this.IMAP_port_lable.AutoSize = true;
            this.IMAP_port_lable.Location = new System.Drawing.Point(10, 80);
            this.IMAP_port_lable.MaximumSize = new System.Drawing.Size(0, 20);
            this.IMAP_port_lable.MinimumSize = new System.Drawing.Size(115, 0);
            this.IMAP_port_lable.Name = "IMAP_port_lable";
            this.IMAP_port_lable.Size = new System.Drawing.Size(115, 20);
            this.IMAP_port_lable.TabIndex = 4;
            this.IMAP_port_lable.Text = "IMAP port";
            this.IMAP_port_lable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SMTP_hostname_label
            // 
            this.SMTP_hostname_label.AutoSize = true;
            this.SMTP_hostname_label.Location = new System.Drawing.Point(260, 10);
            this.SMTP_hostname_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.SMTP_hostname_label.MinimumSize = new System.Drawing.Size(230, 0);
            this.SMTP_hostname_label.Name = "SMTP_hostname_label";
            this.SMTP_hostname_label.Size = new System.Drawing.Size(230, 20);
            this.SMTP_hostname_label.TabIndex = 3;
            this.SMTP_hostname_label.Text = "SMTP Hostname";
            this.SMTP_hostname_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SMTP_hostname_textbox
            // 
            this.SMTP_hostname_textbox.Location = new System.Drawing.Point(260, 40);
            this.SMTP_hostname_textbox.Name = "SMTP_hostname_textbox";
            this.SMTP_hostname_textbox.Size = new System.Drawing.Size(230, 27);
            this.SMTP_hostname_textbox.TabIndex = 3;
            this.SMTP_hostname_textbox.TextChanged += new System.EventHandler(this.SMTP_hostname_textbox_TextChanged);
            // 
            // IMAP_hostname_textbox
            // 
            this.IMAP_hostname_textbox.Location = new System.Drawing.Point(10, 40);
            this.IMAP_hostname_textbox.Name = "IMAP_hostname_textbox";
            this.IMAP_hostname_textbox.Size = new System.Drawing.Size(230, 27);
            this.IMAP_hostname_textbox.TabIndex = 1;
            this.IMAP_hostname_textbox.TextChanged += new System.EventHandler(this.IMAP_hostname_textbox_TextChanged);
            // 
            // IMAP_hostname_label
            // 
            this.IMAP_hostname_label.AutoSize = true;
            this.IMAP_hostname_label.Location = new System.Drawing.Point(10, 10);
            this.IMAP_hostname_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.IMAP_hostname_label.MinimumSize = new System.Drawing.Size(230, 0);
            this.IMAP_hostname_label.Name = "IMAP_hostname_label";
            this.IMAP_hostname_label.Size = new System.Drawing.Size(230, 20);
            this.IMAP_hostname_label.TabIndex = 0;
            this.IMAP_hostname_label.Text = "IMAP Hostname";
            this.IMAP_hostname_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // connect_button
            // 
            this.connect_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connect_button.Location = new System.Drawing.Point(696, 459);
            this.connect_button.Name = "connect_button";
            this.connect_button.Size = new System.Drawing.Size(94, 30);
            this.connect_button.TabIndex = 5;
            this.connect_button.Text = "Connect";
            this.connect_button.UseVisualStyleBackColor = true;
            this.connect_button.Click += new System.EventHandler(this.connect_button_Click);
            // 
            // big_logo_box
            // 
            this.big_logo_box.Image = global::G5EmailClient.Properties.Resources.G5Logo;
            this.big_logo_box.Location = new System.Drawing.Point(225, 40);
            this.big_logo_box.Name = "big_logo_box";
            this.big_logo_box.Size = new System.Drawing.Size(350, 225);
            this.big_logo_box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.big_logo_box.TabIndex = 1;
            this.big_logo_box.TabStop = false;
            // 
            // close_button
            // 
            this.close_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.close_button.Location = new System.Drawing.Point(12, 459);
            this.close_button.Name = "close_button";
            this.close_button.Size = new System.Drawing.Size(94, 29);
            this.close_button.TabIndex = 10;
            this.close_button.Text = "Close";
            this.close_button.UseVisualStyleBackColor = true;
            this.close_button.Click += new System.EventHandler(this.close_button_Click);
            // 
            // message_label
            // 
            this.message_label.AutoSize = true;
            this.message_label.Location = new System.Drawing.Point(160, 464);
            this.message_label.MinimumSize = new System.Drawing.Size(475, 20);
            this.message_label.Name = "message_label";
            this.message_label.Size = new System.Drawing.Size(475, 20);
            this.message_label.TabIndex = 9;
            this.message_label.Text = "Message label";
            this.message_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.message_label.Visible = false;
            this.message_label.Click += new System.EventHandler(this.message_label_Click);
            // 
            // login_button
            // 
            this.login_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.login_button.Location = new System.Drawing.Point(696, 459);
            this.login_button.Name = "login_button";
            this.login_button.Size = new System.Drawing.Size(94, 30);
            this.login_button.TabIndex = 8;
            this.login_button.Text = "Log In";
            this.login_button.UseVisualStyleBackColor = true;
            this.login_button.Visible = false;
            this.login_button.Click += new System.EventHandler(this.login_button_Click);
            // 
            // login_panel
            // 
            this.login_panel.Controls.Add(this.password_label);
            this.login_panel.Controls.Add(this.password_textbox);
            this.login_panel.Controls.Add(this.username_textbox);
            this.login_panel.Controls.Add(this.username_label);
            this.login_panel.Location = new System.Drawing.Point(150, 300);
            this.login_panel.Name = "login_panel";
            this.login_panel.Size = new System.Drawing.Size(500, 150);
            this.login_panel.TabIndex = 8;
            this.login_panel.Visible = false;
            // 
            // password_label
            // 
            this.password_label.AutoSize = true;
            this.password_label.Location = new System.Drawing.Point(10, 80);
            this.password_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.password_label.MinimumSize = new System.Drawing.Size(480, 0);
            this.password_label.Name = "password_label";
            this.password_label.Size = new System.Drawing.Size(480, 20);
            this.password_label.TabIndex = 3;
            this.password_label.Text = "Password";
            this.password_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // password_textbox
            // 
            this.password_textbox.Location = new System.Drawing.Point(10, 110);
            this.password_textbox.Name = "password_textbox";
            this.password_textbox.Size = new System.Drawing.Size(480, 27);
            this.password_textbox.TabIndex = 7;
            this.password_textbox.UseSystemPasswordChar = true;
            this.password_textbox.TextChanged += new System.EventHandler(this.password_textbox_TextChanged);
            // 
            // username_textbox
            // 
            this.username_textbox.Location = new System.Drawing.Point(10, 40);
            this.username_textbox.Name = "username_textbox";
            this.username_textbox.Size = new System.Drawing.Size(480, 27);
            this.username_textbox.TabIndex = 6;
            this.username_textbox.TextChanged += new System.EventHandler(this.username_textbox_TextChanged);
            // 
            // username_label
            // 
            this.username_label.AutoSize = true;
            this.username_label.Location = new System.Drawing.Point(10, 10);
            this.username_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.username_label.MinimumSize = new System.Drawing.Size(480, 0);
            this.username_label.Name = "username_label";
            this.username_label.Size = new System.Drawing.Size(480, 20);
            this.username_label.TabIndex = 0;
            this.username_label.Text = "Username";
            this.username_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // message_label_tooltip
            // 
            this.message_label_tooltip.AutoPopDelay = 5000000;
            this.message_label_tooltip.InitialDelay = 500;
            this.message_label_tooltip.ReshowDelay = 100;
            this.message_label_tooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.message_label_tooltip.ToolTipTitle = "Error message. Click to copy to clipboard.";
            // 
            // new_host_button
            // 
            this.new_host_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_host_button.Location = new System.Drawing.Point(12, 421);
            this.new_host_button.Name = "new_host_button";
            this.new_host_button.Size = new System.Drawing.Size(94, 29);
            this.new_host_button.TabIndex = 8;
            this.new_host_button.Text = "New Host";
            this.new_host_button.UseVisualStyleBackColor = true;
            this.new_host_button.Visible = false;
            this.new_host_button.Click += new System.EventHandler(this.new_host_button_Click);
            // 
            // users_button
            // 
            this.users_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.users_button.Location = new System.Drawing.Point(696, 421);
            this.users_button.Name = "users_button";
            this.users_button.Size = new System.Drawing.Size(94, 29);
            this.users_button.TabIndex = 9;
            this.users_button.Text = "Users";
            this.users_button.UseVisualStyleBackColor = true;
            this.users_button.Click += new System.EventHandler(this.users_button_Click);
            // 
            // saved_users_panel
            // 
            this.saved_users_panel.Controls.Add(this.saved_uers_label);
            this.saved_users_panel.Controls.Add(this.saved_users_list);
            this.saved_users_panel.Controls.Add(this.delete_user_button);
            this.saved_users_panel.Controls.Add(this.load_user_button);
            this.saved_users_panel.Controls.Add(this.save_user_button);
            this.saved_users_panel.Location = new System.Drawing.Point(150, 40);
            this.saved_users_panel.Name = "saved_users_panel";
            this.saved_users_panel.Size = new System.Drawing.Size(500, 250);
            this.saved_users_panel.TabIndex = 13;
            this.saved_users_panel.Visible = false;
            // 
            // saved_uers_label
            // 
            this.saved_uers_label.AutoSize = true;
            this.saved_uers_label.Location = new System.Drawing.Point(10, 27);
            this.saved_uers_label.MaximumSize = new System.Drawing.Size(0, 20);
            this.saved_uers_label.MinimumSize = new System.Drawing.Size(480, 0);
            this.saved_uers_label.Name = "saved_uers_label";
            this.saved_uers_label.Size = new System.Drawing.Size(480, 20);
            this.saved_uers_label.TabIndex = 4;
            this.saved_uers_label.Text = "Saved Users";
            this.saved_uers_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saved_users_list
            // 
            this.saved_users_list.FormattingEnabled = true;
            this.saved_users_list.ItemHeight = 20;
            this.saved_users_list.Location = new System.Drawing.Point(10, 57);
            this.saved_users_list.Name = "saved_users_list";
            this.saved_users_list.Size = new System.Drawing.Size(480, 144);
            this.saved_users_list.TabIndex = 3;
            // 
            // delete_user_button
            // 
            this.delete_user_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete_user_button.Location = new System.Drawing.Point(10, 211);
            this.delete_user_button.Name = "delete_user_button";
            this.delete_user_button.Size = new System.Drawing.Size(94, 29);
            this.delete_user_button.TabIndex = 11;
            this.delete_user_button.Text = "Delete";
            this.delete_user_button.UseVisualStyleBackColor = true;
            this.delete_user_button.Click += new System.EventHandler(this.delete_user_button_Click);
            // 
            // load_user_button
            // 
            this.load_user_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.load_user_button.Location = new System.Drawing.Point(396, 211);
            this.load_user_button.Name = "load_user_button";
            this.load_user_button.Size = new System.Drawing.Size(94, 29);
            this.load_user_button.TabIndex = 13;
            this.load_user_button.Text = "Load";
            this.load_user_button.UseVisualStyleBackColor = true;
            this.load_user_button.Click += new System.EventHandler(this.load_user_button_Click);
            // 
            // save_user_button
            // 
            this.save_user_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_user_button.Location = new System.Drawing.Point(114, 211);
            this.save_user_button.Name = "save_user_button";
            this.save_user_button.Size = new System.Drawing.Size(94, 29);
            this.save_user_button.TabIndex = 12;
            this.save_user_button.Text = "Save";
            this.save_user_button.UseVisualStyleBackColor = true;
            this.save_user_button.Visible = false;
            this.save_user_button.Click += new System.EventHandler(this.save_user_button_Click);
            // 
            // save_button_tooltip
            // 
            this.save_button_tooltip.AutomaticDelay = 50;
            this.save_button_tooltip.AutoPopDelay = 50000;
            this.save_button_tooltip.BackColor = System.Drawing.SystemColors.HighlightText;
            this.save_button_tooltip.InitialDelay = 50;
            this.save_button_tooltip.ReshowDelay = 10;
            this.save_button_tooltip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.save_button_tooltip.ToolTipTitle = "User saving";
            // 
            // ConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.users_button);
            this.Controls.Add(this.new_host_button);
            this.Controls.Add(this.message_label);
            this.Controls.Add(this.close_button);
            this.Controls.Add(this.saved_users_panel);
            this.Controls.Add(this.big_logo_box);
            this.Controls.Add(this.login_panel);
            this.Controls.Add(this.login_button);
            this.Controls.Add(this.server_connect_panel);
            this.Controls.Add(this.connect_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ConnectionForm";
            this.Text = "ConnectionForm";
            this.server_connect_panel.ResumeLayout(false);
            this.server_connect_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SMTP_port_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IMAP_port_box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.big_logo_box)).EndInit();
            this.login_panel.ResumeLayout(false);
            this.login_panel.PerformLayout();
            this.saved_users_panel.ResumeLayout(false);
            this.saved_users_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel server_connect_panel;
        private PictureBox big_logo_box;
        private Button close_button;
        private Label IMAP_hostname_label;
        private Label SMTP_hostname_label;
        private TextBox SMTP_hostname_textbox;
        private TextBox IMAP_hostname_textbox;
        private Label SMTP_port_label;
        private NumericUpDown IMAP_port_box;
        private Label IMAP_port_lable;
        private NumericUpDown SMTP_port_box;
        private Button connect_button;
        private Label message_label;
        private Button login_button;
        private Panel login_panel;
        private Label password_label;
        private TextBox password_textbox;
        private TextBox username_textbox;
        private Label username_label;
        private ToolTip message_label_tooltip;
        private Button new_host_button;
        private Button users_button;
        private Panel saved_users_panel;
        private Label saved_uers_label;
        private ListBox saved_users_list;
        private Button delete_user_button;
        private Button load_user_button;
        private Button save_user_button;
        private ToolTip save_button_tooltip;
    }
}