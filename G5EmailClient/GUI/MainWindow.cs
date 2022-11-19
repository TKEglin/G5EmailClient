
using System.Diagnostics;
using System.Timers;
using System.Threading;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{

    public partial class MainWindow : FormDragBase
    {
        IEmail EmailClient;
        List<IEmail> EmailClientsList = new();

        int NotificationsCount = 0;

        // This list will be used to smoothly switch between folders
        List<EnvelopeFlowPanel> EnvelopeFlowPanels = new();
        EnvelopeFlowPanel? activePanel;
        EnvelopeFlowPanel  searchPanel;

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            // Saving email client to local variable
            EmailClient = ParamEmailClient;
            EmailClientsList.Add(EmailClient);

            // Opening connection form
            this.Visible = false;
            Application.Run(new ConnectionForm(EmailClient, true));
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
                // Search panel
            searchPanel = new EnvelopeFlowPanel();
                searchPanel.isCopyPanel = true;
                searchPanel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                searchPanel.Visible = false;
                searchPanel.Size = template_flow_panel.Size;
                searchPanel.Dock = template_flow_panel.Dock;
                searchPanel.Location = template_flow_panel.Location;
                searchPanel.AutoScroll = template_flow_panel.AutoScroll;
                searchPanel.Parent = template_flow_panel.Parent;
                searchPanel.BringToFront();
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
                initializeFoldersView();
                updateFolderView(0, true, true);
                searchPanel.sourcePanel = EnvelopeFlowPanels[0];
                active_email_label.Text = EmailClient.GetActiveUser().username;
                connected_users_listbox.Items.Add(EmailClient.GetActiveUser().username);
                // Events
                EmailClient.SentMessage += SentMessageHandler;
                EmailClient.FolderUpdateFinished += FolderUpdateFinishedHandler;
                EmailClient.MoveMessageFinished += MoveMessageFinishedHandler;

                // Setting inbox as default selected folder
                folders_lisbox.SetSelected(0, true);
            }
        }

        #region utility functions
        // This timer will be used for the button functions.
        // It is declared here so it can be reset by new calls of DisableButton
        System.Timers.Timer buttonTimer = new(1000) { Enabled = false };
        /// <summary>
        /// Temporarily disables a button for time 'double seconds'.
        /// If seconds <= 0, the button will be disabled until reenabled
        /// using reenableButton();
        /// </summary>
        /// <param name="button"></param>
        void DisableButton(ToolStripButton button, double seconds)
        {
            if(button.Owner.InvokeRequired)
            {
                Action safeDisable = delegate { DisableButton(button, seconds); };
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

        #endregion

        /// <summary>
        /// Gets message envelopes for the given folder and adds them to the corresponding flow_panel.
        /// </summary>
        void updateFolderView(int folderIndex, bool update, bool switchView)
        {
            this.Cursor = Cursors.WaitCursor;
            // If set active fails, the folder cannot be updated.
            if (EmailClient.LoadSetActiveFolder(folderIndex) < 0)
                return;

            // Getting panel
            var panel = EnvelopeFlowPanels[folderIndex];

            // If update is true, repopulate list.
            if(update == true)
            {
                panel.Clear();

                var envelopes = EmailClient.GetFolderEnvelopes(folderIndex);
                foreach (var envelope in envelopes)
                {
                    panel.Add(envelope.UID, envelope.from,
                                            envelope.date,
                                            envelope.subject,
                                            envelope.read);
                }

                // Preloading the first 20 messages
                for(int i = 0; i < 20 & i < envelopes.Count; i++)
                {
                    EmailClient.PreloadMessage(folderIndex, panel[i].UID);
                }
            }

            if(switchView)
            {
                panel.Visible = true;
                panel.BringToFront();
                activePanel = panel;
            }
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Updates the folder listbox
        /// </summary>
        void initializeFoldersView()
        {
            folders_lisbox.Items.Clear();

            // The folder index is saved as a tag in each button
            int index = -1;

            foreach (var folderName in EmailClient.GetFoldernames())
            {
                // Folders list
                folders_lisbox.Items.Add(folderName);

                // Adding flow panel to list
                var flowPanel = new EnvelopeFlowPanel();
                    flowPanel.Visible = false;
                    flowPanel.Dock         =         template_flow_panel.Dock;
                    flowPanel.Dock         =         template_flow_panel.Dock;
                    flowPanel.Parent       =       template_flow_panel.Parent;
                    flowPanel.Location     =     template_flow_panel.Location;
                    flowPanel.AutoSize     =     template_flow_panel.AutoSize;
                    flowPanel.AutoScroll   =   template_flow_panel.AutoScroll;
                    flowPanel.MinimumSize  =  template_flow_panel.MinimumSize;
                    flowPanel.AutoSizeMode = template_flow_panel.AutoSizeMode;
                    flowPanel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                    flowPanel.BringToFront();
                EnvelopeFlowPanels.Add(flowPanel);

                // Move button list
                index++;
                ToolStripButton folderButton = new();
                folderButton.Text = folderName;
                folderButton.Tag = index;
                folderButton.Click += folder_move_button_Click;
                move_message_dropdown.DropDownItems.Add(folderButton);
            }
        }

        private void EnvelopePanel_MessageOpen(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var envelope = (EnvelopePanel)sender;

            var message = EmailClient.OpenMessage(envelope.UID);
            msg_from_label.Text = message.from;
            if(message.cc != null & message.cc.Length > 0)
            {
                msg_cc_label.Visible = true;
                msg_padding_panel2.Visible = true;
                msg_cc_label.Text = "cc: " + message.cc!.ToString();
            }
            else
            {
                msg_cc_label.Visible = false;
                msg_padding_panel2.Visible = false;
                msg_cc_label.Text = "cc: ";
            }
            msg_subject_label.Text = message.subject;
            msg_body_rtextbox.Text = message.body;

            //Debug.WriteLine("Opening message " + envelope.index.ToString() + ": " 
            //              + message.from + message.subject + message.body);

            main_tab.SelectedTab = open_message_tab;

            this.Cursor = Cursors.Default;
        }

        private void toggle_read_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripButton button = (ToolStripButton)sender;
            DisableButton(button, 0.2);

            var UIDs = activePanel!.ToggleReadSelected();

            foreach(var UID in UIDs)
            {
                EmailClient.ToggleRead(UID);
            }
            this.Cursor = Cursors.Default;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var deleted_UIDs = activePanel!.DeleteSelected();
            foreach (var UID in deleted_UIDs)
            {
                EmailClient.Delete(UID);
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
        }

        private void add_cc_menuitem_Click(object sender, EventArgs e)
        {
            cmp_cc_panel.Visible = !cmp_cc_panel.Visible;
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
                if(cmp_cc_panel.Visible)
                    message.cc  = cmp_cc_textbox.Text;
                if (cmp_bcc_panel.Visible)
                    message.bcc = cmp_bcc_textbox.Text;
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
        private void SentMessageHandler(Exception? ex, IEmail.Message message)
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
                                            + "\nClick to retry. Error:\n" + ex.Message);
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
                DisableButton(button, -1);

                EmailClient.UpdateInboxAsync();
            }
            this.Cursor = Cursors.Default;
        }
        private void FolderUpdateFinishedHandler(object sender, EventArgs e)
        {
            var index = (int)sender;

            if (refresh_button.Owner.InvokeRequired)
            {
                Action safeInboxHandler = delegate { FolderUpdateFinishedHandler(sender, e); };
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

                updateFolderView(index, true, true);

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
            if (msg_cc_label.Text.Length > 4)
            {
                cmp_cc_textbox.Text = msg_cc_label.Text.Remove(0, 4);
                cmp_cc_panel.Visible = true;
            }
            else 
                cmp_cc_panel.Visible = false;

            if(!cmp_cc_panel.Visible)
                add_cc_menuitem_Click(sender, EventArgs.Empty);
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

            var folderIndex = (int)button.Tag;

            // The messages to be moved will be deleted from the active panel
            var messageUIDs = activePanel!.DeleteSelected();

            foreach(var messageUID in messageUIDs)
            {
                EmailClient.MoveMessage(messageUID, folderIndex);
            }

            Debug.WriteLine("Folder with index " + button.Tag + " and name " + button.Text + " selected for move.");
        }
        private void MoveMessageFinishedHandler(string UID, int folderIndex, IEmail.Message Envelope, 
                                                bool seen, bool succes, Exception? ex)
        {
            var panel = EnvelopeFlowPanels[folderIndex];

            if (panel.InvokeRequired)
            {
                Action safeMoveHandler = delegate { MoveMessageFinishedHandler(UID, folderIndex, Envelope, 
                                                                               seen, succes, ex); };
                panel.Invoke(safeMoveHandler);
            }
            else
            {
                if(!succes & ex != null)
                {
                    MoveMessageFailed(UID, folderIndex, ex);
                }
                // Panel will be origin folder if operation is failed. Otherwise, destination index
                panel.Add(UID, Envelope.from,
                               Envelope.date,
                               Envelope.subject,
                               seen);
            }
        }
        private void MoveMessageFailed(string UID, int folderIndex, Exception ex)
        {
            if(notifications_flowpanel.InvokeRequired)
            {
                Action safeHandler = delegate { MoveMessageFailed(UID, folderIndex, ex); };
                notifications_flowpanel.Invoke(safeHandler);
            }
            else
            {
                var notification = prepareNotification("Move message failed.",
                                       Properties.Resources.ErrorAnimatedIcon,
                                       "Move message failed. Message moved back to origin folder.\n\n"
                                     + "Error message: \n" + ex!.Message);
                AddNotification(notification);
            }
        }

        private void folders_lisbox_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            int folderIndex = folders_lisbox.SelectedIndex;

            search_textbox.Text = string.Empty;
            if(activePanel!.envelopesHidden)
                activePanel!.ShowAll();

            // Clearing selection
            var FlowPanel = EnvelopeFlowPanels[folderIndex];
            FlowPanel.ClearSelection();

            // Setting to update folder if necesarry
            updateFolderView(folderIndex, false, true);

            this.Cursor = Cursors.Default;
        }

        // Used to return to the previous panel when search is cleared
        private void search_button_Click(object sender, EventArgs e)
        {
            if (search_textbox.Text.Length == 0) return;

            if(activePanel != null)
                activePanel!.ClearSelection();

            IEmail.SearchFlags flags = IEmail.SearchFlags.Empty;
            if (search_sender_checkbox.Checked)  flags |= IEmail.SearchFlags.From;
            if (search_subject_checkbox.Checked) flags |= IEmail.SearchFlags.Subject;
            if (search_body_checkbox.Checked)    flags |= IEmail.SearchFlags.Body;
            if (search_cc_checkbox.Checked)      flags |= IEmail.SearchFlags.Cc;
            if (flags == IEmail.SearchFlags.Empty) return;

            this.Cursor = Cursors.WaitCursor;

            var envelopes = EmailClient.SearchFolder(search_textbox.Text, flags);

            activePanel!.HideRest(envelopes.UIDs);

            search_textbox.Text = " " + envelopes.UIDs.Count.ToString() + " messages found. Click here to clear.";

            this.Cursor = Cursors.Default;
        }

        private void search_textbox_Enter(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text = string.Empty;

            if(activePanel!.envelopesHidden)
                activePanel!.ShowAll();   
        }

        private void search_textbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) search_button_Click(sender, e);
        }

        private void search_settings_button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (!search_settings_panel.Visible)
            {
                button.BackgroundImage = Properties.Resources.UpArrowIcon;
                search_settings_panel.Visible = true;
            }
            else
            {
                button.BackgroundImage = Properties.Resources.Settings_Icon;
                search_settings_panel.Visible = false;
            }
        }

        private void search_subject_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            // Searching body also searches subject, so body
            // search is disabled when subject is disabled
            if(search_subject_checkbox.Checked)
            {
                search_body_checkbox.Enabled = true;
            }
            else
            {
                search_body_checkbox.Checked = false;
                search_body_checkbox.Enabled = false;
            }
        }

        ConnectionForm connectionForm;
        bool connectionFormOpen = false;
        private void add_user_button_Click(object sender, EventArgs e)
        {
            if (connectionFormOpen) return;

            IEmail newClient = new MailKitEmail();
            newClient.SetDatabase(EmailClient.GetDatabase());

            connectionForm = new ConnectionForm(newClient, false);
            connectionForm.Show();
            connectionForm.connectionSuccesful += connectionFormSuccesHandler;
            connectionForm.FormClosed += connectionFormClosed;
            connectionFormOpen = true;


        }
        private void connectionFormSuccesHandler(object sender, EventArgs e)
        {
            var newClient = (IEmail)sender;

            if (newClient.isConnected())
            {
                EmailClientsList.Add(newClient);
                var username = newClient.GetActiveUser().username;
                active_email_label.Text = username;
                connected_users_listbox.Items.Add(username);

                //EmailClient = newClient;
            }
        }
        private void connectionFormClosed(object sender, EventArgs e)
        {
            connectionFormOpen = false;
        }

        private void user_settings_button_Click(object sender, EventArgs e)
        {
            main_tab.SelectedTab = user_settings_tab;
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach(var client in EmailClientsList)
            {
                client.Disconnect();
            }
            this.Cursor = Cursors.Default;
            Application.Exit();
        }
    }
}