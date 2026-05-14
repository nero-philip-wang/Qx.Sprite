// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 创建审计接口
    /// </summary>
    public interface ICreationAudited
    {
        /// <summary>
        /// Gets or sets 创建时间
        /// </summary>
        DateTime CreationTime { get; set; }

        /// <summary>
        /// Gets or sets 创建人
        /// </summary>
        string? Creator { get; set; }
    }
}