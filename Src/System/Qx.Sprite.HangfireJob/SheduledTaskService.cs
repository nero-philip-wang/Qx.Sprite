// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Hangfire;
    using Hangfire.Storage;
    using Humanizer;
    using Mapster;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Qx.Sprite.Core;

    /// <summary>
    /// 定时任务管理器
    /// </summary>
    [Area("default")]
    [Route("api/v{version:apiVersion}/[area]/[controller]/")]
    [ApiController]
    [ServiceType(typeof(ISheduledTaskService))]
    public partial class SheduledTaskService : ISheduledTaskService, IScoped
    {
        private readonly ILogger<SheduledTaskService> logger;
        private readonly IEnumerable<ISheduledTask> jobs;

        /// <summary>
        /// Initializes a new instance of the <see cref="SheduledTaskService"/> class.
        /// </summary>
        /// <param name="logger">日志</param>
        /// <param name="jobs">定时任务列表</param>
        public SheduledTaskService(ILogger<SheduledTaskService> logger, IEnumerable<ISheduledTask> jobs)
        {
            this.logger = logger;
            this.jobs = jobs;
        }

        /// <summary>
        /// 初始化定时任务 - 同步 DI 中的 Job 和 Hangfire 中的 Job
        /// 如果 DI 中的 Job 不在 Hangfire 中，则创建
        /// 如果 Hangfire 中的 Job 不在 DI 中，则删除
        /// </summary>
        [NonAction]
        public void Init()
        {
            // 获取 Hangfire 中所有的定时任务
            var hangfireJobs = this.All();
            var hangfireJobIds = hangfireJobs.Select(j => j.Id).ToHashSet();

            // 获取 DI 中所有的定时任务
            var diJobs = this.jobs.ToList();
            var diJobIds = diJobs.Select(j => j.Title.Underscore()).ToHashSet();

            // 遍历 DI 中的 Job，如果不在 Hangfire 中，则创建
            foreach (var job in diJobs)
            {
                var jobId = job.Title.Underscore();
                if (!hangfireJobIds.Contains(jobId))
                {
                    this.logger.LogInformation("创建定时任务: {JobId}, Cron: {Cron}", jobId, job.CronExpression);
                    RecurringJob.AddOrUpdate(jobId, () => job.Start(), job.CronExpression);
                }
            }

            // 遍历 Hangfire 中的 Job，如果不在 DI 中，则删除
            foreach (var hangfireJob in hangfireJobs)
            {
                if (!diJobIds.Contains(hangfireJob.Id))
                {
                    this.logger.LogInformation("删除定时任务: {JobId}", hangfireJob.Id);
                    RecurringJob.RemoveIfExists(hangfireJob.Id);
                }
            }
        }

        /// <summary>
        /// 获取所有定时任务列表
        /// </summary>
        /// <returns>定时任务列表</returns>
        [HttpGet]
        public List<RecurringJobDto> All()
        {
            using var connection = JobStorage.Current.GetConnection();

            var recurringJobs = connection.GetRecurringJobs();
            return recurringJobs.Adapt<List<RecurringJobDto>>();
        }

        /// <summary>
        /// 手动触发定时任务
        /// </summary>
        /// <param name="jobId">任务ID</param>
        [HttpPost("{jobId}/run")]
        public void Run([FromRoute]string jobId)
        {
            RecurringJob.TriggerJob(jobId);
        }

        /// <summary>
        /// 删除定时任务
        /// </summary>
        /// <param name="jobId">任务ID</param>
        [HttpDelete("{jobId}")]
        public void Remove(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
        }

        /// <summary>
        /// 更新定时任务的 Cron 表达式
        /// </summary>
        /// <param name="jobId">任务ID</param>
        /// <param name="cronExpression">新的 Cron 表达式</param>
        [HttpPost("{jobId}")]
        public void Update([FromRoute] string jobId, [FromBody] string cronExpression)
        {
            var job = this.jobs.FirstOrDefault(j => j.Title.Underscore() == jobId);
            if (job != null)
            {
                RecurringJob.AddOrUpdate(jobId, () => job.Start(), cronExpression);
                this.logger.LogInformation("更新定时任务: {JobId}, 新 Cron: {Cron}", jobId, cronExpression);
            }
        }
    }
}
