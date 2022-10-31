using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Diagnostics;
using System.Collections;
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
            public EmailFolder(IMailFolder mainFolder, IMailFolder preloadFolder, string name) 
            { 
                MainImapFolder = mainFolder;
                PreloadImapFolder = preloadFolder;
                FolderName = name; 
            }

            public Mutex Mutex = new();

            public IMailFolder? MainImapFolder    { get; set; }
            public IMailFolder? PreloadImapFolder { get; set; }
            // __Flags__

            public bool isLoaded { get; set; } = false;
            // Is used to stop preload if it is progress during a new update
            public bool stopPreload { get; set; } = false;
            // Is used to lock a folder while loading is in progress
            public bool isLocked { get; set; } = false;

            // __Folder Data__
            public string? FolderName;
            public List<UniqueId> UIDs { get; set; } = new();
            public Dictionary<UniqueId, MimeMessage>    Messages  { get; set; } = new();
            public Dictionary<UniqueId, bool>           Seen      { get; set; } = new();
            // The following messages only contains envelope information. The remaining fields are empty.
            public Dictionary<UniqueId, IEmail.Message> Envelopes { get; set; } = new();
        }

        IDatabase data = new JSONDatabase();




        /// <summary>
        /// This client is the primary connection used to interact with the imap server
        /// </summary>
        ImapClient mainImapClient = new();
        Mutex MainImapMutex = new();
        /// <summary>
        /// This imap client is used to preload messenges.
        /// </summary>
        ImapClient preloadImapClient = new();
        Mutex preloadImapMutex = new();

        SmtpClient smtpClient = new();
        Mutex SmtpMutex = new();

        IDatabase.User activeUser = new();

        List<EmailFolder> emailFolders = new();
        EmailFolder? activeFolder;
        // This variable is used to throttle preloading speed when multiple folders are loading
        int foldersPreloading = 0;

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
            emailFolders.Add(new EmailFolder(mainImapClient.Inbox, 
                                             preloadImapClient.Inbox, 
                                             mainImapClient.Inbox.Name));
            string inbox_name = mainImapClient.Inbox.Name;

            // Adding folders
            var mainFolders = mainImapClient.GetFolders(mainImapClient.PersonalNamespaces[0], false);
            var preloadFolders = preloadImapClient.GetFolders(mainImapClient.PersonalNamespaces[0], false);
            for (int i = 0; i < mainFolders.Count; i++)
            {
                var mainFolder = mainFolders[i];
                // If statement to avoid adding the inbox twice
                if (mainFolder.Name != inbox_name & mainFolder.Exists)
                {
                    emailFolders.Add(new EmailFolder(mainFolder, preloadFolders[i], mainFolder.Name));
                }
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

            var folder = emailFolders[folderIndex];

            folder.stopPreload = true;

            MainImapMutex.WaitOne();
            folder.Mutex.WaitOne();

            // This will be used to remove duplicates once the new messages have been opened.
            int count = folder.UIDs!.Count;
            // This is necessary to let the user access old messages while inbox is updating.

            folder!.MainImapFolder!.Open(FolderAccess.ReadWrite);

            // Emptying lists and dictionaries to avoid adding things twice on reload
            folder.UIDs.Clear();
            folder.Seen.Clear();
            folder.Envelopes.Clear();

            Debug.WriteLine("Fetching summary items from folder " + folder.FolderName);
            foreach (var items in folder.MainImapFolder.Fetch(0, -1, 
                                                       MessageSummaryItems.Envelope |
                                                       MessageSummaryItems.UniqueId |
                                                       MessageSummaryItems.Flags))
            {
                folder.UIDs.Add(items.UniqueId);
                folder.Seen.Add(items.UniqueId, items.Flags!.Value.HasFlag(MessageFlags.Seen));

                IEmail.Message envelope = new();
                    envelope.from    = items.Envelope.From.ToString();
                if(items.Envelope.Date != null)
                    envelope.date    = items.Envelope.Date.Value.LocalDateTime.ToString();
                if(items.Envelope.Subject != null)
                    envelope.subject = items.Envelope.Subject.ToString();
                folder.Envelopes.Add(items.UniqueId, envelope);

            }

            MainImapMutex.ReleaseMutex();

            folder.isLoaded = true;

            //// Deleting UIDs of previously downloaded messages
            //if (count >= 0)
            //{
            //    folder.UIDs.RemoveRange(0, count);
            //}

            if (FolderUpdateFinished != null)
            {
                this.FolderUpdateFinished(folderIndex, EventArgs.Empty);
            }

            folder.Mutex.ReleaseMutex();

            // Proloading messages
            folder.stopPreload = false;
            ThreadPool.QueueUserWorkItem(state => PreloadMessages(folder));
        }

        private void PreloadMessages(EmailFolder folder)
        {
            foldersPreloading++;

            // This integer value sets a limit of how many messenges are preloaded.
            int loadLimit = 50;
            int maxCount = Math.Min(folder.UIDs.Count, loadLimit);

            for(int i = 0; i < maxCount; i++)
            {
                if (folder.stopPreload)
                {
                    folder.stopPreload = false;
                    return;
                }

                //Thread.Sleep(500 * foldersPreloading);

                var UID = folder.UIDs[i];
                if (!folder.Messages.ContainsKey(UID))
                {
                    preloadImapMutex.WaitOne();

                    if (!folder!.PreloadImapFolder!.IsOpen)
                        folder!.PreloadImapFolder.Open(FolderAccess.ReadWrite);

                    var message = folder!.PreloadImapFolder!.GetMessage(UID);

                    Debug.WriteLine("Folder " + folder.FolderName + " preloading message with subject " + message.Subject 
                                  + ". " + foldersPreloading.ToString() + " folders currently preloading.");

                    folder.Mutex.WaitOne();
                    folder.Messages[UID] = message;
                    folder.Mutex.ReleaseMutex();

                    preloadImapMutex.ReleaseMutex();
                }
            }

            Debug.WriteLine("Preload of folder \"" + folder.FolderName + "\" completed.");

            foldersPreloading--;
        }

        #endregion

         //__________________________________________
        // Functions that deal with server connection
        #region server connection functions
        void IEmail.Disconnect()
        {
            mainImapClient.Disconnect(true);
            preloadImapClient.Disconnect(true);
            smtpClient.Disconnect(true);
        }

        bool IEmail.isConnected()
        {
            return (mainImapClient.IsConnected & smtpClient.IsConnected);
        }

        Exception? IEmail.Connect(string IMAP_hostname, int IMAP_port, string SMTP_hostname, int SMTP_port)
        {
            try
            {
                mainImapClient.Connect(IMAP_hostname, IMAP_port, SecureSocketOptions.SslOnConnect);
                preloadImapClient.Connect(IMAP_hostname, IMAP_port, SecureSocketOptions.SslOnConnect);
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
                mainImapClient.Authenticate(username, password);
                preloadImapClient.Authenticate(username, password);
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

        int IEmail.SetActiveFolder(int folderIndex)
        {
            var folder = emailFolders[folderIndex];

            // If folder is locked, return
            if (folder.isLocked)
                return -1;

            // Else, set active with load if not loaded yet
            if(!folder.isLoaded)
            {
                updateFolder(folderIndex);
            }

            activeFolder = folder;

            return 0;
        }

        List<(string UID, string from, string date, string subject, bool read)> IEmail.GetFolderEnvelopes(int folderIndex)
        {
            List<(string, string, string, string, bool)> envelopes = new();

            var folder = emailFolders[folderIndex];

            // This list will contain the flags
            if (folder != null)
            {
                // This foreach loop merges the messages and UID lists to
                // add them to the return list in one iteration.
                for (int i = 0; i < folder.Envelopes.Count; i++)
                {
                    var UID = folder.UIDs[i];
                    envelopes.Add((UID.ToString(),
                                   folder.Envelopes[UID].from,
                                   folder.Envelopes[UID].date,
                                   folder.Envelopes[UID].subject,
                                   folder.Seen[UID]));
                }
            }
            return envelopes;
        }

        IEmail.Message? IEmail.OpenMessage(string sUID)
        {
            // Adding read flag
            var UID = UniqueId.Parse(sUID);

            if (!activeFolder!.Seen[UID])
            {
                ToggleReadQueue.Enqueue(new ToggleReadParameters() {folder = activeFolder!,
                                                                    ID = UID,
                                                                    seenFlag = false  });
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            }

            // Getting message. If it was not already loaded, it is added to the local folder
            MimeMessage ImapMessage;
            if(activeFolder.Messages.ContainsKey(UID))
                ImapMessage = activeFolder.Messages[UID];
            else
            {
                MainImapMutex.WaitOne();

                if (!activeFolder!.MainImapFolder!.IsOpen) { }
                    activeFolder!.MainImapFolder.Open(FolderAccess.ReadWrite);

                Debug.WriteLine("Message not loaded. Loading...");
                ImapMessage = activeFolder!.MainImapFolder!.GetMessage(UID);
                activeFolder.Messages[UID] = ImapMessage;
                Debug.WriteLine("Message with subject {0} loaded", ImapMessage.Subject);

                MainImapMutex.ReleaseMutex();
            }
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

        List<string> IEmail.GetFoldernames()
        {
            List<string> folderNames = new();
            foreach (var folder in emailFolders)
            {
                folderNames.Add(folder.MainImapFolder!.Name);
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

        void IEmail.ToggleRead(string sUID)
        {
            Debug.WriteLine("Running MailKitEmail.ToggleRead()");
            var UID = UniqueId.Parse(sUID);
            var seen = activeFolder!.Seen[UID];
            if (!seen)
            {
                activeFolder.Seen[UID] = true; // Updating local data
                Debug.WriteLine("Locally adding flag");
            }  
            else
            {
                activeFolder.Seen[UID] = false;
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
            MainImapMutex.WaitOne();

            if (ToggleReadQueue.Count > 0)
            {

                // Gettings parameters
                var parameters = ToggleReadQueue.Dequeue();

                if (!parameters.folder!.MainImapFolder!.IsOpen)
                    parameters.folder.MainImapFolder.Open(FolderAccess.ReadWrite);

                if (!parameters.seenFlag)
                {
                    parameters.folder!.MainImapFolder.AddFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to add flag");
                }
                else
                {
                    parameters.folder!.MainImapFolder.RemoveFlags(parameters.ID, MessageFlags.Seen, true);
                    Debug.WriteLine("Server request to remove flag");
                }

                // Recursive call to handle next queued task if necessary
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(null, EventArgs.Empty));
            }

            MainImapMutex.ReleaseMutex();
        }

        void IEmail.Delete(string sUID)
        {
            Debug.WriteLine("Running MailKitEmail.Delete()");
            var UID = UniqueId.Parse(sUID);

            // The local message entry will not be removed
            ThreadPool.QueueUserWorkItem(state => AsyncDelete(activeFolder!, UID));

            Debug.WriteLine("MailKitEmail.Delete() complete");
        }
        private void AsyncDelete(EmailFolder folder, UniqueId ID)
        {
            MainImapMutex.WaitOne();

            if(!folder.MainImapFolder!.IsOpen)
            {
                folder.MainImapFolder.Open(FolderAccess.ReadWrite);
            }

            folder.MainImapFolder.AddFlags(ID, MessageFlags.Deleted, true);
            Debug.WriteLine("Server request to add deleted flag");

            MainImapMutex.ReleaseMutex();
        }

        // This list is increment when a move task is started. 
        // It will be decremented when a move task is done.
        // The MoveMessageCompleted signal will be sent when the value is 0 for the given folder.
        List<int> FolderRemainingMoveTasks; // = new(emailFolders.Count);
        void IEmail.MoveMessage(string sUID, int folderIndex)
        {
            Debug.WriteLine("Running MailKitEmail.MoveMessage() with destination " 
                           + emailFolders[folderIndex].MainImapFolder!.Name);
            var UID = UniqueId.Parse(sUID);

            FolderRemainingMoveTasks[folderIndex]++;

            ThreadPool.QueueUserWorkItem(state => AsyncMoveMessage(UID, activeFolder!, folderIndex));

            Debug.WriteLine("MailKitEmail.MoveMessage() complete");
        }
        private void AsyncMoveMessage(UniqueId UID, EmailFolder origin, int destinationIndex)
        {
            MainImapMutex.WaitOne();
            var destinationFolder = emailFolders[destinationIndex];

            if (!origin.MainImapFolder!.IsOpen)
            {
                origin.MainImapFolder.Open(FolderAccess.ReadWrite);
            }

            Debug.WriteLine("Server request to move message.");

            var destinationUID = origin!.MainImapFolder!.MoveTo(UID, destinationFolder.MainImapFolder)!.Value;

            // Adding message information to new folder
            destinationFolder.UIDs.Add(destinationUID);
            destinationFolder.Envelopes[destinationUID] = origin.Envelopes[UID];
            destinationFolder.Seen[destinationUID] = origin.Seen[UID];
            destinationFolder.Messages[destinationUID] = origin.Messages[UID];

            // Removing the message from the origin folder locally:
            origin.Messages.Remove(UID);

            FolderRemainingMoveTasks[destinationIndex]--;

            MainImapMutex.ReleaseMutex();

            Debug.WriteLine("Sending MoveMessageFinished event for message with subject " + origin.Envelopes[UID].subject);
            this.MoveMessageFinished(destinationUID.ToString()!,
                                     destinationIndex,
                                     origin.Envelopes[UID],
                                     origin.Seen[UID]);
        }
        public event IEmail.MoveMessageFinishedHandler MoveMessageFinished;


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
