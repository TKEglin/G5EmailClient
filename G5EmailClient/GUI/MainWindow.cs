
using System.Diagnostics;
using System.Timers;
using System.Threading;

using G5EmailClient.Email;

namespace G5EmailClient.GUI
{

    public partial class MainWindow : FormDragBase
    {
        public class EmailClient
        {
            public EmailClient(IEmail paramClient)
            {
                Client = paramClient;
            }

            public IEmail Client { get; set; }

            // This list will be used to store and smoothly switch between envelope panels
            public List<EnvelopeFlowPanel> EnvelopeFlowPanels = new();
            public EnvelopeFlowPanel? activePanel;

            // Used to repopulate the folders list when switching
            public List<string> FolderNames = new();
            public bool FoldersInitialized = false;
        }

        EmailClient MainClient;
        List<EmailClient> EmailClientsList = new();

        int NotificationsCount = 0;

        public MainWindow(IEmail ParamEmailClient)
        {
            InitializeComponent();

            // Saving email client to local variable
            MainClient = new EmailClient(ParamEmailClient);
            EmailClientsList.Add(MainClient);

            // Opening connection form
            this.Visible = false;
            Application.Run(new ConnectionForm(MainClient.Client, true));
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
                // Loading panel

            if (MainClient.Client.isConnected())
            {
            // Initializing email data.
                // Events
                MainClient.Client.SentMessage += SentMessageHandler;
                MainClient.Client.FolderUpdateFinished += FolderUpdateFinishedHandler;
                MainClient.Client.MoveMessageFinished += MoveMessageFinishedHandler;
                MainClient.Client.NoTrashFolderDetected += NoTrashFolderHandler;
                refresh_timer.Tick += refresh_button_Click;
                // Data
                initializeFoldersView();
                updateFolderView(0, true, true);

                active_email_label.Text = MainClient.Client.GetActiveUser().username;
                connected_users_listbox.Items.Add(MainClient.Client.GetActiveUser().username);
                // Setting inbox as default selected folder
                folders_lisbox.SetSelected(0, true);
                // Setting first user as connected user
                connected_users_listbox.SetSelected(0, true);
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
        /// This function modifies the appearance of the folders listbox
        /// </summary>
        private void connected_users_DrawItem(object sender, DrawItemEventArgs e)
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
            e.Graphics.DrawString(connected_users_listbox.Items[e.Index].ToString(), e.Font, Brushes.Black, e.Bounds, StringFormat.GenericDefault);
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
            if (MainClient.Client.LoadSetActiveFolder(folderIndex) < 0)
                return;

            // Getting panel
            var panel = MainClient.EnvelopeFlowPanels[folderIndex];

            // If update is true, repopulate list.
            if(update == true)
            {
                int loadAmount = 50;

                var envelopes = MainClient.Client.GetFolderEnvelopes(folderIndex, loadAmount);
                foreach (var envelope in envelopes)
                {
                    panel.Add(envelope.UID, envelope.from,
                                            envelope.date,
                                            envelope.subject,
                                            envelope.read);

                    // Preloading the loaded message
                    MainClient.Client.PreloadMessage(folderIndex, envelope.UID);
                }
                // If there envelopes list is full, there may be more messages.
                if(envelopes.Count >= loadAmount)
                {
                    panel.AddLoadMorePanel();
                }

                panel.needsUpdate = false;
            }

            if(switchView)
            {
                panel.Visible = true;
                panel.BringToFront();
                MainClient.activePanel = panel;
            }
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Updates the folder listbox
        /// </summary>
        void initializeFoldersView()
        {
            folders_lisbox.Items.Clear();

            // The folder index is saved as a tag in each dropwdown button
            int index = -1;

            foreach (var folderName in MainClient.Client.GetFoldernames())
            {
                index++;

                // Folders list
                folders_lisbox.Items.Add(folderName);
                MainClient.FolderNames.Add(folderName);

                // Adding flow panel to list
                var flowPanel = new EnvelopeFlowPanel();
                    flowPanel.Visible = false;  flowPanel.folderIndex = index;
                    flowPanel.Dock         =         template_flow_panel.Dock;
                    flowPanel.Dock         =         template_flow_panel.Dock;
                    flowPanel.Parent       =       template_flow_panel.Parent;
                    flowPanel.Location     =     template_flow_panel.Location;
                    flowPanel.AutoSize     =     template_flow_panel.AutoSize;
                    flowPanel.AutoScroll   =   template_flow_panel.AutoScroll;
                    flowPanel.MinimumSize  =  template_flow_panel.MinimumSize;
                    flowPanel.AutoSizeMode = template_flow_panel.AutoSizeMode;
                    flowPanel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                    flowPanel.LoadMoreClicked += LoadMoreHandler;
                    flowPanel.BringToFront();
                MainClient.EnvelopeFlowPanels.Add(flowPanel);

                // Move button list
                ToolStripButton folderButton = new();
                folderButton.Text = folderName;
                folderButton.Tag = index;
                folderButton.Click += folder_move_button_Click;
                move_message_dropdown.DropDownItems.Add(folderButton);
            }

            MainClient.FoldersInitialized = true;
        }

        void LoadMoreHandler(object? sender, EventArgs e)
        {
            var flowPanel = (EnvelopeFlowPanel)sender!;
            updateFolderView(flowPanel.folderIndex, true, true);
        }

        void NoTrashFolderHandler(object? sender, EventArgs e)
        {
            Debug.WriteLine("Runnning no trash folder event handler function.");
            var notification = prepareNotification("No trash folder detected!",
                                                    Properties.Resources.ErrorAnimatedIcon,
                                                    "The IMAP server did not indicate a trash " +
                                                    "folder on load. Deleted messages will be " +
                                                    "deleted permanently!");
            AddNotification(notification);
        }

        private void EnvelopePanel_MessageOpen(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var envelope = (EnvelopePanel)sender;

            var message = MainClient.Client.OpenMessage(envelope.UID);
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
                msg_cc_label.Text = "";
            }
            msg_subject_label.Text = message.subject;
            msg_body_rtextbox.Text = message.body;

            // Clearing old and loading new attachments
            msg_attachments_flow_panel.Controls.Clear();
            foreach(string attachment in message.attachments)
            {
                var label = CreateMsgAttachmentLabel(attachment, envelope.UID);
                msg_attachments_flow_panel.Controls.Add(label);
            }

            main_tab.SelectedTab = open_message_tab;

            this.Cursor = Cursors.Default;
        }
        private Label CreateMsgAttachmentLabel(string filename, string UID)
        {
            var label = new Label();
            label.Text = filename;
            label.Font = msg_cc_label.Font;
            label.Dock = DockStyle.Top;
            label.AutoSize = true;
            label.Tag = UID;
            label.MouseEnter += new EventHandler(delegate (Object? sender, EventArgs e)
            {
                Font font = new(label.Font, FontStyle.Underline);
                label.Font = font;
                this.Cursor = Cursors.Hand;
            });
            label.MouseLeave += new EventHandler(delegate (Object? sender, EventArgs e)
            {
                label.Font = msg_cc_label.Font;
                this.Cursor = Cursors.Default;
            });
            label.Click += MsgAttachmentLabel_Click;
            return label;
        }
        private void MsgAttachmentLabel_Click(object? sender, EventArgs e)
        {
            var label = (Label)sender!;
            var fileName = label.Text;
            var UID = label.Tag.ToString()!;
            Debug.WriteLine("Getting attachment with UID " + UID + " and file name " + fileName);

            save_attachment_dialog.FileName = fileName;


            if(save_attachment_dialog.ShowDialog() == DialogResult.OK)
            {
                // Creating a file stream and passing it to the client to get the attached file
                var stream = save_attachment_dialog.OpenFile();
                MainClient.Client.WriteAttachmentToFile(ref stream, UID, fileName);

                stream.Close();
            }
        }

        private void cmp_attach_button_Click(object sender, EventArgs e)
        {
            if (load_attachment_dialog.ShowDialog() == DialogResult.OK)
            {
                var fileName = load_attachment_dialog.SafeFileName;
                var fileStream = load_attachment_dialog.OpenFile();

                var label = CreateCmpAttachmentLabel(fileName, fileStream);
                cmp_attachments_flowpanel.Controls.Add(label);
            }
        }
        private Label CreateCmpAttachmentLabel(string filename, Stream stream)
        {
            var label = new Label();
            label.Text = filename;
            label.Font = msg_cc_label.Font;
            label.Dock = DockStyle.Top;
            label.AutoSize = true;
            label.Tag = stream;
            label.MouseEnter += new EventHandler(delegate (Object? sender, EventArgs e)
            {
                Font font = new(label.Font, FontStyle.Strikeout);
                label.Font = font;
                this.Cursor = Cursors.Hand;
            });
            label.MouseLeave += new EventHandler(delegate (Object? sender, EventArgs e)
            {
                label.Font = msg_cc_label.Font;
                this.Cursor = Cursors.Default;
            });
            label.Click += CmpAttachmentLabel_Click;
            return label;
        }
        private void CmpAttachmentLabel_Click(object? sender, EventArgs e)
        {
            var label = (Label)sender!;
            label.Dispose();
        }

        private void toggle_read_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ToolStripButton button = (ToolStripButton)sender;
            DisableButton(button, 0.3);

            var UIDs = MainClient.activePanel!.ToggleReadSelected();

            foreach(var UID in UIDs)
            {
                MainClient.Client.ToggleRead(UID);
            }
            this.Cursor = Cursors.Default;
        }

