// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// fw服务器信息
    /// </summary>
    public interface IServeInfo
    {
        /// <summary>
        /// Gets 名称
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets 服务器版本
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets 服务器基础地址
        /// </summary>
        string ServeBaseUrl { get; }
    }
}