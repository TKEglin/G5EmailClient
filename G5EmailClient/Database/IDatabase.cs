using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G5EmailClient.Database
{
    public interface IDatabase
    {
        /// <summary>
        /// Contains User data. The username is used as a unique identifier.
        /// Is initialised to empty strings and 0 integers.
        /// </summary>
        public class User
        {
            public string IMAP_hostname { get; set; } = string.Empty;
            public int    IMAP_port     { get; set; } = 0;
            public string SMTP_hostname { get; set; } = string.Empty;
            public int    SMTP_port     { get; set; } = 0;
            public string username      { get; set; } = string.Empty;
            public string password      { get; set; } = string.Empty;
        };

        /// <summary>
        /// Used to get all saved users. If no users are saved, the list will be empty.
        /// </summary>
        /// <returns>A list of users. </returns>
        List<User> GetUsers();

        /// <summary>
        /// Gets the user with the given username.
        /// </summary>
        /// <returns>Returns the user. If no user by the given username is found, returns null.</returns>
        User? GetUser(string username);

        /// <summary>
        /// Gets the user whose username matches the saved default username.
        /// </summary>
        /// <returns> Returns the user if found. If the default username does not match a user, returns a new (empty) user. </returns>
        User GetDefaultUser();

        /// <summary>
        /// Sets the default user. IF the user is not found, nothing is changed.
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns 1 if default user changed, 0 if not.</returns>
        int SetDefaultUser(string username);

        /// <summary>
        /// Saves the user to Json string. If a user with the same username exists, the user will be overwritten.
        /// </summary>
        /// <returns>Returns 0 if new user added, 1 if user is overwritten.</returns>
        int SaveUser(User user);

        /// <summary>
        /// Deletes a user with the given username. Will do nothing if no user by that ID exists.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns> Returns 1 if the user is deleted, 0 if no user with the username is found. </returns>
        int DeleteUser(string username);

    }
}
