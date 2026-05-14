// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 定义实体的修改审计信息
    /// </summary>
    public interface IModificationAudited : ICreationAudited
    {
        /// <summary>
        /// Gets or sets 最后修改时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Gets or sets 最后修改人
        /// </summary>
        string? LastModifier { get; set; }
    }
}