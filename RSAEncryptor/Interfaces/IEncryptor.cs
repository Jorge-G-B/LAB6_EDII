using System;
using System.Collections.Generic;
using System.Text;

namespace Encryptors.Interfaces
{
    interface IEncryptor
    {
        string[] GetKeys(string p, string q);
        string EncryptFile(string keyPath, string filePath, string savingPath, string nombre);
    }
}
