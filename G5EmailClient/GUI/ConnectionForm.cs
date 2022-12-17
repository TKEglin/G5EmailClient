using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using G5EmailClient.Email;
using G5EmailClient.Database;

namespace G5EmailClient.GUI
{
    public partial class ConnectionForm : FormDragBase
    {
        IEmail EmailClient;

        // Server connection information
        IDatabase.User activeUser = new();

        // Used to remove messages after a duration
        System.Timers.Timer timer = new System.Timers.Timer(1000) { Enabled = false };

        bool isInitialForm = false;

        public ConnectionForm(IEmail ParamEmailClient, bool isInitialFormParam)
        {
            InitializeComponent();

            // Making the window draggable when clicking on the given objects
            AddDraggingControl(big_logo_box);
            AddDraggingControl(server_connect_panel);
            AddDraggingControl(login_panel);
            AddDraggingControl(saved_users_panel);

            // Saving email client to local variable
            EmailClient = ParamEmailClient;
            isInitialForm = isInitialFormParam;

            // Populating saved users list
            foreach(var username in EmailClient.GetUsernames())
            {
                saved_users_list.Items.Add(username);
            }

            // Getting and applying default user information
            activeUser = EmailClient.GetDefaultUser();
            updateInterface();

            // Miscellaneous GUI intialization
            save_button_tooltip.SetToolTip(save_user_button,
                "Saves the information currently typed into the text fields. " +
                "Only one user can be saved for any given username.\n" +
                "To overwrite a user, simply save a user with the same username.\n" +
                "To save a user as default, so it is loaded when the client launches, " +
                "select a user and load it.");
        }

        /// <summary>
        /// Updates the values of all text boxes
        /// </summary>
        void updateInterface()
        {
            IMAP_hostname_textbox.Text = activeUser.IMAP_hostname;
            IMAP_port_box.Value        = activeUser.IMAP_port;
            SMTP_hostname_textbox.Text = activeUser.SMTP_hostname;
            SMTP_port_box.Value        = activeUser.SMTP_port;
            username_textbox.Text = activeUser.username;
            password_textbox.Text = activeUser.password;
        }

        #region Message display utillity functions
        /// <summary>
        /// Shows a message on the given label. 
        /// If seconds is zero or less, the message will be shown indefinitely.
        /// </summary>
        private void showMessage(Label label, string message, double seconds, Color color)
        {
            // The program will throw an exception if the label is accesed from
            // a thread that did not create it, so we might need to use the invoke method
            if (label.InvokeRequired)
            {
                Action safeShow = delegate { showMessage(label, message, seconds, color); };
                label.Invoke(safeShow);
            }
            else
            {
                label.Text = message;
                label.ForeColor = color;
                label.Visible = true;

                if(seconds > 0)
                {                
                    // Creating a timer to remove the message after a short time
                    timer.Interval = seconds * 1000;
                    timer.Enabled = true;
                    timer.Elapsed += (sender, args) =>
                    {
                        unshowMessage(label);
                        timer.Enabled = false;
                    };
                }

            }
        }

        /// <summary>
        /// Thread safe function that hides the given label.
        /// </summary>
        private void unshowMessage(Label label)
        {
            if (label.InvokeRequired)
            {
                Action safeUnshow = delegate { unshowMessage(label); };
                label.Invoke(safeUnshow);
            }
            else
            {
                // Hiding tooltip so new messages don't show old tooltips.
                message_label_tooltip.Active = false;
                // Hiding label.
                label.Visible = false;
            }
        }
        #endregion

        #region Input boxes assignment functions
        private void IMAP_hostname_textbox_TextChanged(object sender, EventArgs e)
        {
            activeUser.IMAP_hostname = IMAP_hostname_textbox.Text;
        }

        private void SMTP_hostname_textbox_TextChanged(object sender, EventArgs e)
        {
            activeUser.SMTP_hostname = SMTP_hostname_textbox.Text;
        }

        private void IMAP_port_box_ValueChanged(object sender, EventArgs e)
        {
            activeUser.IMAP_port = (int)IMAP_port_box.Value;
        }

        private void SMTP_port_box_ValueChanged(object sender, EventArgs e)
        {
            activeUser.SMTP_port = (int)SMTP_port_box.Value;
        }

        private void username_textbox_TextChanged(object sender, EventArgs e)
        {
            activeUser.username = username_textbox.Text;
        }

        private void password_textbox_TextChanged(object sender, EventArgs e)
        {
            activeUser.password = password_textbox.Text;
        }
        #endregion

        private void close_button_Click(object sender, EventArgs e)
        {
            EmailClient.Disconnect();
            if (isInitialForm)
                Application.Exit();
            else
                this.Close();
        }

