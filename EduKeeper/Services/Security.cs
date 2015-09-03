using System;
using System.Security.Cryptography;
using System.Text;

namespace EduKeeper.Services
{
    public class Security
    {
        public static string ComputeSha256(String value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}