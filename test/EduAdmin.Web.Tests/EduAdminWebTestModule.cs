using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EduAdmin.EntityFrameworkCore;
using EduAdmin.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace EduAdmin.Web.Tests
{
    [DependsOn(
        typeof(EduAdminWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EduAdminWebTestModule : AbpModule
    {
        public EduAdminWebTestModule(EduAdminEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EduAdminWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EduAdminWebMvcModule).Assembly);
        }
    }
}