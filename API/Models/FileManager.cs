using Encryptors.Encryptors;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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
            var keyPaths = encryptor.GetKeys(p, q);
            var savingPath = $"{path}/Keys";
            if (!Directory.Exists(savingPath))
            {
                Directory.CreateDirectory(savingPath);
            }
            using var compressedFiles = new FileStream($"{savingPath}/Compressed_Files.zip", FileMode.OpenOrCreate);
            using var zipArchive = new ZipArchive(compressedFiles, ZipArchiveMode.Create, false);
            zipArchive.CreateEntryFromFile(keyPaths[0], "public.key");
            zipArchive.CreateEntryFromFile(keyPaths[1], "private.key");
            return $"{savingPath}/Compressed_Files.zip";
        }

        public static string EncryptFile(string keyPath, string filePath, string savingPath, string nombre)
        {
            var encryptor = new RSAEncryptor();
            var encryptionsPath = $"{savingPath}/Encryptions";
            if (!Directory.Exists(encryptionsPath))
            {
                Directory.CreateDirectory(encryptionsPath);
            }
            return encryptor.EncryptFile(keyPath, filePath, encryptionsPath, nombre);
        }
    }
}
