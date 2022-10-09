using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        SmtpClient smtpClient = new();
        ImapClient imapClient = new();

        IDatabase.User activeUser = new();

        IMailFolder? activeFolder;
        // These two lists will contains a message and its ID at the same index
        List<MimeMessage> activeFolderMessages = new();
        List<UniqueId> activeFolderUIDS = new();

        public MailKitEmail()
        {
            // Initalizing class data variables.
            List<IDatabase.User> users = data.GetUsers();

        }

        /// <summary>
        /// Sets as active and opens the given folder
        /// </summary>
        /// <param name="folder"></param>
        void updateActiveFolder(IMailFolder folder)
        {
            activeFolder = folder;
            folder.Open(FolderAccess.ReadWrite);

            activeFolderMessages.Clear();
            activeFolderUIDS.Clear();
            // Adding the messages from oldest to newest
            foreach(var message in activeFolder.Reverse())
            {
                activeFolderMessages.Add(message);
            }
            foreach (var UID in activeFolder.Fetch(0, -1, 
                                                   MessageSummaryItems.UniqueId))
            {
                activeFolderUIDS.Add(UID.UniqueId);
            }
            // To correspond with messages, the list must be reversed.
            activeFolderUIDS.Reverse();
        }

        /// <summary>
        /// Verifies that an input string is an email address
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool verifyEmail(string input)
        {
            // __To be implemented__

            return true;
        }

        IDatabase.User IEmail.GetActiveUser()
        {
            return activeUser;
        }

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
            updateActiveFolder(imapClient.Inbox);
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

        //__________________________________________
        // Functions that supply IMAP info to the GUI
        #region imap interaction functions
        List<(string from, string subject, bool read)> IEmail.GetFolderEnvelopes()
        {
            List<(string, string, bool)> envelopes = new();

            // This list will contain the flags
            var flags = activeFolder.Fetch(activeFolderUIDS, MessageSummaryItems.Flags);
            if (activeFolder != null)
                // This foreach loop merges the messages and UID lists to
                // add them to the return list in one iteration.
                foreach (var item in activeFolderMessages.Zip(flags, (a, b) => new { message = a, flag = b }))
                {
                    // Gettings seen flag for message
                    var seenFlag = item.flag.Flags.Value.HasFlag(MessageFlags.Seen);
                    envelopes.Add((item.message.From.ToString(), item.message.Subject, seenFlag));
                }

            return envelopes;
        }

        IEmail.Message? IEmail.GetMessage(int messageIndex)
        {
            if(messageIndex < activeFolderMessages.Count & messageIndex >= 0)
            {
                var ImapMessage = activeFolderMessages[messageIndex];
                IEmail.Message message = new();
                    message.from = ImapMessage.From.ToString();
                    message.to = ImapMessage.To.ToString();
                    message.subject = ImapMessage.Subject;
                    message.body = ImapMessage.Body.ToString();
                return message;
            }
            else
            {
                return null;
            }
            
        }
        #endregion
    }
}
