using System;
using System.Collections.Generic;
using System.Linq;

namespace Cyclops.Core.Security
{
    public interface IStringEncryptor
    {
        string EncryptString(string plainText);
        string DecryptString(string encryptedText);
    }
}
