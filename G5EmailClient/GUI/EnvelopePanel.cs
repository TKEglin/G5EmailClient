using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace G5EmailClient.GUI
{
    public partial class EnvelopePanel : UserControl
    {
        internal bool Selected = false;
        public int index = -1;

        // Definition of the color of the edge bar for each status
        static Color ReadColor = Color.LightGray;
        static Color UnreadColor = Color.Black;

        public EnvelopePanel()
        {
            InitializeComponent();

            env_from_label.MaximumSize = new Size(this.Width - 15, 0);
            env_subject_label.MaximumSize = new Size(this.Width - 15, 0);
        }

        public void toggleRead()
        {
            if (env_edge_colorbar_panel.BackColor == ReadColor)
                env_edge_colorbar_panel.BackColor = UnreadColor;
            else
                env_edge_colorbar_panel.BackColor = ReadColor;
        }

        [Category("Fields"), Description("The text of the from label")]
        public string fromText
        {
            get
            {
                return env_from_label.Text;
            }
            set
            {
                env_from_label.Text = value;
            }
        }
        [Category("Fields"), Description("The text of the subject label")]
        public string subjectText
        {
            get
            {
                return env_subject_label.Text;
            }
            set
            {
                env_subject_label.Text = value;
            }
        }
        [Category("Fields"), Description("The color of the edge bar")]
        public Color colorBar
        {
            get
            {
                return env_edge_colorbar_panel.BackColor;
            }
            set
            {
                env_edge_colorbar_panel.BackColor = value;
            }
        }
        [Category("Fields"), Description("The color of the edge bar")]
        public bool selected
        {
            get
            {
                return Selected;
            }
            set
            {
                if (value == Selected)
                {
                    return;
                }
                else if (!Selected)
                {
                    BackColor = SystemColors.ActiveBorder;
                    Selected = true;
                }
                else
                {
                    BackColor = SystemColors.ButtonHighlight;
                    Selected = false;
                }
            }
        }

        private void EnvelopePanel_SizeChanged(object sender, EventArgs e)
        {
            env_from_label.MaximumSize = new Size(Width - 15, 0);
            env_subject_label.MaximumSize = new Size(Width - 15, 0);
        }

        private void EnvelopePanel_MouseEnter(object sender, EventArgs e)
        {
            if(!Selected)
                BackColor = SystemColors.ButtonFace;
        }

        private void EnvelopePanel_MouseLeave(object sender, EventArgs e)
        {
            if(!Selected)
                BackColor = SystemColors.ButtonHighlight;
        }

        private void EnvelopePanel_Click(object sender, EventArgs e)
        {
            if(!Selected)
            {
                BackColor = SystemColors.ActiveBorder;
                Selected = true;
            }
            else
            {
                BackColor = SystemColors.ButtonFace; 
                Selected = false;
            }
        }
    }
}
