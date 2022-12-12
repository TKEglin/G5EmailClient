using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.IO;
using System.Security.Cryptography;

namespace G5EmailClient.Database
{

    public static class G5Encryption
    {
        public static string Encrypt(this string text)
        {
            return Convert.ToBase64String(
                ProtectedData.Protect(
                    Encoding.Unicode.GetBytes(text), null, DataProtectionScope.LocalMachine));
        }

        public static string Decrypt(this string text)
        {
            return Encoding.Unicode.GetString(
                ProtectedData.Unprotect(
                     Convert.FromBase64String(text), null, DataProtectionScope.LocalMachine));
        }
    }
}
