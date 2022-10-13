
using System.Diagnostics;
using System.Timers;
using System.Threading;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{
    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

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
                // General
            this.Resize += Window_Resize;
                // Main Tab Page window
            main_tab.Appearance = TabAppearance.FlatButtons;
            main_tab.ItemSize = new Size(0, 1);
            main_tab.SizeMode = TabSizeMode.Fixed;
                // Message flow panel
            message_flow_panel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                // Message open tab
            msg_from_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_reply_button.   FlatAppearance.BorderSize = 0;
            msg_replyall_button.FlatAppearance.BorderSize = 0;
            msg_forward_button. FlatAppearance.BorderSize = 0;
            brief_control_explain_tooltop.SetToolTip(msg_reply_button,    "Reply");
            brief_control_explain_tooltop.SetToolTip(msg_replyall_button, "Reply All");
            brief_control_explain_tooltop.SetToolTip(msg_forward_button,  "Forward");

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
            message_flow_panel.Clear();

            // The internal index value of the envelope will match the index in the envelopePanels list
            int index = -1;
            var envelopes = EmailClient.GetFolderEnvelopes();
            foreach (var envelope in envelopes)
            {
                index++;
                message_flow_panel.Add(index, envelope.from.ToString(), 
                                             envelope.subject, 
                                             envelope.read);
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

        private void EnvelopePanel_MessageOpen(object sender, EventArgs e)
        {
            var envelope = (EnvelopePanel)sender;

            var message = EmailClient.OpenMessage(envelope.index);
            msg_from_label.Text    = message.from;
            msg_subject_label.Text = message.subject;
            msg_body_rtextbox.Text = message.body;

            Debug.WriteLine("Opening message " + envelope.index.ToString() + ": " 
                          + message.from + message.subject + message.body);

            main_tab.SelectedTab = open_message_tab;
        }

        private void toggle_read_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripButton button = (ToolStripButton)sender;
            tempDisableButton(button, 0.2);

            var indices = message_flow_panel.ToggleReadSelected();

            foreach(var index in indices)
            {
                EmailClient.ToggleRead(index);
            }
            this.Cursor = Cursors.Default;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var deleted_indices = message_flow_panel.DeleteSelected();
            foreach(var index in deleted_indices)
            {
                EmailClient.Delete(index);
            }
            
            this.Cursor = Cursors.Default;
        }

        private void msg_senderinfo_panel_Resize(object sender, EventArgs e)
        {
            msg_from_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
        }

        // This event triggers when the window state changes
        FormWindowState LastWindowState = FormWindowState.Minimized;
        private void Window_Resize(object sender, EventArgs e)
        {
            // When window state changes
            if (WindowState != LastWindowState)
            {
                LastWindowState = WindowState;

                // Resizing message labels
                msg_from_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
                msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
                msg_senderinfo_panel.Size = new Size(msg_senderinfo_panel.Width,
                                                       msg_from_label.Height
                                                     + msg_subject_label.Height
                                                     + msg_senderinfo_padding_panel.Height);
            }
        }
    }
}