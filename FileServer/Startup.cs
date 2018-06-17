using System.Collections.Generic;
using System.Drawing;
using Jin.FileServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


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

            //启用文件服务器
            app.UseJinFileServer(env, opt =>
            {
                //opt.ImageFormats= new List<string> { ".bmp", ".gif", ".jpg", ".jpeg", ".png" };
                //opt.ImageSizes = new List<Size>
                //{
                //    new Size(80, 80),
                //    new Size(100, 100),
                //    new Size(200, 200),
                //    new Size(480, 480),
                //    new Size(500, 500),
                //    new Size(600, 600),
                //    new Size(800, 800),
                //};
            });

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
