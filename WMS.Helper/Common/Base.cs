using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security;


namespace WMS.Core.Helper.Common
{
    public class Base
    {
        #region Security
        public static string Encrypt(string input)
        {
            try
            {
                byte[] passBytes = System.Text.Encoding.Unicode.GetBytes(input);
                string encryptPassword = Convert.ToBase64String(passBytes);
                return encryptPassword;
            }
            catch
            {

                return "0";
            }

        }
        public static string Decrypt(string input)
        {
            try
            {
              
                byte[] passByteData = Convert.FromBase64String(input);
                string originalPassword = System.Text.Encoding.Unicode.GetString(passByteData);
                return originalPassword;
            }
            catch
            {
                return "0";
            }
        }
        #endregion
        #region String Manipulation
        public static string GenearateKey(int maxSize)
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, maxSize)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result.ToString();
        }
        public static string Truncate(string source, int length)
        {
            if (source.Length > length)
            {
                source = source.Substring(0, length) + "...";
            }
            return source;
        }
        public static string SearchString(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            { searchString = string.Format("{0}{1}{2}", "%", searchString, "%"); }
            return searchString.ToString();
        }
        public static string GenerateCode(string code, string name)
        {      
         string newCode = string.Format("{0}-{1}",code, name );
         return newCode.ToUpper();
        }
        #endregion
    }
}
