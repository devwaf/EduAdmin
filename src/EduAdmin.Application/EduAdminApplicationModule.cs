using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EduAdmin.Authorization;
using Abp.Quartz;
using Quartz;
using EduAdmin.TimeJobs;

namespace EduAdmin
{
    [DependsOn(
        typeof(AbpQuartzModule),
        typeof(EduAdminCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(EduAdminCoreModule))]
    public class EduAdminApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EduAdminAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EduAdminApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
        //定时器配置
        public override void PostInitialize()
        {
            var _jobManager = IocManager.Resolve<IQuartzScheduleJobManager>();
            _jobManager.ScheduleAsync<FileClear>(
                job =>
                {
                    // 任务名
                    job.WithIdentity("清理已删除文件", "清理磁盘")
                    .WithDescription("每天 23：00 点检测磁盘内存，小于设定值时删除文件");
                },
                trigger =>
                {
                    // 指定执行时间： 秒(0-59) 分(0-59) 时(0-23) 天(1-31) 月(1-12) 周(1-7) 年(1970-2099)
                    trigger.StartNow().WithCronSchedule("0 0 23 * * ?");// 每天 23：00 点执行
                }
            );
            //_jobManager.ScheduleAsync<FileClear>(
            //    job =>
            //    {
            //        // 任务名
            //        job.WithIdentity("清理已删除文件", "清理磁盘")
            //        .WithDescription("每天 23：00 点检测磁盘内存，小于设定值时删除文件");
            //    },
            //    trigger =>
            //    {
            //        // 指定执行时间： 秒(0-59) 分(0-59) 时(0-23) 天(1-31) 月(1-12) 周(1-7) 年(1970-2099)
            //        trigger.StartNow().WithSimpleSchedule(action =>
            //        {
            //            action.RepeatForever().WithIntervalInSeconds(5)
            //            .Build();
            //        });
            //    }
            //);
        }
    }
}
