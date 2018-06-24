using System;
using System.Collections.Generic;
using System.IO;
using FileServer.Application.Dtos.Glo;
using FileServer.Configs;
using FileServer.Infrastructure.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace FileServer.Application.AppServices.Glo
{
    public class FileAppService
    {
        public FileAppService(IOptions<FileServerSettings> fileServerSettings, IHostingEnvironment env)
        {
            _fileServerSettings = fileServerSettings.Value;
            _env = env;
        }
        private readonly IHostingEnvironment _env;
        private readonly FileServerSettings _fileServerSettings;

        /// <summary>
        /// 文件服务器接收上传文件[要求使用multipart/form-data请求编码类型][上传图片也不保存大/中/小图，需要的时候，自己加上webp后缀即可][不考虑自定义文件名的情况]
        /// </summary>
        public List<string> Upload(UploadFileCmd cmd)
        {
            if (cmd.Files == null || cmd.Files.Files.Count == 0)
                throw new Exception("请选择文件！");

            //文件类型是图片时，进行图片内容类型验证
            if (cmd.FileType == FileType.Image)
                foreach (var formFile in cmd.Files.Files)
                    if (!formFile.ContentType.Contains("image"))
                        throw new Exception("图片格式不正确！");

            //验证文件路径长度和文件长度是否一致
            if (string.IsNullOrEmpty(cmd.FilePath))
                throw new Exception("文件保存路径不能为空！");

            var list = new List<string>();
            var fileServer = _fileServerSettings.FileServer;

            foreach (var formFile in cmd.Files.Files)
            {
                var fileExt = Path.GetExtension(formFile.FileName);

                //保存到指定路径下
                var directory = $"/upload/{EnumHelper.Description(cmd.ComeFrom)}/{cmd.FilePath}/{DateTime.Now:yyyyMM}/";
                var relativePath = directory + StringGenerator.FileName(fileExt);

                if (!Directory.Exists(_env.WebRootPath + directory))
                    Directory.CreateDirectory(_env.WebRootPath + directory);

                using (var fileStream = new FileStream(_env.WebRootPath + relativePath, FileMode.OpenOrCreate))
                {
                    formFile.CopyTo(fileStream);
                    list.Add(fileServer + relativePath);
                }
            }
            return list;
        }

        /// <summary>
        /// 彻底删除文件命令
        /// </summary>
        public void Remove(RemoveFileCmd cmd)
        {
            if (cmd.FilePaths == null || cmd.FilePaths.Count == 0)
                throw new Exception("文件路径数量不能为空！");
            foreach (var filePath in cmd.FilePaths)
            {
                if (string.IsNullOrEmpty(filePath))
                    continue;
                var uri = new Uri(filePath);
                var fileInfo = _env.WebRootFileProvider.GetFileInfo(uri.AbsolutePath);
                if (fileInfo.Exists)
                    File.Delete(fileInfo.PhysicalPath);
            }
        }
    }
}