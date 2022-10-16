namespace G5EmailClient.GUI
{
    partial class EnvelopePanel
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
            this.env_edge_colorbar_panel = new System.Windows.Forms.Panel();
            this.env_from_label = new System.Windows.Forms.Label();
            this.env_subject_label = new System.Windows.Forms.Label();
            this.env_date_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // env_edge_colorbar_panel
            // 
            this.env_edge_colorbar_panel.BackColor = System.Drawing.Color.LightGray;
            this.env_edge_colorbar_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.env_edge_colorbar_panel.Location = new System.Drawing.Point(0, 0);
            this.env_edge_colorbar_panel.Name = "env_edge_colorbar_panel";
            this.env_edge_colorbar_panel.Size = new System.Drawing.Size(5, 73);
            this.env_edge_colorbar_panel.TabIndex = 5;
            // 
            // env_from_label
            // 
            this.env_from_label.AutoEllipsis = true;
            this.env_from_label.AutoSize = true;
            this.env_from_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.env_from_label.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.env_from_label.Location = new System.Drawing.Point(5, 0);
            this.env_from_label.Margin = new System.Windows.Forms.Padding(0);
            this.env_from_label.Name = "env_from_label";
            this.env_from_label.Padding = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.env_from_label.Size = new System.Drawing.Size(130, 31);
            this.env_from_label.TabIndex = 6;
            this.env_from_label.Text = "<from_string>";
            this.env_from_label.Click += new System.EventHandler(this.EnvelopePanel_Click);
            this.env_from_label.MouseEnter += new System.EventHandler(this.EnvelopePanel_MouseEnter);
            this.env_from_label.MouseLeave += new System.EventHandler(this.EnvelopePanel_MouseLeave);
            // 
            // env_subject_label
            // 
            this.env_subject_label.AutoEllipsis = true;
            this.env_subject_label.AutoSize = true;
            this.env_subject_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.env_subject_label.Location = new System.Drawing.Point(5, 46);
            this.env_subject_label.Name = "env_subject_label";
            this.env_subject_label.Padding = new System.Windows.Forms.Padding(5, 2, 5, 5);
            this.env_subject_label.Size = new System.Drawing.Size(129, 27);
            this.env_subject_label.TabIndex = 7;
            this.env_subject_label.Text = "<subject_string>";
            this.env_subject_label.Click += new System.EventHandler(this.EnvelopePanel_Click);
            this.env_subject_label.MouseEnter += new System.EventHandler(this.EnvelopePanel_MouseEnter);
            this.env_subject_label.MouseLeave += new System.EventHandler(this.EnvelopePanel_MouseLeave);
            // 
            // env_date_label
            // 
            this.env_date_label.AutoEllipsis = true;
            this.env_date_label.AutoSize = true;
            this.env_date_label.Dock = System.Windows.Forms.DockStyle.Top;
            this.env_date_label.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.env_date_label.Location = new System.Drawing.Point(5, 31);
            this.env_date_label.Margin = new System.Windows.Forms.Padding(0);
            this.env_date_label.Name = "env_date_label";
            this.env_date_label.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.env_date_label.Size = new System.Drawing.Size(91, 15);
            this.env_date_label.TabIndex = 8;
            this.env_date_label.Text = "<date_string>";
            this.env_date_label.Click += new System.EventHandler(this.EnvelopePanel_Click);
            this.env_date_label.MouseEnter += new System.EventHandler(this.EnvelopePanel_MouseEnter);
            this.env_date_label.MouseLeave += new System.EventHandler(this.EnvelopePanel_MouseLeave);
            // 
            // EnvelopePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.env_subject_label);
            this.Controls.Add(this.env_date_label);
            this.Controls.Add(this.env_from_label);
            this.Controls.Add(this.env_edge_colorbar_panel);
            this.MinimumSize = new System.Drawing.Size(148, 66);
            this.Name = "EnvelopePanel";
            this.Size = new System.Drawing.Size(146, 73);
            this.SizeChanged += new System.EventHandler(this.EnvelopePanel_SizeChanged);
            this.Click += new System.EventHandler(this.EnvelopePanel_Click);
            this.MouseEnter += new System.EventHandler(this.EnvelopePanel_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.EnvelopePanel_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Panel env_edge_colorbar_panel;
        private Label env_from_label;
        private Label env_subject_label;
        private Label env_date_label;
    }
}
