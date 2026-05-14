// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System.Collections.Generic;
    using Hangfire.Storage;

    /// <summary>
    /// 定时任务管理器
    /// </summary>
    public interface ISheduledTaskService
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        List<RecurringJobDto> All();

        /// <summary>
        /// 初始化定时任务
        /// </summary>
        void Init();

        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobId"></param>
        void Remove(string jobId);

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobId"></param>
        void Run(string jobId);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="cronExpression"></param>
        void Update(string jobId, string cronExpression);
    }
}