using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Diagnostics;
// Email handlers
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
// Database
using G5EmailClient.Database;

namespace G5EmailClient.Email
{
    public class MailKitEmail : IEmail
    {
        public class EmailFolder
        {
            public EmailFolder(IMailFolder folder, string name) { Folder = folder;
                                                                  FolderName = name; }

            public IMailFolder? Folder { get; set; }
            public bool isLoaded { get; set; } = false;

            public string? FolderName;
            public List<MimeMessage> Messages { get; set; } = new();
            public List<UniqueId>    UIDs     { get; set; } = new();
            public List<bool>        Seen     { get; set; } = new();
        }

        IDatabase data = new JSONDatabase();

        Mutex ImapMutex = new();
        Mutex SmtpMutex = new();

        ImapClient imapClient = new();
        SmtpClient smtpClient = new();

        IDatabase.User activeUser = new();

        List<EmailFolder> emailFolders = new();
        EmailFolder? activeFolder;

        public MailKitEmail()
        {
            // Initalizing class data variables.
            List<IDatabase.User> users = data.GetUsers();
        }

        /// <summary>
        /// This function is called when connection is established and the user is authenticated.
        /// </summary>
        void initialise()
        {
            initializeFolders();
            updateFolder(0); // Updating inbox
        }

         //_________________
        // Utility functions
        #region utility functions

        /// <summary>
        /// Splits and interprets a string of emails. The character ',' is used a separator.
        /// All whitespace in the emails will be ignored.
        /// </summary>
        /// <returns>A list of email addresses. 
        /// Returns null if string is empty or an error in an email is detected.</returns>
        List<string> SplitEmails(string email_string)
        {
            List<string> result = new();
            Debug.WriteLine("String pre split: " + email_string);
            string[] emails = email_string.Split(',');

            foreach(var InputEmail in emails)
            {
                Debug.WriteLine("Email pre replace: " + InputEmail);
                var OutputEmail = InputEmail;
                OutputEmail = InputEmail.Replace(",", "");
                OutputEmail = OutputEmail.Replace(" ", "");
                Debug.WriteLine("Email post replace: " + OutputEmail);
                result.Add(OutputEmail);
            }

            return result;
        }

        #endregion

        //_____________________________________
        // Functions that access the imap server
        #region IMAP server retrieval functions

        /// <summary>
        /// Initializes the folders for the server.
        /// </summary>
        private void initializeFolders()
        {
            // Adding inbox at index 0
            emailFolders.Add(new EmailFolder(imapClient.Inbox, imapClient.Inbox.Name));
            string inbox_name = imapClient.Inbox.Name;

            // Adding folders
            var folders = imapClient.GetFolders(imapClient.PersonalNamespaces[0], false);
            foreach (var Folder in folders)
            {
                // If statement to avoid adding the inbox twice
                if (Folder.Name != inbox_name & Folder.Exists)
                    emailFolders.Add(new EmailFolder(Folder, Folder.Name));
            }

            FolderRemainingMoveTasks = new (new int[emailFolders.Count]);
        }

        /// <summary>
        /// Sets as active and updates the given folder. This function should be called
        /// everytime a new folder is opened, and intermittently to check for new messages.
        /// </summary>
        /// <param name="folder"></param>
        /// 
        private void updateFolder(int folderIndex)
        {
            ImapMutex.WaitOne();
  
            var folder = emailFolders[folderIndex];

            // This will be used to remove duplicates once the new messages have been opened.
            int count = folder.Folder!.Count;
            // This is necessary to let the user access old messages while inbox is updating.

            folder!.Folder!.Open(FolderAccess.ReadWrite);

            // Adding the messages to the given folder
            foreach (var message in folder.Folder)
            {
                Debug.WriteLine("Adding message with subject \"" + message.Subject 
                              + "\" to folder " + folder.FolderName);
                folder.Messages.Add(message);
            }
            foreach (var items in folder.Folder.Fetch(0, -1, 
                                                       MessageSummaryItems.UniqueId |
                                                       MessageSummaryItems.Flags))
            {
                Debug.WriteLine("Fetching summary items from folder " + folder.FolderName);
                folder.UIDs.Add(items.UniqueId);
                folder.Seen.Add(items.Flags!.Value.HasFlag(MessageFlags.Seen));
            }

            // For some reason inbox messages are listed in reverse (for gmail at least)
            // so it will be reversed here. Note: if tests show this is not consistent for other
            // email servers, we will have to implement sort here.
            if(folder.FolderName == imapClient.Inbox.Name)
            {
                folder.Messages.Reverse();
                folder.UIDs.Reverse();
                folder.Seen.Reverse();
            }

            ImapMutex.ReleaseMutex();

            folder.isLoaded = true;

            // Deleting duplicates
            if (count >= 0)
            {
                folder.Messages.RemoveRange(0, count);
                folder.UIDs.    RemoveRange(0, count);
                folder.Seen.    RemoveRange(0, count);
            }
            if (FolderUpdateFinished != null)
            {
                this.FolderUpdateFinished(folderIndex, EventArgs.Empty);
            }
        }
        #endregion

