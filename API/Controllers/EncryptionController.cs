using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using API.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var zipPath = FileManager.GetZip(Env.ContentRootPath, p, q);
                response.Content = new StreamContent(new FileStream(zipPath, FileMode.Open));
                return (IActionResult)response;
            }
            catch
            {
                return StatusCode(500, "El formato ingresado de p y q no es válido.");
            }
        }

        [HttpPost]
        [Route("/api/rsa/{nombre}")]
        public IActionResult Encrypt(string nombre, [FromForm]IFormFile key, [FromForm]IFormFile file)
        {
            try
            {
                var keyPath = FileManager.SaveFileAsync(key, Env.ContentRootPath);
                var filePath = FileManager.SaveFileAsync(file, Env.ContentRootPath);
                var processedFilePath = FileManager.ProcessFile(keyPath.Result, filePath.Result, Env.ContentRootPath, nombre);
                var fileName = Path.GetFileName(processedFilePath);
                return PhysicalFile(processedFilePath, MediaTypeNames.Text.Plain, fileName);
            }
            catch 
            {
                return StatusCode(500);
            }
        }
    }
}
