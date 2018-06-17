using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Jin.FileServer
{
    /// <summary>
    /// 文件服务器扩展类
    /// </summary>
    public static class FileServerExtensions
    {
        /// <summary>
        /// 启用静态文件服务器中间件
        /// </summary>
        public static IApplicationBuilder UseJinFileServer(this IApplicationBuilder app, IHostingEnvironment env, Action<JinFileServerOptions> action)
        {
            return app.UseMiddleware<FileServerMiddleware>(env, action);
        }

    }
}