         //__________________________________________
        // Functions that deal with server connection
        #region server connection functions
        void IEmail.Disconnect()
        {
            imapClient.Disconnect(true);
            smtpClient.Disconnect(true);
        }

        bool IEmail.isConnected()
        {
            return (imapClient.IsConnected & smtpClient.IsConnected);
        }

        Exception? IEmail.Connect(string IMAP_hostname, int IMAP_port, string SMTP_hostname, int SMTP_port)
        {
            try
            {
                imapClient.Connect(IMAP_hostname, IMAP_port, SecureSocketOptions.SslOnConnect);
                smtpClient.Connect(SMTP_hostname, SMTP_port, SecureSocketOptions.SslOnConnect);
            }
            catch(Exception ex)
            {
                ((IEmail)this).Disconnect();
                return ex;
            }
            activeUser.SMTP_hostname = SMTP_hostname;
            activeUser.SMTP_port     = SMTP_port;
            activeUser.IMAP_hostname = IMAP_hostname;
            activeUser.IMAP_port     = IMAP_port;
            return null;
        }

        Exception? IEmail.Authenticate(string username, string password)
        {
            try
            {
                imapClient.Authenticate(username, password);
                smtpClient.Authenticate(username, password);
            }
            catch(Exception ex)
            {
                return ex;
            }
            activeUser.password = password;
            activeUser.username = username;
            // Preparing main window
            initialise();
            return null;
        }
        #endregion

         //__________________________________
        // Functions that access the database
        #region database functions
        List<string> IEmail.GetUsernames()
        {
            var users = data.GetUsers();
            List<string> usernames = new();
            foreach(var user in users)
            {
                usernames.Add(user.username.ToString());
            }
            return usernames;
        }

        IDatabase.User? IEmail.GetUser(string username)
        {
            return data.GetUser(username);
        }

        IDatabase.User IEmail.GetDefaultUser()
        {
            return data.GetDefaultUser();
        }

        int IEmail.SetDefaultUser(string username)
        {
            return data.SetDefaultUser(username);
        }

        int IEmail.DeleteUser(string username)
        {
            return data.DeleteUser(username);
        }

        int IEmail.SaveUser(IDatabase.User saveUser)
        {
            return data.SaveUser(saveUser);
        }
        #endregion

         //____________________________________________
        // Functions that supply client info to the GUI
        #region client data retrieval functions
        IDatabase.User IEmail.GetActiveUser()
        {
            return activeUser;
        }

        void IEmail.UpdateFolder(int folderIndex)
        {
            updateFolder(folderIndex);
        }

        void IEmail.UpdateFolderAsync(int folderIndex)
        {
            ThreadPool.QueueUserWorkItem(state => updateFolder(folderIndex));
        }

        void IEmail.UpdateInbox()
        {
            updateFolder(0);
        }
        void IEmail.UpdateInboxAsync()
        {
            ThreadPool.QueueUserWorkItem(state => updateFolder(0));
        }
        public event EventHandler FolderUpdateFinished;

        void IEmail.SetActiveFolder(int folderIndex)
        {
            var folder = emailFolders[folderIndex];

            if(!folder.isLoaded)
            {
                updateFolder(folderIndex);
            }

            activeFolder = folder;
        }

