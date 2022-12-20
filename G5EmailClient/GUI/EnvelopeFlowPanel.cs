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

        public bool hasLoadMorePanel = false;

        /// <summary>
        /// Set to true if the Envelopes in this panel are copies
        /// </summary>
        public bool isCopyPanel = false;
        // A panel is either copy/search panel or source panel
        public EnvelopeFlowPanel? sourcePanel;
        public EnvelopeFlowPanel? searchPanel;

        public EnvelopeFlowPanel()
        {
            InitializeComponent();
        }
        public EnvelopeFlowPanel(EnvelopeFlowPanel source)
        {
            InitializeComponent();

            isCopyPanel = true;
            sourcePanel = source;
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
            hasLoadMorePanel = true;
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
        /// Toggles read appearance for all envelopes for the given UIDs.
        /// </summary>
        public void ToggleReadUIDs(List<string> UIDs)
        {
            foreach (var UID in UIDs)
            {
                if (panelList.ContainsKey(UID))
                {
                    panelList[UID].toggleRead();
                }
            }
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
        /// Remove all envelopePanels with the given UIDs from the control.
        /// </summary>
        public void DeleteUIDs(List<string> UIDs)
        {
            foreach (var UID in UIDs)
            {
                if(panelList.ContainsKey(UID))
                    panelList[UID].Dispose();
            }
        }

        /// <summary>
        /// Disposes all envelope panels in the control.
        /// </summary>
        public void Clear()
        {
            foreach (var panel in panelList) panel.Value.Dispose();

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

        public List<string> SelectedUIDs()
        {
            var UIDs = new List<string>();

            foreach(var panel in selectedPanels)
            {
                UIDs.Add(panel.UID);
            }

            return UIDs;
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

                if(isCopyPanel)
                {
                    sourcePanel!.ClearSelection();
                    sourcePanel.SetEnvelopeSelected(panel.UID);
                    sourcePanel.SetEnvelopeRead(panel.UID);
                }

                this.EnvelopePanelOpened(panel, e);
            }
            else
            {
                panel.SetSelected(!panel.Selected);

                if (isCopyPanel)
                {
                    sourcePanel.SetEnvelopeSelected(panel.UID);
                }
            }
            selectedPanels.Add(panel);
        }

        public void SetEnvelopeSelected(string UID)
        {
            if(panelList.ContainsKey(UID))
            {
                var envelope = panelList[UID];
                selectedPanels.Add(envelope);
                envelope.SetSelected(true);
            }
        }

        public void SetEnvelopeRead(string UID)
        {
            if (panelList.ContainsKey(UID))
            {
                panelList[UID].setRead();
            }
        }

        public void LoadMorePanel_Click(object? sender, EventArgs e)
        {
            //var panel = (LoadMorePanel)sender;
            flow_control.Controls.RemoveAt(flow_control.Controls.Count-1);
            hasLoadMorePanel = false;
            this.LoadMoreClicked(this, e);
        }

        /// <summary>
        /// Creates a copy panel to be used for search.
        /// Make sure to run
        /// </summary>
        /// <returns>The searchPanel</returns>
        public EnvelopeFlowPanel GetSearchPanel()
        {
            if(searchPanel == null)
            {
                searchPanel = new EnvelopeFlowPanel(this);

                searchPanel.Visible = true;
                searchPanel.folderIndex = this.folderIndex;
                searchPanel.Dock = this.Dock;
                searchPanel.Parent = this.Parent;
                searchPanel.Location = this.Location;
                searchPanel.AutoSize = this.AutoSize;
                searchPanel.AutoScroll = this.AutoScroll;
                searchPanel.MinimumSize = this.MinimumSize;
                searchPanel.AutoSizeMode = this.AutoSizeMode;
                searchPanel.EnvelopePanelOpened = this.EnvelopePanelOpened;
                searchPanel.LoadMoreClicked += this.LoadMoreClicked;
            }
            else
            {
                searchPanel.Clear();
            }

            return searchPanel;
        }

        //
        // External events
        //
        public event EventHandler EnvelopePanelOpened;

        public event EventHandler LoadMoreClicked;
    }
}
