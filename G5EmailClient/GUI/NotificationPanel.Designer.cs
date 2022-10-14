namespace G5EmailClient.GUI
{
    partial class NotificationPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.not_icon = new System.Windows.Forms.PictureBox();
            this.not_title_label = new System.Windows.Forms.Label();
            this.title_panel = new System.Windows.Forms.Panel();
            this.not_button_panel = new System.Windows.Forms.Panel();
            this.collapse_button = new System.Windows.Forms.Button();
            this.not_left_button = new System.Windows.Forms.Button();
            this.not_text_panel = new System.Windows.Forms.Panel();
            this.not_text_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.not_icon)).BeginInit();
            this.title_panel.SuspendLayout();
            this.not_button_panel.SuspendLayout();
            this.not_text_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // not_icon
            // 
            this.not_icon.BackgroundImage = global::G5EmailClient.Properties.Resources.SendIcon;
            this.not_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.not_icon.Dock = System.Windows.Forms.DockStyle.Left;
            this.not_icon.Location = new System.Drawing.Point(3, 3);
            this.not_icon.MaximumSize = new System.Drawing.Size(32, 32);
            this.not_icon.MinimumSize = new System.Drawing.Size(32, 32);
            this.not_icon.Name = "not_icon";
            this.not_icon.Size = new System.Drawing.Size(32, 32);
            this.not_icon.TabIndex = 0;
            this.not_icon.TabStop = false;
            this.not_icon.Click += new System.EventHandler(this.title_panel_Click);
            this.not_icon.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_icon.MouseHover += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // not_title_label
            // 
            this.not_title_label.AutoEllipsis = true;
            this.not_title_label.AutoSize = true;
            this.not_title_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.not_title_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.not_title_label.Location = new System.Drawing.Point(35, 3);
            this.not_title_label.MaximumSize = new System.Drawing.Size(149, 0);
            this.not_title_label.MinimumSize = new System.Drawing.Size(149, 32);
            this.not_title_label.Name = "not_title_label";
            this.not_title_label.Size = new System.Drawing.Size(149, 32);
            this.not_title_label.TabIndex = 1;
            this.not_title_label.Text = "<title here>";
            this.not_title_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.not_title_label.Click += new System.EventHandler(this.title_panel_Click);
            this.not_title_label.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_title_label.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // title_panel
            // 
            this.title_panel.AutoSize = true;
            this.title_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.title_panel.BackColor = System.Drawing.Color.Transparent;
            this.title_panel.Controls.Add(this.not_title_label);
            this.title_panel.Controls.Add(this.not_icon);
            this.title_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.title_panel.Location = new System.Drawing.Point(0, 0);
            this.title_panel.MinimumSize = new System.Drawing.Size(120, 0);
            this.title_panel.Name = "title_panel";
            this.title_panel.Padding = new System.Windows.Forms.Padding(3);
            this.title_panel.Size = new System.Drawing.Size(190, 38);
            this.title_panel.TabIndex = 2;
            this.title_panel.Click += new System.EventHandler(this.title_panel_Click);
            this.title_panel.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.title_panel.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // not_button_panel
            // 
            this.not_button_panel.BackColor = System.Drawing.Color.Transparent;
            this.not_button_panel.Controls.Add(this.collapse_button);
            this.not_button_panel.Controls.Add(this.not_left_button);
            this.not_button_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.not_button_panel.Location = new System.Drawing.Point(0, 68);
            this.not_button_panel.MinimumSize = new System.Drawing.Size(0, 30);
            this.not_button_panel.Name = "not_button_panel";
            this.not_button_panel.Size = new System.Drawing.Size(190, 30);
            this.not_button_panel.TabIndex = 3;
            this.not_button_panel.Visible = false;
            this.not_button_panel.Click += new System.EventHandler(this.not_text_panel_Click);
            this.not_button_panel.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_button_panel.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // collapse_button
            // 
            this.collapse_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.collapse_button.BackgroundImage = global::G5EmailClient.Properties.Resources.UpArrowIcon;
            this.collapse_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.collapse_button.Dock = System.Windows.Forms.DockStyle.Right;
            this.collapse_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.collapse_button.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.collapse_button.Location = new System.Drawing.Point(160, 0);
            this.collapse_button.MaximumSize = new System.Drawing.Size(30, 30);
            this.collapse_button.MinimumSize = new System.Drawing.Size(30, 30);
            this.collapse_button.Name = "collapse_button";
            this.collapse_button.Size = new System.Drawing.Size(30, 30);
            this.collapse_button.TabIndex = 2;
            this.collapse_button.UseVisualStyleBackColor = true;
            this.collapse_button.Click += new System.EventHandler(this.collapse_button_Click);
            this.collapse_button.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.collapse_button.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // not_left_button
            // 
            this.not_left_button.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.not_left_button.BackgroundImage = global::G5EmailClient.Properties.Resources.CloseIcon;
            this.not_left_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.not_left_button.Dock = System.Windows.Forms.DockStyle.Left;
            this.not_left_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.not_left_button.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.not_left_button.Location = new System.Drawing.Point(0, 0);
            this.not_left_button.MaximumSize = new System.Drawing.Size(30, 30);
            this.not_left_button.MinimumSize = new System.Drawing.Size(30, 30);
            this.not_left_button.Name = "not_left_button";
            this.not_left_button.Size = new System.Drawing.Size(30, 30);
            this.not_left_button.TabIndex = 1;
            this.not_left_button.UseVisualStyleBackColor = true;
            this.not_left_button.Click += new System.EventHandler(this.not_left_button_Click);
            this.not_left_button.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_left_button.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // not_text_panel
            // 
            this.not_text_panel.AutoScroll = true;
            this.not_text_panel.AutoSize = true;
            this.not_text_panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.not_text_panel.BackColor = System.Drawing.Color.Transparent;
            this.not_text_panel.Controls.Add(this.not_text_label);
            this.not_text_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.not_text_panel.Location = new System.Drawing.Point(0, 38);
            this.not_text_panel.MinimumSize = new System.Drawing.Size(0, 20);
            this.not_text_panel.Name = "not_text_panel";
            this.not_text_panel.Padding = new System.Windows.Forms.Padding(5);
            this.not_text_panel.Size = new System.Drawing.Size(190, 30);
            this.not_text_panel.TabIndex = 4;
            this.not_text_panel.Visible = false;
            this.not_text_panel.Click += new System.EventHandler(this.not_text_panel_Click);
            this.not_text_panel.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_text_panel.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // not_text_label
            // 
            this.not_text_label.AutoSize = true;
            this.not_text_label.Dock = System.Windows.Forms.DockStyle.Fill;
            this.not_text_label.Location = new System.Drawing.Point(5, 5);
            this.not_text_label.MaximumSize = new System.Drawing.Size(180, 0);
            this.not_text_label.Name = "not_text_label";
            this.not_text_label.Size = new System.Drawing.Size(93, 20);
            this.not_text_label.TabIndex = 0;
            this.not_text_label.Text = "<text_label>";
            this.not_text_label.Click += new System.EventHandler(this.not_text_panel_Click);
            this.not_text_label.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.not_text_label.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            // 
            // NotificationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.not_text_panel);
            this.Controls.Add(this.not_button_panel);
            this.Controls.Add(this.title_panel);
            this.MinimumSize = new System.Drawing.Size(192, 0);
            this.Name = "NotificationPanel";
            this.Size = new System.Drawing.Size(190, 98);
            this.SizeChanged += new System.EventHandler(this.NotificationPanel_SizeChanged);
            this.MouseEnter += new System.EventHandler(this.NotificationPanel_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.NotificationPanel_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.not_icon)).EndInit();
            this.title_panel.ResumeLayout(false);
            this.title_panel.PerformLayout();
            this.not_button_panel.ResumeLayout(false);
            this.not_text_panel.ResumeLayout(false);
            this.not_text_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox not_icon;
        private Label not_title_label;
        private Panel title_panel;
        private Panel not_button_panel;
        private Button not_left_button;
        private Panel not_text_panel;
        private Label not_text_label;
        private Button collapse_button;
    }
}
