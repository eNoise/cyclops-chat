using System.IO;
using System.Security.Cryptography;
using System.Text;
using Cyclops.Core.Helpers;

namespace Cyclops.Core.Security
{
    public class TripleDesStringEncryptor : IStringEncryptor
    {
        private byte[] key;
        private byte[] iv;
        private TripleDESCryptoServiceProvider provider;

        public TripleDesStringEncryptor()
        {
            key = System.Text.Encoding.ASCII.GetBytes("ASYAHAGCBDUUADIADKOPAAAW");
            iv = System.Text.Encoding.ASCII.GetBytes("SSAZBGAW");
            provider = new TripleDESCryptoServiceProvider();
        }

        #region IStringEncryptor Members

        public string EncryptString(string plainText)
        {
            return Base64Helper.Encode(Transform(plainText, provider.CreateEncryptor(key, iv)));
        }

        public string DecryptString(string encryptedText)
        {
            return Transform(Base64Helper.Decode(encryptedText), provider.CreateDecryptor(key, iv));
        }

        #endregion

        private string Transform(string text, ICryptoTransform transform)
        {
            if (text == null)
                return null;
            using (var stream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    byte[] input = Encoding.Default.GetBytes(text);
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();

                    return Encoding.Default.GetString(stream.ToArray());
                }
            }
        }
    }
}