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
    public partial class LoadMorePanel : UserControl
    {
        public LoadMorePanel()
        {
            InitializeComponent();
        }

        private void LoadMorePanel_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ButtonFace;
        }

        private void LoadMorePanel_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = SystemColors.ButtonHighlight   ;
        }

        private void LoadMorePanel_Click(object sender, EventArgs e)
        {
            this.PanelClicked(this, e);
        }
        public event EventHandler PanelClicked;
    }
}
