using System;
using System.IO;
using FileServer.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using SixLabors.ImageSharp;

namespace FileServer.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {
        /// <summary>
        /// 上传图片[要求客户端必须使用multipart/form-data请求编码类型]
        /// </summary>
        [HttpPost]
        public IActionResult Upload(UploadImageCmd cmd)
        {
            if (cmd.Files == null)
                throw new Exception("请选择文件！（要求使用multipart/form-data请求编码类型）");

            var directory = PlatformServices.Default.Application.ApplicationBasePath + "/upload/" + DateTime.Now.ToString("yyyyMM/");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            foreach (var formFile in cmd.Files.Files)
            {
                //MimeKit，文件Mime类型检查
                var fileExt = Path.GetExtension(formFile.FileName);

                //原图[不保存大/中/小图，需要的时候，自己加上webp后缀即可]
                using (var image = Image.Load(formFile.OpenReadStream()))
                {
                    var fileName = directory + NewId.Next().ToString("N") + fileExt;
                    image.Save(fileName);
                }
            }
            return Json(true);
        }
    }
}