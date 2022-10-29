
using System.Diagnostics;
using System.Timers;
using System.Threading;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{

    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;

        int NotificationsCount = 0;

        // This list will be used to smoothly switch between folders
        List<EnvelopeFlowPanel> EnvelopeFlowPanels = new();
        EnvelopeFlowPanel? activePanel;

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
            template_flow_panel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                // Message open tab
            msg_from_label.MaximumSize    = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
            msg_cc_label.Text = string.Empty;
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

            if (EmailClient.isConnected())
            {
            // Initializing email data.
                // Data
                updateFoldersView();
                updateFolderView(0, true);
                active_email_label.Text = EmailClient.GetActiveUser().username;
                // Events
                EmailClient.SentMessage += SentMessageHandler;
                EmailClient.InboxUpdateFinished += InboxUpdateFinishedHandler;

                // Setting inbox as default selected folder
                folders_lisbox.SetSelected(0, true);
            }
        }

        #region utility functions
        // This timer will be used for the button functions.
        // It is declared here so it can be reset by new calls of tempDisableButton
        System.Timers.Timer buttonTimer = new(1000) { Enabled = false };
        /// <summary>
        /// Temporarily disables a button for time 'double seconds'.
        /// If seconds <= 0, the button will be disabled until reenabled
        /// using reenableButton();
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

                if(seconds > 0)
                {
                    buttonTimer.Interval = seconds * 1000;
                    buttonTimer.Enabled = true;
                    buttonTimer.Elapsed += (sender, args) =>
                    {
                        reenableButton(button);
                        buttonTimer.Enabled = false;
                    };
                }
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

        /// <summary>
        /// This function modifies the appearance of the folders listbox
        /// </summary>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.LightGray);

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(folders_lisbox.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            // e.DrawFocusRectangle();
        }

        /// <summary>
        /// Remove the quotes from a from email. Returns the email address.
        /// </summary>
        string trimFromToEmail(string from)
        {
            if (!from.Contains("\""))
                return from;

            string[] split_string = from.Split("<");

            // If "<" is part of the nickname, there will be more than two string,
            // so the last string has the ">" removed and is returned.
            return split_string.Last().Replace(">", "");
        }

        #endregion

        /// <summary>
        /// Prepares and returns a notification panel.
        /// </summary>
        NotificationPanel prepareNotification(string title, Image image, string body)
        {
            var notification = new NotificationPanel();
                notification.MinimumSize = new Size(notifications_flowpanel.Width - 12, 0);
                notification.titleText = title;
                notification.Image = image;
                notification.bodyText = body;
                notification.NotificationClosed += NotificationClosed;
            return notification;
        }
        private void NotificationClosed(object sender, EventArgs e)
        {
            NotificationsCount--;
            if (NotificationsCount > 0)
                notifications_label.Text = "Notifications (" + NotificationsCount.ToString() + ")";
            else
                notifications_label.Text = "Notifications";
        }
        void AddNotification(NotificationPanel notification)
        {
            NotificationsCount++;
            notifications_label.Text = "Notifications (" + NotificationsCount.ToString() + ")";
            notifications_flowpanel.Controls.Add(notification);
        }

        /// <summary>
        /// Gets message envelopes for the given folder and adds them to the corresponding flow_panel.
        /// </summary>
        void updateFolderView(int folderIndex, bool update)
        {
            EmailClient.SetActiveFolder(folderIndex);

            // Getting panel
            var panel = EnvelopeFlowPanels[folderIndex];

            // If list is empty or update is true, repopulate list.
            if(panel.ListSize == 0 | update == true)
            {
                panel.Clear();

                // The internal index value of the envelope will match the index in the envelopePanels list
                int index = -1;

                var envelopes = EmailClient.GetFolderEnvelopes(folderIndex);
                foreach (var envelope in envelopes)
                {
                    index++;
                    panel.Add(index, envelope.from.ToString(),
                                     envelope.date.ToString(),
                                     envelope.subject,
                                     envelope.read);
                }
            }

            if(activePanel != null)
                activePanel.Visible = false;
            panel.Visible = true;
            activePanel = panel;
        }

        /// <summary>
        /// Updates the folder listbox
        /// </summary>
        void updateFoldersView()
        {
            folders_lisbox.Items.Clear();
            foreach (var folderName in EmailClient.GetFoldernames())
            {
                // Folders list
                folders_lisbox.Items.Add(folderName);

                // Adding flow panel to list
                var flowPanel = new EnvelopeFlowPanel();
                    flowPanel.Visible = false;
                    flowPanel.Size       = template_flow_panel.Size;
                    flowPanel.Dock       = template_flow_panel.Dock;
                    flowPanel.Location   = template_flow_panel.Location;
                    flowPanel.AutoScroll = template_flow_panel.AutoScroll;
                    flowPanel.Parent     = template_flow_panel.Parent;
                    flowPanel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                    flowPanel.BringToFront();
                EnvelopeFlowPanels.Add(flowPanel);

                // Move button list
                ToolStripButton folderButton = new();
                folderButton.Text = folderName;
                folderButton.Click += folder_move_button_Click;
                move_message_dropdown.DropDownItems.Add(folderButton);
            }


        }

        private void EnvelopePanel_MessageOpen(object sender, EventArgs e)
        {
            var envelope = (EnvelopePanel)sender;

            var message = EmailClient.OpenMessage(envelope.index);
            msg_from_label.Text = message.from;
            if(message.cc != null & message.cc!.Length > 0)
            {
                msg_cc_label.Visible = true;
                msg_padding_panel2.Visible = true;
                msg_cc_label.Text = "cc: " + message.cc!.ToString();
            }
            else
            {
                msg_cc_label.Visible = false;
                msg_padding_panel2.Visible = false;
            }
            msg_subject_label.Text = message.subject;
            msg_body_rtextbox.Text = message.body;

            //Debug.WriteLine("Opening message " + envelope.index.ToString() + ": " 
            //              + message.from + message.subject + message.body);

            main_tab.SelectedTab = open_message_tab;
        }

        private void toggle_read_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripButton button = (ToolStripButton)sender;
            tempDisableButton(button, 0.2);

            var indices = activePanel.ToggleReadSelected();

            foreach(var index in indices)
            {
                EmailClient.ToggleRead(index);
            }
            this.Cursor = Cursors.Default;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var deleted_indices = activePanel.DeleteSelected();
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
                msg_from_label.MaximumSize    = new Size(msg_senderinfo_panel.Width - 10, 0);
                msg_cc_label.MaximumSize      = new Size(msg_senderinfo_panel.Width - 10, 0);
                msg_subject_label.MaximumSize = new Size(msg_senderinfo_panel.Width - 10, 0);
                msg_senderinfo_panel.Size = new Size(msg_senderinfo_panel.Width,
                                                     msg_from_label.Height
                                                   + msg_subject_label.Height
                                                   + msg_senderinfo_padding_panel1.Height);
            }
        }

        private void new_message_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text        = "";
            cmp_cc_textbox.Text        = "";
            cmp_bcc_textbox.Text       = "";
            cmp_subject_textbox.Text   = "";
            cmp_mailbody_rtextbox.Text = "";
            activePanel!.ClearSelection();
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
                    NotificationPanel notification;
                    if(ex.Data.Count > 0)
                    {
                        notification = prepareNotification("Multi send Failed!",
                                            Properties.Resources.ErrorAnimatedIcon,
                                            ex.Data["Multi receiver email failed"].ToString() + "\n"
                                            + "Message sent to <" + message.to + "> with "
                                            + " subject \"" + message.subject + "\" failed\n"
                                            + "\nClick to retry. Error:\n" + ex.Message);;
                    }
                    else
                    {
                        notification = prepareNotification("Send Failed!",
                                            Properties.Resources.ErrorAnimatedIcon,
                                            "Message sent to <" + message.to + "> with "
                                          + " subject \"" + message.subject + "\" failed\n"
                                          + "\nClick to retry. Error:\n" + ex.Message);
                    }

                    notification.Object = message;
                    notification.NotificationBodyClicked += LoadMessageNotificationMessage;
                    AddNotification(notification);
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
        private void LoadMessageNotificationMessage(object sender, EventArgs e)
        {
            var message = (IEmail.Message)sender;
            LoadMessage(message);
        }

        private void msg_reply_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text        = trimFromToEmail(msg_from_label.Text);
            cmp_subject_textbox.Text   = "RE: " + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n\n________________\n"
                                             + "Previous Message:"
                                             + "\nFrom: " + msg_from_label.Text;

            if (msg_cc_label.Text.Length > 0)
            {
                cmp_mailbody_rtextbox.Text += "\n" + msg_cc_label.Text;
            }
            else
            {
                cmp_cc_textbox.Text = string.Empty;
            }

            cmp_mailbody_rtextbox.Text += "\nSubject: " + msg_subject_label.Text
                                         + "\n\n" + msg_body_rtextbox.Text;

            activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;
        }

        private void refresh_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var button = (ToolStripButton)sender;
            if (button.Owner.InvokeRequired)
            {
                Action safeRefresh = delegate { refresh_button_Click(sender, e); };
                button.Owner.Invoke(safeRefresh);
            }
            else
            {
                button.Image = Properties.Resources.RefreshAnimatedIcon;
                button.Text = "Refreshing";
                tempDisableButton(button, -1);

                EmailClient.UpdateInboxAsync();
            }
            this.Cursor = Cursors.Default;
        }
        private void InboxUpdateFinishedHandler(object sender, EventArgs e)
        {
            var index = (int)sender;

            if (refresh_button.Owner.InvokeRequired)
            {
                Action safeInboxHandler = delegate { InboxUpdateFinishedHandler(sender, e); };
                refresh_button.Owner.Invoke(safeInboxHandler);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                reenableButton(refresh_button);
                refresh_button.Text = "Refresh";
                refresh_button.Image = Properties.Resources.RefreshIcon;

                // Selecting and updating inbox
                folders_lisbox.ClearSelected();
                folders_lisbox.SetSelected(index, true);
                updateFolderView(index, true);

                this.Cursor = Cursors.Default;
            }
        }

        private void msg_forward_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text = string.Empty;
            cmp_subject_textbox.Text = "FW: " + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n\n________________\n"
                                             + "Forwarded Message:"
                                             + "\nFrom: " + msg_from_label.Text;

            if (msg_cc_label.Text.Length > 0)
                cmp_mailbody_rtextbox.Text += "\n" + msg_cc_label.Text;

            cmp_mailbody_rtextbox.Text += "\nSubject: " + msg_subject_label.Text
                                        + "\n\n" + msg_body_rtextbox.Text;

            activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;

        }

        private void msg_replyall_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text = trimFromToEmail(msg_from_label.Text);
            if(!cmp_cc_panel.Visible)
                add_cc_menuitem_Click(null, EventArgs.Empty);
            cmp_subject_textbox.Text = "RE: " + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n\n________________\n"
                                             + "Previous Message:"
                                             + "\nFrom: " + msg_from_label.Text;

            if (msg_cc_label.Text.Length > 0)
                cmp_mailbody_rtextbox.Text += "\n" + msg_cc_label.Text;

            cmp_mailbody_rtextbox.Text += "\nSubject: " + msg_subject_label.Text
                                        + "\n\n" + msg_body_rtextbox.Text;
            activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;

        }

        private void folder_move_button_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            Debug.WriteLine("Folder name click: " + button.Text);
        }

        private void folders_lisbox_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int folderIndex = folders_lisbox.SelectedIndex;

            // Clearing selection
            EnvelopeFlowPanels[folderIndex].ClearSelection();

            updateFolderView(folderIndex, false);

            this.Cursor = Cursors.Default;
        }
    }
}