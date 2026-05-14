// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.BackgroundJob
{
    using System;
    using Hangfire.Common;
    using Mapster;
    using Qx.Sprite.Core;

    /// <summary>
    /// Hangfire 定时任务数据传输对象
    /// </summary>
    public class RecurringJobDto : IMapperConfiguration
    {
        /// <summary>
        /// Gets or sets 任务标识
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets cron 表达式
        /// </summary>
        public string Cron { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 队列名称
        /// </summary>
        public string Queue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 下次执行时间
        /// </summary>
        public DateTime? NextExecution { get; set; }

        /// <summary>
        /// Gets or sets 上次任务Id
        /// </summary>
        public string LastJobId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 上次任务状态
        /// </summary>
        public string LastJobState { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 上次执行时间
        /// </summary>
        public DateTime? LastExecution { get; set; }

        /// <summary>
        /// Gets or sets 创建时间
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 是否已移除
        /// </summary>
        public bool Removed { get; set; }

        /// <summary>
        /// Gets or sets 时区Id
        /// </summary>
        public string TimeZoneId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 错误信息
        /// </summary>
        public string Error { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets 重试次数
        /// </summary>
        public int RetryAttempt { get; set; }

        /// <inheritdoc/>
        public void AddMapper(TypeAdapterConfig cfg)
        {
            cfg.NewConfig<Hangfire.Storage.RecurringJobDto, RecurringJobDto>();
        }
    }
}
