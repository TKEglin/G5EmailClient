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

        public MailKitEmail()
        {
            List<IDatabase.User> users = data.GetUsers();
            
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


    }
}
