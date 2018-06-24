using System.Collections.Generic;
using FileServer.Application.AppServices.Glo;
using FileServer.Application.Dtos;
using FileServer.Application.Dtos.Glo;
using FileServer.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Controllers
{
    public class FileController : BaseController
    {
        public FileController(FileAppService fileAppService)
        {
            _fileAppService = fileAppService;
        }
        private readonly FileAppService _fileAppService;

        /// <summary>
        /// 通用上传文件接口[post,multipart/form-data]
        /// 大小限制、
        /// 数量限制、
        /// 文件类型限制
        /// </summary>
        [HttpPost]
        public ResponseDto<List<string>> Upload(UploadFileCmd cmd)
        {
            var result = _fileAppService.Upload(cmd);
            return Success("上传成功！", result);
        }

        /// <summary>
        /// 通用删除文件接口
        /// </summary>
        [HttpPost]
        public ResponseDto<string> Remove([FromBody]RemoveFileCmd cmd)
        {
            _fileAppService.Remove(cmd);
            return Success("删除成功！");
        }
    }
}