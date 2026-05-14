// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 完整审计接口
    /// </summary>
    public interface IFullAudited : IModificationAudited, ISoftDelete
    {
        /// <summary>
        /// Gets or sets 删除人
        /// </summary>
        string? Deleter { get; set; }

        /// <summary>
        /// Gets or sets 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}