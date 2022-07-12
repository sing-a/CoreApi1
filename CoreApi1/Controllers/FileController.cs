using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string[] permittedExtensions = { ".txt", ".pdf", ".docx" };
        private static readonly Dictionary<string, List<byte[]>> _fileSignature =
            new Dictionary<string, List<byte[]>>
        {
            {".pdf",new List<byte[]>
            {
                new byte[]{0x25,0x50,0x44,0x46}
            }
            },
            {".docx",new List<byte[]>
            {
                new byte[]{0x50,0x48,0x03,0x04},
                new byte[]{0x50,0x48,0x03,0x04,0x14,0x00,0x06,0x00}
            }
            }
        };
        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        /// <summary>
        /// 单文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [RequestSizeLimit(524288000)]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return BadRequest();
            }
            //using (var reader = new BinaryReader(file.OpenReadStream()))
            //{
            //    var signature = _fileSignature[ext];
            //    var headerBytes = reader.ReadBytes(signature.Max(m => m.Length));
            //    if (!signature.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature)))
            //    {
            //        return BadRequest(new { Msg = "文件有误" });
            //    }
            //}
            var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);

            //拼接存储路径
            var filePath = Path.Combine(_environment.ContentRootPath, fileName);
            using (var fs = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(fs);
            }
            return Ok(new { Msg = "上传成功", Code = 200, FileName = fileName });
        }

        [HttpPost("multi-upload")]
        public async Task<IActionResult> MultiUploadFileAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            List<string> fileNames = new List<string>();
            foreach (var file in files)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(_environment.ContentRootPath, fileName);
                fileNames.Add(fileName);
                using (var fs = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                }
            }
            return Ok(new { Msg = "上传成功", Code = 200, FileName = fileNames, TotalSize = size });
        }
    }
}
