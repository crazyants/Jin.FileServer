using FileServer.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Controllers.Common
{
    /// <summary>
    /// 表示所有控制器的基类（登陆/注册页面，不需授权）
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class BaseController : Controller
    {
        /// <summary>
        /// 返回成功实体[这样处理，无法返回不同的响应状态码]
        /// </summary>
        protected ResponseDto<string> Success(string message)
        {
            return ResponseDto.Success(message, string.Empty);
        }

        /// <summary>
        /// 返回成功实体，和具体的数据
        /// </summary>
        protected ResponseDto<T> Success<T>(string message, T data)
        {
            return ResponseDto.Success(message, data);
        }

        //弃用：无法返回强类型
        //protected IActionResult Json

    }
}