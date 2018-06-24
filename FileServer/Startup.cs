using FileServer.Application.AppServices.Glo;
using FileServer.Configs;
using FileServer.Middlewares;
using Jin.FileServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace FileServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //[无依赖]文件服务器配置信息
            services.Configure<FileServerSettings>(Configuration.GetSection(nameof(FileServerSettings)));

            services.AddTransient<FileAppService>();

            services.AddSwaggerGen(c =>
            {
                //文档信息
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Jin.FileServer API文档",
                    Version = "v1",
                    Description = "",
                    Contact = new Contact
                    {
                        Name = "Jin",
                        Email = "869758965@qq.com"
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //3.启用异常处理中间件
            app.UseMiddleware<ExceptionMiddleware>();

            //4.启用文件服务器
            app.UseJinFileServer(env);

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Jins API v1");
            });

        }
    }
}
