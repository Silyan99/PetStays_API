using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PetStays.App.Helpers
{
    public static class MD5GenHelper
    {
        public static string MD5Hash(string text)
        {
            MD5 mD = new MD5CryptoServiceProvider();
            mD.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] hash = mD.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                stringBuilder.Append(hash[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
