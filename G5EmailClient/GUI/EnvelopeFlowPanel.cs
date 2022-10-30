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

        public bool needsUpdate = false;


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
        public void Add(string UID, string from, string date, string subject, bool seen)
        {
            var envelopePanel = new EnvelopePanel();
            envelopePanel.UID = UID;
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
        /// UIDs of the affected envelopePanels
        /// </summary>
        /// <returns></returns>
        public List<string> ToggleReadSelected()
        {
            List<string> UIDs = new();
            foreach (var envelope in selectedPanels)
            {
                envelope.toggleRead();
                UIDs.Add(envelope.UID);
            }
            return UIDs;
        }

        /// <summary>
        /// Remove all selected envelopePanels from the control. Returns a list of the
        /// UIDs of the affected envelopePanels.
        /// </summary>
        /// <returns></returns>
        public List<string> DeleteSelected()
        {
            List<string> UIDs = new();
            foreach (var envelope in selectedPanels)
            {
                envelope.Dispose();
                UIDs.Add(envelope.UID);
            }
            return UIDs;
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
        /// Returns the selected UID. 
        /// If more than one panel is selected, it returns the first UID.
        /// If no panel is selected, returns -1.
        /// </summary>
        public string SelectedUID()
        {
            if (selectedPanels.Count == 0) 
                return string.Empty;
            else 
                return selectedPanels[0].UID;
        }

        /// <summary>
        /// Returns a list of UIDs for all selected panels. If no panels are selected, returns an empty list.
        /// </summary>
        /// <returns></returns>
        public List<string> SelectedIndices()
        {
            List<string> UIDs = new();
            foreach(var panel in selectedPanels) 
                UIDs.Add(panel.UID);
            return UIDs;
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

        public class EnvelopeDateComparer : Comparer<EnvelopePanel>
        {

            public override int Compare(EnvelopePanel? x, EnvelopePanel? y)
            {
                var date_x = DateTimeOffset.Parse(x.dateText);
                var date_y = DateTimeOffset.Parse(y.dateText);

                return date_x.CompareTo(date_y);
            }
        }
        /// <summary>
        /// Sorts the panels by date
        /// </summary>
        public void SortDate()
        {
            panelList.Sort(0, panelList.Count, new EnvelopeDateComparer());

            flow_control.Controls.Clear();

            foreach(var panel in panelList) flow_control.Controls.Add(panel);
        }

        //
        // External events
        //
        public event EventHandler EnvelopePanelOpened;
    }
}
