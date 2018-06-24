using System;
using System.Threading.Tasks;
using FileServer.Application.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace FileServer.Middlewares
{
    /// <summary>
    /// 异常处理中间件，无法拦截身份认证异常
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <inheritdoc />
        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        private readonly RequestDelegate _requestDelegate;

        /// <summary>
        /// 由运行时调用的异步方法
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// 处理异常
        /// </summary>
        public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ResponseDto<string> response;

            if (exception is DbUpdateConcurrencyException dbUpdateConcurrencyException)
                response = ResponseDto.Error("服务器君累倒了！请重试！！", dbUpdateConcurrencyException.ToString());//解决并发问题异常
            else if (exception is SecurityTokenExpiredException securityTokenExpiredException)
                response = ResponseDto.Error(securityTokenExpiredException.Message, securityTokenExpiredException.ToString());//一般异常
            else
                response = ResponseDto.Error(exception.Message, exception.ToString());//一般异常

            var result = JsonConvert.SerializeObject(response, ResponseDto.SerializerSettings);
            context.Response.ContentType = "application/json;charset=utf-8";
            context.Response.StatusCode = (int)ResponseStatus.Ok;
            await context.Response.WriteAsync(result);
        }
    }
}