using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Mime;
using API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private IWebHostEnvironment Env;
        public EncryptionController(IWebHostEnvironment _env)
        {
            Env = _env;
        }

        // GET: api/<EncryptionController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("/api/rsa/keys/{p}/{q}")]
        public IActionResult GetKeys(string p, string q)
        {
            try
            {
                var zipPath = FileManager.GetZip(Env.ContentRootPath, p, q);
                ZipFile.CreateFromDirectory($"{zipPath}", $"{zipPath}/../keys.zip");
                var filestream = new FileStream($"{zipPath}/../keys.zip", FileMode.Open);
                return File(filestream, "application/zip");
            }
            catch
            {
                return StatusCode(500, "El formato ingresado de p y q no es válido.");
            }
        }

        [HttpPost]
        [Route("/api/rsa/{nombre}")]
        public IActionResult EncryptAndDecrypt(string nombre, [FromForm]IFormFile key, [FromForm]IFormFile file)
        {
            try
            {
                var keyPath = FileManager.SaveFileAsync(key, Env.ContentRootPath);
                var originalKey = key.FileName;
                if (originalKey.Equals("public.key"))
                {
                    var filePath = FileManager.SaveFileAsync(file, Env.ContentRootPath);
                    var processedFilePath = FileManager.EncryptFile(keyPath.Result, filePath.Result, Env.ContentRootPath, nombre);
                    var fileName = Path.GetFileName(processedFilePath);
                    return PhysicalFile(processedFilePath, MediaTypeNames.Text.Plain, fileName);
                }
                else if (originalKey.Equals("private.key"))
                {
                    var filePath = FileManager.SaveFileAsync(file, Env.ContentRootPath);
                    var processedFilePath = FileManager.DecryptFile(keyPath.Result, filePath.Result, Env.ContentRootPath, nombre);
                    var fileName = Path.GetFileName(processedFilePath);
                    return PhysicalFile(processedFilePath, MediaTypeNames.Text.Plain, fileName);
                }
                return StatusCode(500);

            }
            catch 
            {
                return StatusCode(500);
            }
        }
    }
}
