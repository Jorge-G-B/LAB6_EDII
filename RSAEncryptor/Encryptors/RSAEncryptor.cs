using Encryptors.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Encryptors.Encryptors
{
    public class RSAEncryptor : IEncryptor
    {
        public string[] GetKeys(string p, string q)
        {
            return new string[] { string.Empty, string.Empty };
        }

        public string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            return string.Empty;
        }
    }
}
