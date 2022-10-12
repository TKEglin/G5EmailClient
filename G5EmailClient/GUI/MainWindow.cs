
using System.Diagnostics;
using System.Timers;
using System.Threading;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

        List<EnvelopePanel> envelopePanels = new();

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            // Enabling window dragging for controls
            AddDraggingControl(this);
            AddDraggingControl(top_toolstrip);

            // Saving email client to local variable
            EmailClient = ParamEmailClient;

            // Opening connection form
            this.Visible = false;
            Application.Run(new ConnectionForm(EmailClient));
            this.Visible = true;

            // Setting up GUI
                // Main Tab Page window:
            main_tab.TabPages.Remove(compose_message_tab);
            main_tab.TabPages.Remove(open_message_tab);
            main_tab.Appearance = TabAppearance.FlatButtons;
            main_tab.ItemSize = new Size(0, 1);
            main_tab.SizeMode = TabSizeMode.Fixed;

            // Initializing email data.
            updateInboxView();
            updateFoldersView();
            active_email_label.Text = EmailClient.GetActiveUser().username;
        }

        #region utility functions
        // This timer will be used for the button functions.
        // It is declared here so it can be reset by new calls of tempDisableButton
        System.Timers.Timer buttonTimer = new(1000) { Enabled = false };
        /// <summary>
        /// Temporarily disables a button for time 'double seconds'.
        /// </summary>
        /// <param name="button"></param>
        void tempDisableButton(ToolStripButton button, double seconds)
        {
            if(button.Owner.InvokeRequired)
            {
                Action safeDisable = delegate { tempDisableButton(button, seconds); };
                button.Owner.Invoke(safeDisable);
            }
            else
            {
                button.Enabled = false;

                buttonTimer.Interval = seconds * 1000;
                buttonTimer.Enabled = true;
                buttonTimer.Elapsed += (sender, args) =>
                {
                    reenableButton(button);
                    buttonTimer.Enabled = false;
                };
            }
        }
        void reenableButton(ToolStripButton button)
        {
            if (button.Owner.InvokeRequired)
            {
                Action safeEnable = delegate { reenableButton(button); };
                button.Owner.Invoke(safeEnable);
            }
            else
            {
                button.Enabled = true;
            }
        }

        #endregion

        /// <summary>
        /// Gets message envelopes for the active inbox and adds them to the inbox_view.
        /// Ideally this will be done asynchronously.
        /// </summary>
        void updateInboxView()
        {
            // Clearing inbox
            foreach (var envelope in envelopePanels) envelope.Dispose();
            envelopePanels.Clear();

            // The internal index value of the envelope will match the index in the envelopePanels list
            int index = -1;
            var envelopes = EmailClient.GetFolderEnvelopes();
            foreach (var envelope in envelopes)
            {
                index++;
                var envelopePanel = new EnvelopePanel();
                    envelopePanel.index = index;
                    envelopePanel.fromText = envelope.from.ToString();
                    envelopePanel.subjectText = envelope.subject;
                    if (!envelope.read)
                        envelopePanel.toggleRead();
                    envelopePanel.Anchor = AnchorStyles.Top;
                    envelopePanel.AutoSize = true;
                    envelopePanel.MinimumSize = new Size(envelopes_flowpanel.Width - 6 - SystemInformation.VerticalScrollBarWidth, 68);
                    envelopePanel.DoubleClick += new EventHandler(EnvelopePanel_DoubleClick);
                // Adding the control to the window
                envelopes_flowpanel.Controls.Add(envelopePanel);
                // Adding to list for later use
                envelopePanels.Add(envelopePanel);
            }
        }

        /// <summary>
        /// Updates the folder listbox
        /// </summary>
        void updateFoldersView()
        {
            folders_lisbox.Items.Clear();
            foreach (var folderName in EmailClient.GetFoldernames())
            {
                folders_lisbox.Items.Add(folderName);
            }
        }

        private void EnvelopePanel_DoubleClick(object sender, EventArgs e)
        {
            var envelope = (EnvelopePanel)sender;
            var message = EmailClient.OpenMessage(envelope.index);
            // Test
            Debug.WriteLine("Opening message " + envelope.index.ToString() + ": " 
                          + message.from + message.subject + message.body);
        }

        private void toggle_read_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripButton button = (ToolStripButton)sender;
            tempDisableButton(button, 0.2);

            foreach(var envelope in envelopePanels)
            {
                if(envelope.Selected)
                {
                    // Updating the UI representation of the read status
                    envelope.toggleRead();
                    // Calling client function
                    EmailClient.ToggleRead(envelope.index);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach(var envelope in envelopePanels)
            {
                if(envelope.Selected)
                {
                    // Calling client function
                    EmailClient.Delete(envelope.index);
                    // Locally removing the envelope
                    envelope.Dispose();
                }
            }
            this.Cursor = Cursors.Default;
        }
    }
}