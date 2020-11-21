namespace Encryptors.Interfaces
{
    interface IEncryptor
    {
        void GetKeys(string p, string q, string keyPath);
        string DecryptFile(string keyPath, string filePath, string savingPath, string nombre);
        string EncryptFile(string keyPath, string filePath, string savingPath, string nombre);
    }
}
