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
    public partial class EnvelopePanel : UserControl
    {
        internal bool Selected = false;
        public string UID = string.Empty;

        // Definition of the color of the edge bar for each status
        static Color ReadColor = Color.LightGray;
        static Color UnreadColor = Color.Black;

        public EnvelopePanel()
        {
            InitializeComponent();

            env_from_label.MaximumSize = new Size(this.Width - 15, 0);
            env_date_label.MaximumSize = new Size(this.Width - 15, 0);
            env_subject_label.MaximumSize = new Size(this.Width - 15, 0);
        }

        public void toggleRead()
        {
            if (env_edge_colorbar_panel.BackColor == ReadColor)
                env_edge_colorbar_panel.BackColor = UnreadColor;
            else
                env_edge_colorbar_panel.BackColor = ReadColor;
        }

        public void setRead()
        {
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
        [Category("Fields"), Description("The text of the date label")]
        public string dateText
        {
            get
            {
                return env_date_label.Text;
            }
            set
            {
                env_date_label.Text = value;
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
                if (value == false)
                {
                    Debug.WriteLine("Setting selected false flag and changing color");
                    BackColor = SystemColors.ButtonHighlight;
                    Selected = false;
                }
                else
                {
                    Debug.WriteLine("Setting selected true flag and changing color");
                    BackColor = SystemColors.ActiveBorder;
                    Selected = true;
                }
            }
        }

        public void SetSelected(bool value)
        {
            if (value == false)
            {
                Debug.WriteLine("Setting selected false flag and changing color");
                BackColor = SystemColors.ButtonHighlight;
                Selected = false;
            }
            else
            {
                Debug.WriteLine("Setting selected true flag and changing color");
                BackColor = SystemColors.ActiveBorder;
                Selected = true;
            }
        }

        private void EnvelopePanel_SizeChanged(object sender, EventArgs e)
        {
            env_from_label.MaximumSize = new Size(Width - 15, 0);
            env_date_label.MaximumSize = new Size(Width - 15, 0);
            env_subject_label.MaximumSize = new Size(Width - 15, 0);
        }

        private void EnvelopePanel_MouseEnter(object sender, EventArgs e)
        {
            if(!Selected)
            {
                BackColor = SystemColors.ButtonFace;
            }
        }

        private void EnvelopePanel_MouseLeave(object sender, EventArgs e)
        {
            if (!Selected) 
            {
                BackColor = SystemColors.ButtonHighlight;
            }
        }

        private void EnvelopePanel_Click(object sender, EventArgs e)
        {
            this.PanelClicked(this, e);
        }
        public event EventHandler PanelClicked;
    }
}
