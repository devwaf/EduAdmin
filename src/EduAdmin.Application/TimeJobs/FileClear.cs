using Abp.Dependency;
using Abp.Quartz;
using EduAdmin.FileManagements;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EduAdmin.TimeJobs
{
    /// <summary>
    /// 已删除文件清理 但磁盘小于一定数量时，自动清理文件
    /// </summary>
    public class FileClear : JobBase, ITransientDependency
    {
        private readonly IFileManagementAppService _fileManagementAppService;

        public FileClear(IFileManagementAppService fileManagementAppService) {
            _fileManagementAppService = fileManagementAppService;
        }
        public override async Task Execute(IJobExecutionContext context)
        {
            // 清理文件
            await _fileManagementAppService.DeleteInActiveFile();
        }
    }
}