        private void connect_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Exception? attempt = EmailClient.Connect(activeUser.IMAP_hostname, activeUser.IMAP_port,
                                                     activeUser.SMTP_hostname, activeUser.SMTP_port);
            if (attempt == null) // Success
            {
                // If success, enable login controls:
                server_connect_panel.Visible = false;
                connect_button.Visible       = false;
                login_panel.Visible      = true;
                login_button.Visible     = true;
                new_host_button.Visible  = true;
                save_user_button.Visible = true;
                // In case an error message is being diplayed:
                unshowMessage(message_label);
            }
            else // Error
            {
                // Showing message to user
                showMessage(message_label, "Connection error. Move mouse here for details.", 0, Color.Red);
                // Updating tooltip
                message_label_tooltip.Active = true;
                message_label_tooltip.SetToolTip(message_label, attempt.ToString());
            }
            this.Cursor = Cursors.Default;
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Exception? attempt = EmailClient.Authenticate(activeUser.username, activeUser.password);
            if(attempt == null) // Success
            {
                if(!isInitialForm) connectionSuccesful(EmailClient, e);
                this.Close();
            }
            else // Error
            {
                // Showing message to user
                showMessage(message_label, "Authentication error. Move mouse here for details.", 0, Color.Red);
                // Updating tooltip
                message_label_tooltip.Active = true;
                message_label_tooltip.SetToolTip(message_label, attempt.ToString());
            }
            this.Cursor = Cursors.Default;
        }
        public event EventHandler connectionSuccesful;

        private void message_label_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(message_label_tooltip.GetToolTip(message_label));
            showMessage(message_label, "Error text copied!", 1.5, Color.Black);
        }

        private void new_host_button_Click(object sender, EventArgs e)
        {
            EmailClient.Disconnect();
            server_connect_panel.Visible = true;
            connect_button.Visible       = true;
            login_panel.Visible      = false;
            login_button.Visible     = false;
            new_host_button.Visible  = false;
            save_user_button.Visible = false;
        }

        private void users_button_Click(object sender, EventArgs e)
        {
            saved_users_panel.Visible = !saved_users_panel.Visible;
        }

        private void delete_user_button_Click(object sender, EventArgs e)
        {
            string? selectedUsername = saved_users_list.SelectedItem?.ToString();
            if(selectedUsername == null)
            {
                showMessage(message_label, "Please select a user from the list.", 1.5, Color.Black);
            }
            else
            {
                if (EmailClient.DeleteUser(selectedUsername) == 1) // If delete successful
                {
                    // Remove the username from the list
                    saved_users_list.Items.Remove(saved_users_list.SelectedItem!);

                    // If the deleted user is the default user, default user must be updated
                    if(selectedUsername == EmailClient.GetDefaultUser().username)
                    {
                        EmailClient.SetDefaultUser("");
                    }
                }
            }
        }

        private void save_user_button_Click(object sender, EventArgs e)
        {
            // Calling SaveUser() function:
            if(EmailClient.SaveUser(activeUser) == 1)
            {
                showMessage(message_label, "User overwritten.", 1.5, Color.Black);
            }
            else
            {
                // If user is not overwritten, we can add the username to the list.
                saved_users_list.Items.Add(activeUser.username);
                showMessage(message_label, "New user saved.", 1.5, Color.Black);
            }
        }

        private void load_user_button_Click(object sender, EventArgs e)
        {
            string? selectedUsername = saved_users_list.SelectedItem?.ToString();
            if (selectedUsername == null)
            {
                showMessage(message_label, "Please select a user from the list.", 1.5, Color.Black);
            }
            else
            {
                showMessage(message_label, "User loaded.", 1.5, Color.Black);
                // Disconnecting in case server info is different
                new_host_button_Click(sender, e);

                // Updating info
                EmailClient.SetDefaultUser(selectedUsername);
                activeUser = EmailClient.GetUser(selectedUsername)!;
                updateInterface();
            }
        }

        private void password_show_label_Click(object sender, EventArgs e)
        {
            password_textbox.UseSystemPasswordChar = !password_textbox.UseSystemPasswordChar;
            if(password_textbox.UseSystemPasswordChar)
            {
                password_show_label.Text = "Show";
            }
            else
            {
                password_show_label.Text = "Hide";
            }
        }

        private void password_show_label_MouseEnter(object sender, EventArgs e)
        {
            password_show_label.ForeColor = SystemColors.ActiveBorder;
        }

        private void password_show_label_MouseLeave(object sender, EventArgs e)
        {
            password_show_label.ForeColor = SystemColors.ControlText;
        }
    }
}
