using System;
using System.Security.Cryptography;
using System.Text;

namespace EduKeeper.Infrastructure
{
    public class Encryption
    {
        public static string ComputeSha256(String value)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                foreach (var b in result)
                    sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}