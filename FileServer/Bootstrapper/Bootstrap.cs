using System.Reflection;
using Autofac;

namespace FileServer.Bootstrapper
{
    public class Bootstrap
    {
        /// <summary>
        /// 不要采用XML配置，项目越大，越难维护，配置起来看的头昏眼花。
        /// </summary>
        public static void ConfigureAutofac(ContainerBuilder builder)
        {
            
            var assemblies = new[] { Assembly.GetExecutingAssembly() };

            //注册服务（应用服务|领域服务）
            builder.RegisterAssemblyTypes(assemblies).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}