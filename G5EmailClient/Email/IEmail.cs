using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using G5EmailClient.Database;

namespace G5EmailClient.Email
{
    public interface IEmail
    {
        /// <summary>
        /// Contains message information.
        /// </summary>
        public class Message
        {
            public string date    { get; set; } = string.Empty;
            public string from    { get; set; } = string.Empty;
            public string to      { get; set; } = string.Empty;
            public string cc      { get; set; } = string.Empty;
            public string bcc     { get; set; } = string.Empty;
            public string subject { get; set; } = string.Empty;
            public string body    { get; set; } = string.Empty;
        }

        /// <summary>
        /// Cleanly disconnects from all servers.
        /// </summary>
        public void Disconnect();

        /// <summary>
        /// Checks if IEmail client is connected to server. Does not check if client is authenticated to a user account.
        /// </summary>
        /// <returns>Returns true if both IMAP and SMTP are connected. False if not.</returns>
        bool isConnected();

        /// <summary>
        /// Attempts to connect to IMAP and SMTP servers using the information given in the parameters.
        /// </summary>
        /// <param name="IMAP_hostname"></param>
        /// <param name="IMAP_port"></param>
        /// <param name="SMTP_hostname"></param>
        /// <param name="SMTP_port"></param>
        /// <returns>Returns null if successful. Otherwise, the exception is returned.</returns>
        public Exception? Connect(string IMAP_hostname, int IMAP_port, string SMTP_hostname, int SMTP_port);

        /// <summary>
        /// Attempts to authenticate a user with the email IMAP and SMTP server. 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Returns null if successful. Otherwise, the exception is returned.</returns>
        public Exception? Authenticate(string username, string password);

        /// <summary>
        /// Gets a list of usernames from the database.
        /// </summary>
        /// <returns>A list of strings containing all saved usernames.</returns>
        List<string> GetUsernames();

        /// <summary>
        /// Gets a user given a username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user if it exists. Otherwise, returns null.</returns>
        IDatabase.User? GetUser(string username);

        /// <summary>
        /// Gets the active user.
        /// </summary>
        /// <returns>The active user.</returns>
        IDatabase.User GetActiveUser();

        /// <summary>
        /// Gets the user whose username matches the saved default username.
        /// </summary>
        /// <returns> Returns the user if found. 
        /// If the default username does not match a user, returns a new (empty) user. </returns>
        IDatabase.User GetDefaultUser();

        /// <summary>
        /// Sets the default user. IF the user is not found, nothing is changed.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns 1 if default user changed, 0 if not.</returns>
        int SetDefaultUser(string username);

        /// <summary>
        /// Deletes a user with the given username. Will do nothing if no user by that ID exists.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns> Returns 1 if the user is deleted, 0 if no user with the username is found. </returns>
        int DeleteUser(string username);

        /// <summary>
        /// Saves the user to the database. If a user with the same username exists, the user will be overwritten.
        /// </summary>
        /// <returns>Returns 0 if new user added, 1 if user is overwritten.</returns>
        int SaveUser(IDatabase.User saveUser);

        /// <summary>
        /// Updates the folder of the given folder index. If index is less than 0, inbox is updated.
        /// </summary>
        /// <param name="folderIndex"></param>
        void UpdateFolder(int folderIndex);
        void UpdateFolderAsync(int folderIndex);

        /// <summary>
        /// Updates the active folder.
        /// </summary>
        void UpdateInbox();
        void UpdateInboxAsync();
        event EventHandler FolderUpdateFinished;

        /// <summary>
        /// Sets the active folder using a given index. Opens the folder if it is unopened.
        /// </summary>
        /// <param name="folderIndex"></param>
        /// <returns>Returns 0 if succesful, -1 if not.</returns>
        int SetActiveFolder(int folderIndex);

        /// <summary>
        /// Gets the from string, subject line and read status of all emails in the active folder.
        /// Each index in the list corresponds to the index of the folder in the underlying list.
        /// </summary>
        /// <returns>A list of tuples.</returns>
        List<(string UID, string from, string date, string subject, bool read)> GetFolderEnvelopes(int folderIndex);

        /// <summary>
        /// Retrives and returns a message from the active folder given an index.
        /// </summary>
        /// <param name="messageIndex"></param>
        /// <returns>Returns a message if index is within range. Otherwise, retuns null.</returns>
        Message? OpenMessage(string UID);

        /// <summary>
        /// Gets a list of all foldernames. The index of the list will match the underlying index
        /// of the email folder in the IEmail client.
        /// </summary>
        /// <returns>A list of strings containing the folder names.</returns>
        List<string> GetFoldernames();

        /// <summary>
        /// Toggles between read and unread for the given index. 
        /// If the index is out of bounds, nothing happens.
        /// </summary>
        /// <param name="messageIndex"></param>
        void ToggleRead(string UID);

        /// <summary>
        /// Deletes the message in the active folder that corresponds to the index.
        /// Note: The message will not be locally deleted. The client assumes
        /// that the GUI will remove the message from the UI, so the message cannot
        /// be accessed again.
        /// </summary>
        /// <param name="messageIndex"></param>
        void Delete(string UID);

        /// <summary>
        /// Takes a message from the given message index and moves it
        /// to the folder with the given folder index. 
        /// The destinatin folder must be updated when the 
        /// Note: The message will not be locally removed form the origin folder. 
        /// The client assumes that the GUI will remove the message from the UI, 
        /// so the message cannot be accessed again.
        /// </summary>
        void MoveMessage(string UID, int folderIndex);
        event MoveMessageFinishedHandler MoveMessageFinished;
        public delegate void MoveMessageFinishedHandler(string UID, int folderIndex, 
                                                        Message Envelope, bool seen);

        /// <summary>
        /// Sends the given message to the given email. Empty strings will be ignored.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        void SendMessage(Message message);


        /// <summary>
        /// This event is triggered when the SendMessage function fails.
        /// It will pass a tuple of (Exception, IEmail.Message).
        /// </summary>
        event SentMessageHandler SentMessage;
        public delegate void SentMessageHandler(Exception? ex, IEmail.Message message);

    }
}
