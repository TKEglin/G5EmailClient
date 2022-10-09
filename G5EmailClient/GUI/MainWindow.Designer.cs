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
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.top_panel.SuspendLayout();
            this.top_toolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // top_panel
            // 
            this.top_panel.Controls.Add(this.top_toolstrip);
            this.top_panel.Dock = System.Windows.Forms.DockStyle.Top;
            this.top_panel.Location = new System.Drawing.Point(0, 0);
            this.top_panel.Name = "top_panel";
            this.top_panel.Size = new System.Drawing.Size(1000, 100);
            this.top_panel.TabIndex = 0;
            // 
            // top_toolstrip
            // 
            this.top_toolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.top_toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.top_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.top_toolstrip.Location = new System.Drawing.Point(0, 0);
            this.top_toolstrip.Name = "top_toolstrip";
            this.top_toolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.top_toolstrip.Size = new System.Drawing.Size(1000, 27);
            this.top_toolstrip.Stretch = true;
            this.top_toolstrip.TabIndex = 0;
            this.top_toolstrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(125, 24);
            this.toolStripButton1.Text = "New Message";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.top_panel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1018, 647);
            this.Name = "MainWindow";
            this.Text = "Email Client";
            this.top_panel.ResumeLayout(false);
            this.top_panel.PerformLayout();
            this.top_toolstrip.ResumeLayout(false);
            this.top_toolstrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel top_panel;
        private ToolStrip top_toolstrip;
        private ToolStripButton toolStripButton1;
    }
}