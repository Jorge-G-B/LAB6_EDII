using Encryptors.Encryptors;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace API.Models
{
    public class FileManager
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string path)
        {
            if (!Directory.Exists($"{path}/Uploads"))
            {
                Directory.CreateDirectory($"{path}/Uploads");
            }
            using var saver = new FileStream($"{path}/Uploads/{file.FileName}", FileMode.OpenOrCreate);
            await file.CopyToAsync(saver);
            saver.Close();
            return $"{path}/Uploads/{file.FileName}";
        }

        public static string GetZip(string path, string p, string q)
        {
            var encryptor = new RSAEncryptor();
            var savingPath = $"{path}/Keys";
            encryptor.GetKeys(p, q, savingPath);
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }
            return savingPath;
        }

        public static string ProcessFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            var encryptor = new RSAEncryptor();
            var encryptionsPath = $"{savingPath}/ProcessedFiles";
            if (!Directory.Exists(encryptionsPath))
            {
                Directory.CreateDirectory(encryptionsPath);
            }
            return encryptor.ProcessFile(keyPath, filePath, encryptionsPath, nombre);
        }
    }
}