        List<(string from, string date, string subject, bool read)> IEmail.GetFolderEnvelopes(int folderIndex)
        {
            List<(string, string, string, bool)> envelopes = new();

            var folder = emailFolders[folderIndex];

            // This list will contain the flags
            if (folder != null)
                // This foreach loop merges the messages and UID lists to
                // add them to the return list in one iteration.
                foreach (var item in folder.Messages.Zip(folder.Seen, 
                                                              (a, b) => new { message = a, seen = b }))
                {
                    envelopes.Add((item.message.From.ToString(), 
                                   item.message.Date.LocalDateTime.ToString(), 
                                   item.message.Subject, 
                                   item.seen));
                }

            return envelopes;
        }

        IEmail.Message? IEmail.OpenMessage(int messageIndex)
        {
            if(messageIndex < activeFolder!.Messages.Count & messageIndex >= 0)
            {
                // Adding read flag
                if (!activeFolder.Seen[messageIndex])
                {
                    ToggleReadQueue.Enqueue(new ToggleReadParameters() {folder = activeFolder!,
                                                                        ID = activeFolder.UIDs[messageIndex],
                                                                        seenFlag = false  });
                    ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
                }
                // Getting message
                var ImapMessage = activeFolder.Messages[messageIndex];
                IEmail.Message message = new();
                    message.date    = ImapMessage.Date.ToString();
                    message.from    = ImapMessage.From.ToString();
                    message.to      = ImapMessage.To.ToString();
                    if (ImapMessage.Cc != null)
                        message.cc      = ImapMessage.Cc.ToString();
                    if (ImapMessage.Subject != null)
                        message.subject = ImapMessage.Subject;
                    if (ImapMessage.TextBody != null)
                        message.body    = ImapMessage.TextBody.ToString();
                return message;
            }
            else
            {
                return null;
            }
            
        }

        List<string> IEmail.GetFoldernames()
        {
            List<string> folderNames = new();
            foreach (var folder in emailFolders)
            {
                folderNames.Add(folder.Folder!.Name);
            }
            return folderNames;
        }
        #endregion

         //__________________________________________
        // Functions that make changes in IMAP server
        #region IMAP server changing functions
        /// <summary>
        /// This class and the corresponding queue is used to execute 
        /// asynchronous toggle read instructions in FIFO order.
        /// </summary>
        public class ToggleReadParameters
        {
            public EmailFolder? folder;
            public UniqueId     ID;
            public bool         seenFlag;
        }
        /// <summary>
        /// ToggleReadParameters must be enqueued before the AsyncToggleRead function is called.
        /// </summary>
        Queue<ToggleReadParameters> ToggleReadQueue = new();

        void IEmail.ToggleRead(int messageIndex)
        {
            Debug.WriteLine("Running MailKitEmail.ToggleRead()");
            var UID = activeFolder!.UIDs[messageIndex];
            var seen = activeFolder.Seen[messageIndex];
            if (!seen)
            {
                activeFolder.Seen[messageIndex] = true; // Updating local data
                Debug.WriteLine("Locally adding flag");
            }  
            else
            {
                activeFolder.Seen[messageIndex] = false;
                Debug.WriteLine("Locally removing flag");
            }
            ToggleReadQueue.Enqueue(new ToggleReadParameters() { folder = activeFolder!,
                                                                 ID = UID, 
                                                                 seenFlag = seen         });
            ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            Debug.WriteLine("MailKitEmail.ToggleRead() complete");
        }
        private void AsyncToggleRead(object? sender, EventArgs e)
        {
            ImapMutex.WaitOne();

            if (ToggleReadQueue.Count > 0)
            {

                // Gettings parameters
                var parameters = ToggleReadQueue.Dequeue();

                if (!parameters.folder!.Folder!.IsOpen)
                    parameters.folder.Folder.Open(FolderAccess.ReadWrite);

                if (!parameters.seenFlag)
                {
                    parameters.folder!.Folder.AddFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to add flag");
                }
                else
                {
                    parameters.folder!.Folder.RemoveFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to remove flag");
                }

                // Recursive call to handle next queued task if necessary
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            }

            ImapMutex.ReleaseMutex();
        }

