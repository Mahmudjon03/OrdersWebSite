﻿using System.Security.Cryptography;
using System.Text;

namespace RestaurantLibrary.Helpers
{
    public class Hash
    {
        public static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(hash);
        }
    }
}
