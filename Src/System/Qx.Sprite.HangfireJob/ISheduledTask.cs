// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System.Threading.Tasks;

    /// <summary>
    /// 定时任务接口
    /// </summary>
    public interface ISheduledTask
    {
        /// <summary>
        /// Gets 名称
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets cron 表达式
        /// </summary>
        string CronExpression { get; }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        Task Start();
    }
}