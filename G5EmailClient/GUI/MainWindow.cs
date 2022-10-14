
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
            msg_from_label.MaximumSize    = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_reply_button.   FlatAppearance.BorderSize = 0;
            msg_replyall_button.FlatAppearance.BorderSize = 0;
            msg_forward_button. FlatAppearance.BorderSize = 0;
            brief_control_explain_tooltop.SetToolTip(msg_reply_button,    "Reply");
            brief_control_explain_tooltop.SetToolTip(msg_replyall_button, "Reply All");
            brief_control_explain_tooltop.SetToolTip(msg_forward_button,  "Forward");
                // Compose message tab
            brief_control_explain_tooltop.SetToolTip(cmp_send_button, "Send");
            cmp_send_button.FlatAppearance.BorderSize = 0;
            cmp_add_button. FlatAppearance.BorderSize = 0;

            // Initializing email data.
                // Data
            updateInboxView();
            updateFoldersView();
            active_email_label.Text = EmailClient.GetActiveUser().username;
                // Events
            EmailClient.SentMessage += SentMessageHandler;

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

        
        NotificationPanel prepareNotification(string title, Image image, string body)
        {
            var notification = new NotificationPanel();
                notification.Anchor = AnchorStyles.Top;
                notification.AutoSize = true;
                notification.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                notification.MinimumSize = new Size(notifications_flowpanel.Width - 12, 0);
                notification.titleText = title;
                notification.Image = image;
                notification.bodyText = body;
            return notification;
        }

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

        // This event triggers when the window is maximized,
        // minimized or returned to normal windowed mode.
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

        private void new_message_button_Click(object sender, EventArgs e)
        {
            message_flow_panel.ClearSelction();
            main_tab.SelectedTab = compose_message_tab;
        }

        private void cmp_add_button_Click(object sender, EventArgs e)
        {
            cmp_add_contextstrip.Show(cmp_add_button, new Point(cmp_add_button.Location.X,
                                                                cmp_add_button.Location.Y
                                                              - cmp_add_button.Height - 10));
        }

        private void add_bcc_menuitem_Click(object sender, EventArgs e)
        {
            cmp_bcc_panel.Visible = !cmp_bcc_panel.Visible;
            if (!cmp_bcc_panel.Visible)
                add_bcc_menuitem.Text = "Add Bcc";
            else
                add_bcc_menuitem.Text = "Remove Bcc";
        }

        private void add_cc_menuitem_Click(object sender, EventArgs e)
        {
            cmp_cc_panel.Visible = !cmp_cc_panel.Visible;
            if (!cmp_cc_panel.Visible)
                add_cc_menuitem.Text = "Add cc";
            else
                add_cc_menuitem.Text = "Remove cc";
        }

        private void delete_button_MouseEnter(object sender, EventArgs e)
        {
            delete_button.Image = Properties.Resources.DeleteAnimatedIcon;
        }

        private void delete_button_MouseLeave(object sender, EventArgs e)
        {
            delete_button.Image = Properties.Resources.DeleteIcon;
        }

        private void cmp_send_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            IEmail.Message message = new();
                message.to      = cmp_to_textbox.Text;
                message.cc      = cmp_cc_textbox.Text;
                message.bcc     = cmp_bcc_textbox.Text;
                message.subject = cmp_subject_textbox.Text;
                message.body    = cmp_mailbody_rtextbox.Text;

            EmailClient.SendMessage(message);

            cmp_to_textbox.Text        = "";
            cmp_cc_textbox.Text        = "";
            cmp_bcc_textbox.Text       = "";
            cmp_subject_textbox.Text   = "";
            cmp_mailbody_rtextbox.Text = "";
            this.Cursor = Cursors.Default;
        }
        private void SentMessageHandler(Exception ex, IEmail.Message message)
        {
            if(ex != null)
            {
                if (notifications_flowpanel.InvokeRequired)
                {
                    Action safeFailHandler = delegate { SentMessageHandler(ex, message); };
                    notifications_flowpanel.Invoke(safeFailHandler);
                }
                else
                {
                    var notification = prepareNotification("Send Failed!",
                                                           Properties.Resources.ErrorAnimated_Icon,
                                                           "Click to retry. Error:\n" + ex.Message);
                    notification.RespondsToClick = true;
                    notification.Object = message;
                    notification.NotificationBodyClicked += LoadMessageEventDriver;
                    notifications_flowpanel.Controls.Add(notification);
                }
            }
        }

        /// <summary>
        /// This function loads a message into the message compose panel
        /// </summary>
        /// <param name="message"></param>
        private void LoadMessage(IEmail.Message message)
        {
            cmp_to_textbox.Text        = message.to;
            cmp_cc_textbox.Text        = message.cc;
            cmp_bcc_textbox.Text       = message.bcc;
            cmp_subject_textbox.Text   = message.subject;
            cmp_mailbody_rtextbox.Text = message.body;
        }
        private void LoadMessageEventDriver(object sender, EventArgs e)
        {
            Debug.WriteLine("Running LoadMessage driver");
            var message = (IEmail.Message)sender;
            LoadMessage(message);
        }

        private void msg_reply_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text        = msg_from_label.Text;
            cmp_subject_textbox.Text   = "RE:" + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n________________\n"
                                           + "Previous Message:"
                                           + "\nFrom: " + msg_from_label.Text
                                           + "\nSubject " + msg_subject_label.Text
                                           + "\n" + msg_body_rtextbox.Text;
            main_tab.SelectedTab = compose_message_tab;
        }
    }
}