// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 客户端信息
    /// </summary>
    public interface IClientInfo : IAppId
    {
        /// <summary>
        /// Gets or sets 名称
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets 类型
        /// </summary>
        ClientType Type { get; set; }

        /// <summary>
        /// Gets or sets 版本
        /// </summary>
        Version Version { get; set; }

        /// <summary>
        /// Gets a value indicating whether 版本是否有效
        /// </summary>
        /// <returns></returns>
        bool IsValidVersion { get; }
    }
}