        private void delete_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var deleted_UIDs = MainClient.activePanel!.DeleteSelected();

            foreach (var UID in deleted_UIDs)
            {
                MainClient.Client.Delete(UID);
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
            cmp_cc_panel.Visible = false;
            cmp_bcc_textbox.Text       = "";
            cmp_bcc_panel.Visible = false;
            cmp_subject_textbox.Text   = "";
            cmp_mailbody_rtextbox.Text = "";
            MainClient.activePanel!.ClearSelection();
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
                if(cmp_cc_panel.Visible & cmp_cc_textbox.Text.Length > 0)
                    message.cc  = cmp_cc_textbox.Text;
                if (cmp_bcc_panel.Visible & cmp_bcc_textbox.Text.Length > 0)
                    message.bcc = cmp_bcc_textbox.Text;
                message.subject = cmp_subject_textbox.Text;
                message.body    = cmp_mailbody_rtextbox.Text;

            List<(Stream stream, string filename)> attachments = new();

            foreach(Label label in cmp_attachments_flowpanel.Controls)
            {
                attachments.Add(((Stream)label.Tag, label.Text));
            }

            MainClient.Client.SendMessage(message, attachments);

            cmp_to_textbox.Text        = "";
            cmp_cc_textbox.Text        = "";
            cmp_bcc_textbox.Text       = "";
            cmp_subject_textbox.Text   = "";
            cmp_mailbody_rtextbox.Text = "";
            cmp_attachments_flowpanel.Controls.Clear();
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
            // Hiding cc panel
            cmp_cc_textbox.Text = "";
            cmp_cc_panel.Visible = false;

            cmp_to_textbox.Text        = trimFromToEmail(msg_from_label.Text);
            cmp_subject_textbox.Text   = "RE: " + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n\n________________\n"
                                             + "Previous Message:"
                                             + "\nFrom: " + msg_from_label.Text;
            cmp_mailbody_rtextbox.Text += "\nSubject: " + msg_subject_label.Text
                                         + "\n\n" + msg_body_rtextbox.Text;

            MainClient.activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;
        }

