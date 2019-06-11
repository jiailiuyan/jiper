using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Ji.CommonHelper.Security
{
    public static class Cryptography
    {
        public static string EncryptToSHA1(this string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str);
            var data = SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
