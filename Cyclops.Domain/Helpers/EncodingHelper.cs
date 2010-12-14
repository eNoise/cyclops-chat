using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Cyclops.Core.Helpers
{
    public static class EncodingHelper
    {
        /// <summary>
        /// Encode string into Base64
        /// </summary>
        public static string Encode(string source)
        {
            return Convert.ToBase64String(Encoding.Default.GetBytes(source));
        }

        /// <summary>
        /// Decode string from base64
        /// </summary>
        public static string Decode(string base64)
        {
            var data = Convert.FromBase64String(base64);
            return Encoding.Default.GetString(data);
        }
        /// <summary>
        /// Calculate SHA1 hash from given string
        /// </summary>
        public static byte[] CalculateSha1Hash(string val)
        {
            byte[] data = Encoding.UTF8.GetBytes(val);
            SHA1 sha = new SHA1Managed();
            return sha.ComputeHash(data);
        }
    }
}
