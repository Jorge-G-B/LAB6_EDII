namespace Encryptors.Interfaces
{
    interface IEncryptor
    {
        string[] GetKeys(string p, string q, string keyPath);
        string ProcessFile(string keyPath, string filePath, string savingPath, string nombre);
    }
}
