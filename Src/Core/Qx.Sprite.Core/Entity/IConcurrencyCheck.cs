// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 并发检查接口
    /// </summary>
    public interface IConcurrencyCheck
    {
        /// <summary>
        /// Gets or sets 并发时间戳
        /// </summary>
        long ConcurrencyStamp { get; set; }
    }
}