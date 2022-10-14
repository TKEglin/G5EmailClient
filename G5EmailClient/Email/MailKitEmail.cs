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
        IDatabase data = new JSONDatabase();

        // Concurrency
            // Task queue
        
            // Mutexes 
        Mutex ImapMutex = new();
        Mutex SmtpMutex = new();


        SmtpClient smtpClient = new();
        ImapClient imapClient = new();

        IDatabase.User activeUser = new();

        IList<IMailFolder>? emailFolders;

        IMailFolder? activeFolder;
        // These lists will contains a message and other related info at the same index
        List<MimeMessage> activeFolderMessages = new();
        List<UniqueId> activeFolderUIDs = new();
        List<bool> activeFolderMessagesSeen = new(); // Seen flags are stored locally, for quick access.

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
            getFolders();
            updateActiveFolder(imapClient.Inbox);
        }

         //_____________________________________
        // Functions that access the imap server
        #region IMAP server interaction functions
        /// <summary>
        /// Retrieves the folders from the server.
        /// </summary>
        void getFolders()
        {
            emailFolders = imapClient.GetFolders(imapClient.PersonalNamespaces[0], false);
        }

        /// <summary>
        /// Sets as active and updates the given folder. This function should be called
        /// everytime a new folder is opened, and intermittently to check for new messages.
        /// </summary>
        /// <param name="folder"></param>
        /// 
        void updateActiveFolder(IMailFolder folder)
        {
            activeFolder = folder;
            folder.Open(FolderAccess.ReadWrite);

            activeFolderMessages.Clear();
            activeFolderUIDs.Clear();
            // Adding the messages from oldest to newest
            foreach(var message in activeFolder.Reverse())
            {
                activeFolderMessages.Add(message);
            }
            foreach (var items in activeFolder.Fetch(0, -1, 
                                                   MessageSummaryItems.UniqueId |
                                                   MessageSummaryItems.Flags).Reverse())
            {
                activeFolderUIDs.Add(items.UniqueId);
                activeFolderMessagesSeen.Add(items.Flags.Value.HasFlag(MessageFlags.Seen));
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

        List<(string from, string subject, bool read)> IEmail.GetFolderEnvelopes()
        {
            List<(string, string, bool)> envelopes = new();

            // This list will contain the flags
            if (activeFolder != null)
                // This foreach loop merges the messages and UID lists to
                // add them to the return list in one iteration.
                foreach (var item in activeFolderMessages.Zip(activeFolderMessagesSeen, 
                                                              (a, b) => new { message = a, seen = b }))
                {
                    envelopes.Add((item.message.From.ToString(), item.message.Subject, item.seen));
                }

            return envelopes;
        }

        IEmail.Message? IEmail.OpenMessage(int messageIndex)
        {
            if(messageIndex < activeFolderMessages.Count & messageIndex >= 0)
            {
                // Adding read flag
                if (!activeFolderMessagesSeen[messageIndex]) {
                    ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(
                                                          activeFolder!,
                                                          activeFolderUIDs[messageIndex],
                                                          false)); }

                // Getting message
                var ImapMessage = activeFolderMessages[messageIndex];
                IEmail.Message message = new();
                    message.from = ImapMessage.From.ToString();
                    message.to = ImapMessage.To.ToString();
                    message.subject = ImapMessage.Subject;
                    message.body = ImapMessage.TextBody.ToString();
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
                folderNames.Add(folder.Name);
            }
            return folderNames;
        }
        #endregion

         //__________________________________________
        // Functions that make changes in IMAP server
        #region IMAP server changing functions
        void IEmail.ToggleRead(int messageIndex)
        {
            Debug.WriteLine("Running MailKitEmail.ToggleRead()");
            var UID = activeFolderUIDs[messageIndex];
            var seen = activeFolderMessagesSeen[messageIndex];
            if (!seen)
            {
                activeFolderMessagesSeen[messageIndex] = true; // Updating local data
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(activeFolder!, UID, seen));
                Debug.WriteLine("Locally adding flag");
            }  
            else
            {
                activeFolderMessagesSeen[messageIndex] = false;
                ThreadPool.QueueUserWorkItem(state => AsyncToggleRead(activeFolder!, UID, seen));
                Debug.WriteLine("Locally removing flag");
            }
            Debug.WriteLine("MailKitEmail.ToggleRead() complete");
        }
        private void AsyncToggleRead(IMailFolder folder, UniqueId ID, bool seen)
        {
            ImapMutex.WaitOne();
            if(!seen)
            {
                folder.AddFlags(ID, MessageFlags.Seen, true);
                Debug.WriteLine("Server request to add flag");
            }
            else
            {
                folder.RemoveFlags(ID, MessageFlags.Seen, true);
                Debug.WriteLine("Server request to remove flag");
            }
            ImapMutex.ReleaseMutex();
        }

        void IEmail.Delete(int messageIndex)
        {
            Debug.WriteLine("Running MailKitEmail.Delete()");
            var UID = activeFolderUIDs[messageIndex];

            // The local message entry will not
            ThreadPool.QueueUserWorkItem(state => AsyncDelete(activeFolder!, UID));
            Debug.WriteLine("Locally adding flag");

            Debug.WriteLine("MailKitEmail.Delete() complete");
        }
        private void AsyncDelete(IMailFolder folder, UniqueId ID)
        {
            ImapMutex.WaitOne();

            folder.AddFlags(ID, MessageFlags.Deleted, true);
            Debug.WriteLine("Server request to add deleted flag");

            ImapMutex.ReleaseMutex();
        }

        #endregion

        //____________________________________________
        // Functions that interact with the SMTP server 
        #region SMTP functions

        void IEmail.SendMessage(IEmail.Message message)
        {

            var MimeMsg = new MimeMessage();
                MimeMsg.From.Add(new MailboxAddress("", activeUser.username));
                MimeMsg.To.  Add(new MailboxAddress("", message.to));
                //MimeMsg.Cc.  Add(new MailboxAddress("", message.cc));
                //MimeMsg.Bcc. Add(new MailboxAddress("", message.bcc));
                MimeMsg.Subject = message.subject;
                MimeMsg.Body = new TextPart("plain") { Text = message.body };

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
                this.SentMessage(ex, message);
            }
            SmtpMutex.ReleaseMutex();
            this.SentMessage(null, message);
        }
        public event IEmail.SentMessageHandler SentMessage;

        #endregion
    }
}
