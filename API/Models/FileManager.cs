using Encryptors.Encryptors;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace API.Models
{
    public class FileManager
    {
        public static async Task<string> SaveFileAsync(IFormFile file, string path)
        {
            if (Directory.Exists($"{path}/Uploads"))
            {
                if (File.Exists($"{path}/Uploads/{file.FileName}"))
                {
                    File.Delete($"{path}/Uploads/{file.FileName}");
                }
            }
            else
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
            if (Directory.Exists(savingPath))
            {
                if (File.Exists($"{savingPath}/public.key"))
                {
                    File.Delete($"{savingPath}/public.key");
                }

                if (File.Exists($"{savingPath}/private.key"))
                {
                    File.Delete($"{savingPath}/private.key");
                }
            }
            else
            {
                Directory.CreateDirectory(savingPath);
            }
            encryptor.GetKeys(p, q, savingPath);

            return savingPath;
        }

        public static string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            var encryptor = new RSAEncryptor();
            var encryptionsPath = $"{savingPath}/Encryptions";
            if (Directory.Exists(encryptionsPath))
            {
                if (File.Exists($"{encryptionsPath}/{nombre}.txt"))
                {
                    File.Delete($"{encryptionsPath}/{nombre}.txt");
                }
            }
            else
            {
                Directory.CreateDirectory(encryptionsPath);
            }
            return encryptor.EncryptFile(keyPath, filePath, encryptionsPath, nombre);
        }
        public static string DecryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            var decryptor = new RSAEncryptor();
            var decryptionsPath = $"{savingPath}/Decryptions";
            if (Directory.Exists(decryptionsPath))
            {
                if (File.Exists($"{decryptionsPath}/{nombre}.txt"))
                {
                    File.Delete($"{decryptionsPath}/{nombre}.txt");
                }
            }
            else
            {
                Directory.CreateDirectory(decryptionsPath);
            }
            return decryptor.DecryptFile(keyPath, filePath, decryptionsPath, nombre);
        }

        public static bool PQValidness(string p, string q)
        {
            try
            {
                var pnumber = Convert.ToInt32(p);
                var qnumber = Convert.ToInt32(q);
                if (RSAEncryptor.IsPrime(pnumber) && RSAEncryptor.IsPrime(qnumber) && p != q)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
