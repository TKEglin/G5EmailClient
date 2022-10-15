using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace G5EmailClient.GUI
{
    public partial class NotificationPanel : UserControl
    {
        object? StoredObject;
        bool selected = false;

        public NotificationPanel()
        {
            InitializeComponent();

            not_left_button.FlatAppearance.BorderSize = 0;
            collapse_button.FlatAppearance.BorderSize = 0;

            this.Anchor = AnchorStyles.Top;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        }

        [Category("Fields"), Description("The object attached to the notification")]
        public object? Object
        {
            get
            {
                return StoredObject;
            }
            set
            {
                StoredObject = value;
            }
        }
        [Category("Fields"), Description("The image of the icon")]
        public Image Image
        {
            get
            {
                return not_icon.BackgroundImage;
            }
            set
            {
                not_icon.BackgroundImage = value;
            }
        }
        [Category("Fields"), Description("The text of the title label")]
        public string titleText
        {
            get
            {
                return not_title_label.Text;
            }
            set
            {
                not_title_label.Text = value;
            }
        }
        [Category("Fields"), Description("The text of the body text label")]
        public string bodyText
        {
            get
            {
                return not_text_label.Text;
            }
            set
            {
                not_text_label.Text = value;
            }
        }

        private void not_left_button_Click(object sender, EventArgs e)
        {
            this.NotificationClosed(null, e);
            this.Dispose();
        }
        public EventHandler NotificationClosed;

        private void title_panel_Click(object sender, EventArgs e)
        {
            if(!selected)
            {
                not_text_panel.Visible = !not_text_panel.Visible;
                not_button_panel.Visible = !not_button_panel.Visible;
                selected = true;
            }
            else
            {
                not_text_panel_Click(sender, e);
            }
        }

        private void NotificationPanel_SizeChanged(object sender, EventArgs e)
        {
            not_title_label.MaximumSize = new Size(this.Width - 6 - not_icon.Width, 0);
            not_text_label.MaximumSize  = new Size(this.Width - 10,                 0);
        }

        private void NotificationPanel_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ButtonFace;
        }

        private void NotificationPanel_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ButtonHighlight;
        }

        private void not_text_panel_Click(object sender, EventArgs e)
        {
            this.NotificationClosed(null, e);
            this.NotificationBodyClicked(StoredObject, e);
            this.Dispose();
        }
        public event EventHandler NotificationBodyClicked;

        private void collapse_button_Click(object sender, EventArgs e)
        {
            not_text_panel.Visible = !not_text_panel.Visible;
            not_button_panel.Visible = !not_button_panel.Visible;
            selected = false;
        }
    }
}
