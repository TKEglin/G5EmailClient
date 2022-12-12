using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Security.Cryptography;

namespace G5EmailClient.Database
{
    public class JSONDatabase : IDatabase
    {
        string email_data_file_path;

        JsonNode email_data;

        public JSONDatabase()
        {
            string? program_folder_path = Path.GetDirectoryName(Application.ExecutablePath);
            string data_folder_path = "email_data";
            if (program_folder_path != null)
            {
                data_folder_path = Path.Combine(program_folder_path, data_folder_path);
            }
            email_data_file_path = Path.Combine(data_folder_path, "user_data.json");

            // Creating file if it does not exist
            if (!File.Exists(email_data_file_path))
            {
                Directory.CreateDirectory(data_folder_path);
                File.Create(email_data_file_path);
            }
            if (new FileInfo(email_data_file_path).Length == 0)
            {
                var email_data_json = new JsonObject()
                {   
                    ["DefaultUser"] = "",
                    ["Users"] = new JsonArray()
                };
                var data_json = email_data_json.ToJsonString();

                File.WriteAllText(email_data_file_path, data_json);
            }

            var data_string = File.ReadAllText(email_data_file_path);
            email_data = JsonNode.Parse(data_string)!;
        }

        /// <summary>
        /// Saves the data stored in email_data to disk. If this is not called 
        /// when changes are made, data will be lost when the program closes.
        /// </summary>
        internal void SaveData()
        {
            var data_string = email_data.ToJsonString();
            File.WriteAllText(email_data_file_path, data_string);
        }

        List<IDatabase.User> IDatabase.GetUsers()
        {
            JsonArray? usersJson = email_data["Users"]!.AsArray();
            List<IDatabase.User> users = new();
            foreach (var userJson in usersJson)
            {
                var user = JsonSerializer.Deserialize<IDatabase.User>(userJson);
                users.Add(DecryptUser(user!));
            }
            return users;
        }

        IDatabase.User? IDatabase.GetUser(string username)
        {
            JsonArray? usersJson = email_data!["Users"]!.AsArray();
            foreach(var userJson in usersJson)
            {
                var user = JsonSerializer.Deserialize<IDatabase.User>(userJson);
                if(user!.username == username)
                {
                    return DecryptUser(user);
                }
            }
            // Else:
            return null;
        }

        IDatabase.User IDatabase.GetDefaultUser()
        {
            string defaultUser = email_data!["DefaultUser"]!.GetValue<string>();
            // If defaultUser is not empty, search for default user.
            if (defaultUser.Length > 0)
            {
                var user_profiles = email_data!["Users"]!.AsArray();
                foreach (var user in user_profiles)
                {
                    if (defaultUser == user!["username"]!.GetValue<string>())
                    {
                        return DecryptUser(JsonSerializer.Deserialize<IDatabase.User>(user)!);
                    }
                }
            }
            // Else:
            return new IDatabase.User();
        }

        int IDatabase.SetDefaultUser(string username)
        {
            var user_profiles = email_data!["Users"]!.AsArray();
            foreach (var user in user_profiles)
            {
                if (username == user!["username"]!.GetValue<string>())
                {
                    email_data["DefaultUser"] = username;
                    SaveData();
                    return 1;
                }
            }
            return 0;
        }

        int IDatabase.SaveUser(IDatabase.User param_user)
        {
            param_user = EncryptUser(param_user);
            var userJson = JsonSerializer.Serialize<IDatabase.User>(param_user);
            var user_profiles = email_data!["Users"]!.AsArray();

            for(int i = 0; i < user_profiles.Count; i++)
            {
                if (user_profiles[i]!["username"]!.GetValue<string>() == param_user.username)
                {
                    // Overwrite:
                    user_profiles[i] = JsonNode.Parse(userJson);
                    email_data["Users"] = user_profiles;
                    SaveData();
                    return 1;
                }
            }
            // Else:
            user_profiles.Add(JsonNode.Parse(userJson));
            email_data["Users"] = user_profiles;
            SaveData();
            return 0;
        }

        int IDatabase.DeleteUser(string username)
        {
            var user_profiles = email_data!["Users"]!.AsArray();

            for (int i = 0; i < user_profiles.Count; i++)
            {
                if (user_profiles[i]!["username"]!.GetValue<string>() == username)
                {
                    user_profiles.RemoveAt(i);
                    SaveData();
                    return 1;
                }
            }
            // Else:
            return 0;
        }

        /// <summary>
        /// Decrypts encrypted user password data
        /// </summary>
        /// <returns>The decrypted user</returns>
        private IDatabase.User DecryptUser(IDatabase.User user)
        {
            user.password = G5Encryption.Decrypt(user.password);
            return user;
        }

        /// <summary>
        /// Encrypts user password data
        /// </summary>
        /// <returns>The encrypted user</returns>
        private IDatabase.User EncryptUser(IDatabase.User user)
        {
            user.password = G5Encryption.Encrypt(user.password);
            return user;
        }
    }
}
