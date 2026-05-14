// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 定义实体的软删除信息
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        bool IsDeleted { get; set; }
    }
}