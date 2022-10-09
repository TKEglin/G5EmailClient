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
    }
}
