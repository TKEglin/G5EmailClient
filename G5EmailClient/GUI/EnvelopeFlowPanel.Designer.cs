namespace G5EmailClient.GUI
{
    partial class EnvelopeFlowPanel
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
            this.flow_control = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flow_control
            // 
            this.flow_control.AutoScroll = true;
            this.flow_control.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flow_control.Location = new System.Drawing.Point(0, 0);
            this.flow_control.Name = "flow_control";
            this.flow_control.Size = new System.Drawing.Size(239, 395);
            this.flow_control.TabIndex = 0;
            // 
            // EnvelopeFlowPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flow_control);
            this.Name = "EnvelopeFlowPanel";
            this.Size = new System.Drawing.Size(239, 395);
            this.ResumeLayout(false);

        }

        #endregion

        private FlowLayoutPanel flow_control;
    }
}
