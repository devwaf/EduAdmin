using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EduAdmin.Configuration;

namespace EduAdmin.Web.Host.Startup
{
    [DependsOn(
       typeof(EduAdminWebCoreModule))]
    public class EduAdminWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public EduAdminWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EduAdminWebHostModule).GetAssembly());
        }
    }
}
