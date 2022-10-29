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
    public partial class EnvelopeFlowPanel : UserControl
    {
        List<EnvelopePanel> panelList = new();
        List<EnvelopePanel> selectedPanels = new();


        public EnvelopeFlowPanel()
        {
            InitializeComponent();
        }

        // Defining indexing operator
        public EnvelopePanel this[int index]
        {
            get { return panelList[index]; }
        }


        public int ListSize
        {
            get { return panelList.Count; }
        }


        /// <summary>
        /// Adds a panel to the flow control. The index will be saved in the control and returned
        /// when getting selected items.
        /// </summary>
        public void Add(int index, string from, string date, string subject, bool seen)
        {
            var envelopePanel = new EnvelopePanel();
            envelopePanel.index = index;
            envelopePanel.fromText = from;
            envelopePanel.dateText = date;
            envelopePanel.subjectText = subject;
            if (!seen)
                envelopePanel.toggleRead();
            envelopePanel.Anchor = AnchorStyles.Top;
            envelopePanel.AutoSize = true;
            envelopePanel.MinimumSize = new Size(flow_control.Width - 6 - SystemInformation.VerticalScrollBarWidth, 68);
            envelopePanel.PanelClicked += EnvelopePanel_Click;
            // Adding the control to the window
            flow_control.Controls.Add(envelopePanel);
            // Adding to the list
            panelList.Add(envelopePanel);
        }

        /// <summary>
        /// Toggles read appearance for all envelopes currently selected. Returns a list of the
        /// indices of the affected envelopePanels
        /// </summary>
        /// <returns></returns>
        public List<int> ToggleReadSelected()
        {
            List<int> indices = new();
            foreach (var envelope in selectedPanels)
            {
                envelope.toggleRead();
                indices.Add(envelope.index);
            }
            return indices;
        }

        /// <summary>
        /// Remove all selected envelopePanels from the control. Returns a list of the
        /// indices of the affected envelopePanels.
        /// </summary>
        /// <returns></returns>
        public List<int> DeleteSelected()
        {
            List<int> indices = new();
            foreach (var envelope in selectedPanels)
            {
                envelope.Dispose();
                indices.Add(envelope.index);
            }
            return indices;
        }

        /// <summary>
        /// Dispoces all envelope panels in the control.
        /// </summary>
        public void Clear()
        {
            foreach (var panel in panelList) panel.Dispose();
            panelList.Clear();
            selectedPanels.Clear();
        }

        /// <summary>
        /// Deselects all envelope panels.
        /// </summary>
        public void ClearSelection()
        {
            foreach(var envelope in selectedPanels)
            {
                envelope.SetSelected(false);
            }
            selectedPanels.Clear();
        }

        /// <summary>
        /// Returns the selected index. 
        /// If more than one panel is selected, it returns the first index.
        /// If no panel is selected, returns -1.
        /// </summary>
        public int SelectedIndex()
        {
            if (selectedPanels.Count == 0) 
                return -1;
            else 
                return selectedPanels[0].index;
        }

        /// <summary>
        /// Returns a list of indices for all selected panels. If no panels are selected, returns an empty list.
        /// </summary>
        /// <returns></returns>
        public List<int> SelectedIndices()
        {
            List<int> indices = new();
            foreach(var panel in selectedPanels) 
                indices.Add(panel.index);
            return indices;
        }

        //
        // Internal events
        //
        private void EnvelopePanel_Click(object sender, EventArgs e)
        {
            EnvelopePanel panel = (EnvelopePanel)sender;
            if(!ModifierKeys.HasFlag(Keys.Control))
            {
                // When the control button is not held, click opens the message
                ClearSelection();
                panel.SetSelected(true);

                Debug.WriteLine("Flow panel opening message");
                panel.setRead();
                this.EnvelopePanelOpened(panel, e);
            }
            else
            {
                panel.SetSelected(!panel.Selected);
            }
            selectedPanels.Add(panel);
        }

        //
        // External events
        //
        public event EventHandler EnvelopePanelOpened;
    }
}