        private void refresh_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            Debug.WriteLine("Refreshing inbox");

            var button = refresh_button;
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

                var envelopes = MainClient.Client.GetNewMessageEnvelopes();

                foreach(var envelope in envelopes)
                {
                    var panel = MainClient.EnvelopeFlowPanels[0];
                    panel.AddToFront(envelope.UID, envelope.from,
                                                   envelope.date,
                                                   envelope.subject,
                                                   envelope.read);

                    // Preloading the loaded message
                    MainClient.Client.PreloadMessage(panel.folderIndex, envelope.UID);
                }

                button.Text = "Refresh";
                button.Image = Properties.Resources.RefreshIcon;
                reenableButton(button);
            }
            Debug.WriteLine("Refresh complete");
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

                if(index == 0)
                {
                    reenableButton(refresh_button);
                    refresh_button.Text = "Refresh";
                    refresh_button.Image = Properties.Resources.RefreshIcon;
                }

                Debug.WriteLine("Folder update finished handler running for folder with index " + index);

                // Selecting and updating inbox
                folders_lisbox.ClearSelected();
                folders_lisbox.SetSelected(index, true);

                updateFolderView(index, false, true);

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

            MainClient.activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;
        }

        private void msg_replyall_button_Click(object sender, EventArgs e)
        {
            cmp_to_textbox.Text = trimFromToEmail(msg_from_label.Text);
            if (msg_cc_label.Visible)
            {
                cmp_cc_textbox.Text = msg_cc_label.Text.Remove(0, 4);
                cmp_cc_panel.Visible = true;
            }
            else 
                cmp_cc_panel.Visible = false;

            cmp_subject_textbox.Text = "RE: " + msg_subject_label.Text;
            cmp_mailbody_rtextbox.Text = "\n\n\n________________\n"
                                             + "Previous Message:"
                                             + "\nFrom: " + msg_from_label.Text;

            if (msg_cc_label.Text.Length > 0)
                cmp_mailbody_rtextbox.Text += "\n" + msg_cc_label.Text;

            cmp_mailbody_rtextbox.Text += "\nSubject: " + msg_subject_label.Text
                                        + "\n\n" + msg_body_rtextbox.Text;
            MainClient.activePanel!.ClearSelection();
            main_tab.SelectedTab = compose_message_tab;
        }

        private void folder_move_button_Click(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;

            var folderIndex = (int)button.Tag;

            // The messages to be moved will be deleted from the active panel
            var messageUIDs = MainClient.activePanel!.DeleteSelected();

            foreach(var messageUID in messageUIDs)
            {
                MainClient.Client.MoveMessage(messageUID, folderIndex);
            }

            Debug.WriteLine("Folder with index " + button.Tag + " and name " + button.Text + " selected for move.");
        }
        private void MoveMessageFinishedHandler(string UID, int folderIndex, IEmail.Message Envelope, 
                                                bool seen, bool succes, Exception? ex)
        {
            var panel = MainClient.EnvelopeFlowPanels[folderIndex];

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
            if(MainClient.activePanel!.envelopesHidden)
                MainClient.activePanel!.ShowAll();

            // Clearing selection
            var FlowPanel = MainClient.EnvelopeFlowPanels[folderIndex];
            FlowPanel.ClearSelection();

            // Updating folder view and setting to update if flow panel is empty
            updateFolderView(folderIndex, FlowPanel.ListSize == 0, true);

            this.Cursor = Cursors.Default;
        }

        // Used to return to the previous panel when search is cleared
        private void search_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (search_textbox.Text.Length == 0) return;

            var panel = MainClient.activePanel!;

            if (panel != null)
                panel.ClearSelection();

            IEmail.SearchFlags flags = IEmail.SearchFlags.Empty;
            if (search_subject_checkbox.Checked) flags |= IEmail.SearchFlags.Subject;
            if (search_sender_checkbox.Checked)  flags |= IEmail.SearchFlags.From;
            if (search_body_checkbox.Checked)    flags |= IEmail.SearchFlags.Body;
            if (search_cc_checkbox.Checked)      flags |= IEmail.SearchFlags.Cc;
            if (flags == IEmail.SearchFlags.Empty) return;

            var envelopes = MainClient.Client.SearchFolder(search_textbox.Text, flags);

            while(panel!.hasLoadMorePanel)
            {
                panel!.LoadMorePanel_Click(null, EventArgs.Empty);
                //LoadMoreHandler(MainClient.activePanel, EventArgs.Empty);
            }

            panel!.HideRest(envelopes.UIDs);

            search_textbox.Text = " " + envelopes.UIDs.Count.ToString() + " messages found. Click here to clear.";

            this.Cursor = Cursors.Default;
        }

        private void search_textbox_Enter(object sender, EventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Text = string.Empty;

            if(MainClient.activePanel!.envelopesHidden)
                MainClient.activePanel!.ShowAll();   
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
            newClient.SetDatabase(MainClient.Client.GetDatabase());

            connectionForm = new ConnectionForm(newClient, false);
            connectionForm.Show();
            connectionForm.connectionSuccesful += connectionFormSuccesHandler;
            connectionForm.FormClosed += connectionFormClosed;
            connectionFormOpen = true;
        }
        private void connectionFormSuccesHandler(object sender, EventArgs e)
        {
            var paramClient = (IEmail)sender;
            var newClient = new EmailClient(paramClient);

            if (newClient.Client.isConnected())
            {
                EmailClientsList.Add(newClient);
                var username = newClient.Client.GetActiveUser().username;
                active_email_label.Text = username;
                connected_users_listbox.Items.Add(username);
                // Events
                newClient.Client.SentMessage += SentMessageHandler;
                newClient.Client.FolderUpdateFinished += FolderUpdateFinishedHandler;
                newClient.Client.MoveMessageFinished += MoveMessageFinishedHandler;
                newClient.Client.NoTrashFolderDetected += NoTrashFolderHandler;

                // Setting new client as selected
                connected_users_listbox.SetSelected(connected_users_listbox.Items.Count - 1, true);
                connected_users_listbox_Click(sender, e);

                // A user can now be deleted without depleting the list so
                delete_user_button.Visible = true;
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


        private void connected_users_listbox_Click(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            MainClient = EmailClientsList[connected_users_listbox.SelectedIndex];

            if (!MainClient.FoldersInitialized)
                initializeFoldersView();
            else
            {
                folders_lisbox.Items.Clear();
                foreach(var name in MainClient.FolderNames)
                {
                    folders_lisbox.Items.Add(name);
                }
            }

            var panel = MainClient.EnvelopeFlowPanels[0];

            updateFolderView(0, panel.ListSize == 0, true);
            folders_lisbox.SetSelected(0, true);

            this.Cursor = Cursors.Default;
        }

        private void delete_user_button_Click(object sender, EventArgs e)
        {
            // If there are other users, we can show one of these
            if(connected_users_listbox.Items.Count > 1)
            {
                // Disposing deleted client
                var index = connected_users_listbox.SelectedIndex;

                connected_users_listbox.Items.RemoveAt(index);
                foreach(var panel in EmailClientsList[index].EnvelopeFlowPanels)
                {
                    panel.Dispose();
                }
                EmailClientsList[index].Client.Disconnect();
                EmailClientsList.RemoveAt(index);

                // Updating to show other client
                connected_users_listbox.SelectedIndex = 0;
                connected_users_listbox_Click(null, EventArgs.Empty);

                if(connected_users_listbox.Items.Count < 2)
                {
                    delete_user_button.Visible = false;
                }
            }
            // Otherwise we need to treat it as a logout
            else
            {
                logout_button_Click(null, EventArgs.Empty);
            }
        }

        private void logout_button_Click(object? sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            foreach (var client in EmailClientsList)
            {
                client.Client.Disconnect();
            }

            this.Cursor = Cursors.Default;
            Application.Exit();
        }

        private void add_folder_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var text = new_foldername_textbox.Text;

            new_foldername_textbox.Text = string.Empty;

            if (text.Length <= 0) return;

            if (MainClient.Client.AddFolder(text) == null)
            {
                var index = MainClient.FolderNames.Count;

                folders_lisbox.Items.Add(text);
                MainClient.FolderNames.Add(text);

                // Adding flow panel to list
                var flowPanel = new EnvelopeFlowPanel();
                flowPanel.Visible = false; flowPanel.folderIndex = index;
                flowPanel.Dock = template_flow_panel.Dock;
                flowPanel.Dock = template_flow_panel.Dock;
                flowPanel.Parent = template_flow_panel.Parent;
                flowPanel.Location = template_flow_panel.Location;
                flowPanel.AutoSize = template_flow_panel.AutoSize;
                flowPanel.AutoScroll = template_flow_panel.AutoScroll;
                flowPanel.MinimumSize = template_flow_panel.MinimumSize;
                flowPanel.AutoSizeMode = template_flow_panel.AutoSizeMode;
                flowPanel.EnvelopePanelOpened += EnvelopePanel_MessageOpen;
                flowPanel.LoadMoreClicked += LoadMoreHandler;
                flowPanel.BringToFront();
                MainClient.EnvelopeFlowPanels.Add(flowPanel);

                // Move button list
                ToolStripButton folderButton = new();
                folderButton.Text = text;
                folderButton.Tag = index;
                folderButton.Click += folder_move_button_Click;
                move_message_dropdown.DropDownItems.Add(folderButton);
            }

            this.Cursor = Cursors.Default;
        }

        private void rename_folder_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var index = folders_lisbox.SelectedIndex;
            var newName = new_foldername_textbox.Text;
            var oldName = (string)folders_lisbox.Items[index];

            new_foldername_textbox.Text = string.Empty;

            if (newName.Length <= 0) return;

            if (MainClient.Client.RenameFolder(oldName, newName, index) == null)
            {
                folders_lisbox.Items[index]   = newName;
                MainClient.FolderNames[index] = newName;

                // Move button list
                move_message_dropdown.DropDownItems[index].Text = newName;
            }

            this.Cursor = Cursors.Default;
        }

        private void delete_folder_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            var index = folders_lisbox.SelectedIndex;
            var oldName = (string)folders_lisbox.Items[index];

            if (MainClient.Client.DeleteFolder(oldName, index) == null)
            {
                folders_lisbox.Items.RemoveAt(index);
                MainClient.FolderNames.RemoveAt(index);
                move_message_dropdown.DropDownItems.RemoveAt(index);
            }

            this.Cursor = Cursors.Default;
        }
    }
}