        void IEmail.Delete(int messageIndex)
        {
            Debug.WriteLine("Running MailKitEmail.Delete()");
            var UID = activeFolder!.UIDs[messageIndex];

            // The local message entry will not be removed
            ThreadPool.QueueUserWorkItem(state => AsyncDelete(activeFolder!, UID));

            Debug.WriteLine("MailKitEmail.Delete() complete");
        }
        private void AsyncDelete(EmailFolder folder, UniqueId ID)
        {
            ImapMutex.WaitOne();

            if(!folder.Folder!.IsOpen)
            {
                folder.Folder.Open(FolderAccess.ReadWrite);
            }

            folder.Folder.AddFlags(ID, MessageFlags.Deleted, true);
            Debug.WriteLine("Server request to add deleted flag");

            ImapMutex.ReleaseMutex();
        }

        // This list is increment when a move task is started. 
        // It will be decremented when a move task is done.
        // The MoveMessageCompleted signal will be sent when the value is 0 for the given folder.
        List<int> FolderRemainingMoveTasks; // = new(emailFolders.Count);
        void IEmail.MoveMessage(int messageIndex, int folderIndex)
        {
            Debug.WriteLine("Running MailKitEmail.MoveMessage() with destination " 
                           + emailFolders[folderIndex].Folder!.Name);
            var UID = activeFolder!.UIDs[messageIndex];

            FolderRemainingMoveTasks[folderIndex]++;
            ThreadPool.QueueUserWorkItem(state => AsyncMoveMessage(UID, activeFolder!, folderIndex));

            Debug.WriteLine("MailKitEmail.MoveMessage() complete");
        }
        private void AsyncMoveMessage(UniqueId UID, EmailFolder origin, int destinationIndex)
        {
            ImapMutex.WaitOne();

            if (!origin.Folder!.IsOpen)
            {
                origin.Folder.Open(FolderAccess.ReadWrite);
            }

            Debug.WriteLine("Server request to move message.");

            origin!.Folder!.MoveTo(UID, emailFolders[destinationIndex].Folder);
            FolderRemainingMoveTasks[destinationIndex]--;

            ImapMutex.ReleaseMutex();

            if (FolderRemainingMoveTasks[destinationIndex] == 0)
            {
                Debug.WriteLine("No waiting move tasks. Updating folder and sending event");
                updateFolder(destinationIndex);
            }
        }


        #endregion

         //____________________________________________
        // Functions that interact with the SMTP server 
        #region SMTP functions

        void IEmail.SendMessage(IEmail.Message message)
        {
            var addresses     = SplitEmails(message.to);
            var cc_addresses  = SplitEmails(message.cc);
            var bcc_addresses = SplitEmails(message.bcc);
            List<MimeMessage> messages = new();

            // If the strings are invalid, an exception will be thrown.
            foreach(var address in addresses)
            {
                var MimeMsg = new MimeMessage();
                try
                {
                    MimeMsg.From.Add(new MailboxAddress("", activeUser.username));
                    MimeMsg.To.Add(new MailboxAddress("", address));
                    if (message.cc.Length > 0)
                        foreach(var cc_address in cc_addresses)
                            MimeMsg.Cc.Add(new MailboxAddress("", cc_address));
                    if (message.bcc.Length > 0)
                        foreach (var bcc_address in bcc_addresses)
                            MimeMsg.Bcc.Add(new MailboxAddress("", bcc_address));
                    MimeMsg.Subject = message.subject;
                    MimeMsg.Body = new TextPart("plain") { Text = message.body };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Send message syntax error: " + ex.ToString());
                    if (addresses.Count > 1)
                        ex.Data.Add("Multi receiver email failed",
                                    "At least one email address of an email with multiple receivers returned an error. "
                                  + "The email was not send to any of the addresses in the receiver list. "
                                  + "Verify addresses and try again."); 
                    this.SentMessage(ex, message);
                    return;
                }
                messages.Add(MimeMsg);
            }

            foreach(var MimeMsg in messages)
                ThreadPool.QueueUserWorkItem(state => AsyncSendMessage(MimeMsg, message));
        }
        private void AsyncSendMessage(MimeMessage MimeMsg, IEmail.Message message)
        {
            SmtpMutex.WaitOne();
            try
            {
                smtpClient.Send(MimeMsg);
            }
            catch (Exception? ex)
            {
                Debug.WriteLine("AsyncSendMessage server exception: " + ex.ToString());
                this.SentMessage(ex, message);
                SmtpMutex.ReleaseMutex();
                return;
            }
            SmtpMutex.ReleaseMutex();
            this.SentMessage(null, message);
        }
        public event IEmail.SentMessageHandler SentMessage;

        #endregion

    }
}
