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
        Dictionary<string, EnvelopePanel> panelList = new();
        List<EnvelopePanel> selectedPanels = new();

        public bool needsUpdate = true;
        public bool envelopesHidden = false;

        // Used to store the index of the displayed folder
        public int folderIndex = -1;

        /// <summary>
        /// Set to true if the Envelopes in this panel are copies
        /// </summary>
        public bool isCopyPanel = false;
        public EnvelopeFlowPanel? sourcePanel;


        public EnvelopeFlowPanel()
        {
            InitializeComponent();
        }

        // Defining indexing operator
        public EnvelopePanel this[string UID]
        {
            get { return panelList[UID]; }
        }

        // Defining indexing operator
        public EnvelopePanel this[int index]
        {
            get { return (EnvelopePanel)flow_control.Controls[index]; }
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
            var NewPanelDate = DateTimeOffset.Parse(envelopePanel.dateText);

            // ___ SORT DISABLED FOR OPTIMIZATION. IMPLEMENT IN CLIENT CLASS INSTEAD. ___
            // Finding date sorted location
            //foreach (EnvelopePanel OldPanel in flow_control.Controls)
            //{
            //    var OldPanelDate = DateTimeOffset.Parse(OldPanel.dateText);
            //    if (OldPanelDate.CompareTo(NewPanelDate) < 0)
            //    {
            //        Debug.WriteLine("Reached later message. Setting index");
            //        var index = flow_control.Controls.IndexOf(OldPanel);
            //        flow_control.Controls.SetChildIndex(envelopePanel, index);
            //        flow_control.Controls.SetChildIndex(OldPanel, index + 1);
            //        break;
            //    }
            //}

            // Adding to the list
            panelList[UID] = envelopePanel;
        }
        public void Add(EnvelopePanel envelopePanel)
        {
            panelList[envelopePanel.UID] = envelopePanel;
            
            if(envelopePanel.Selected)
                selectedPanels.Add(envelopePanel);

            // Adding the control to the window
            flow_control.Controls.Add(envelopePanel);
            var NewPanelDate = DateTimeOffset.Parse(envelopePanel.dateText);

            // ___ SORT DISABLED FOR OPTIMIZATION. IMPLEMENT IN CLIENT CLASS INSTEAD. ___
            // Finding date sorted location
            //foreach (EnvelopePanel OldPanel in flow_control.Controls)
            //{
            //    var OldPanelDate = DateTimeOffset.Parse(OldPanel.dateText);
            //    if (OldPanelDate.CompareTo(NewPanelDate) < 0)
            //    {
            //        Debug.WriteLine("Reached later message. Setting index");
            //        var index = flow_control.Controls.IndexOf(OldPanel);
            //        flow_control.Controls.SetChildIndex(envelopePanel, index);
            //        flow_control.Controls.SetChildIndex(OldPanel, index + 1);
            //        break;
            //    }
            //}
            // Adding to the list
            panelList[envelopePanel.UID] = envelopePanel;
        }

        public void AddToFront(string UID, string from, string date, string subject, bool seen)
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
            flow_control.Controls.SetChildIndex(envelopePanel, 0);
            var NewPanelDate = DateTimeOffset.Parse(envelopePanel.dateText);
        }

        public void AddLoadMorePanel()
        {
            var LoadMorePanel = new LoadMorePanel();
                LoadMorePanel.Anchor = AnchorStyles.Top;
                LoadMorePanel.AutoSize = true;
                LoadMorePanel.MinimumSize = new Size(flow_control.Width - 6 - SystemInformation.VerticalScrollBarWidth, 68);
                LoadMorePanel.PanelClicked += LoadMorePanel_Click;
            flow_control.Controls.Add(LoadMorePanel);
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
        /// Disposes all envelope panels in the control.
        /// </summary>
        public void Clear()
        {
            if (!isCopyPanel)
                foreach (var panel in panelList) panel.Value.Dispose();
            else
                if(sourcePanel != null)
                    foreach (var panel in panelList)
                        sourcePanel.Add(panel.Value);

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

        /// <summary>
        /// Makes all panels visible.
        /// </summary>
        public void ShowAll()
        {
            foreach (var panel in panelList) panel.Value.Visible = true;
            envelopesHidden = false;
        }

        /// <summary>
        /// Hides all panels with UIDs not included in the parameter list.
        /// </summary>
        public void HideRest(List<string> UIDs)
        {
            //Hiding all
            foreach (var panel in panelList) panel.Value.Visible = false;
            //Showing selected
            foreach (var UID in UIDs) panelList[UID].Visible = true;

            envelopesHidden = true;
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

                panel.setRead();
                this.EnvelopePanelOpened(panel, e);
            }
            else
            {
                panel.SetSelected(!panel.Selected);
            }
            selectedPanels.Add(panel);
        }

        private void LoadMorePanel_Click(object sender, EventArgs e)
        {
            var panel = (LoadMorePanel)sender;
            flow_control.Controls.Remove(panel);
            this.LoadMoreClicked(this, e);
        }


        //
        // External events
        //
        public event EventHandler EnvelopePanelOpened;

        public event EventHandler LoadMoreClicked;
    }
}
