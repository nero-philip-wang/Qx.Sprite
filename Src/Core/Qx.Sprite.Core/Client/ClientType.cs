// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 客户端类型
    /// </summary>
    [Flags]
    public enum ClientType : ushort
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 服务器
        /// </summary>
        Server = 0x1 << 0,

        /// <summary>
        /// 浏览器
        /// </summary>
        Browser = 0x1 << 1,

        /// <summary>
        /// 应用
        /// </summary>
        App = 0x1 << 2,

        /// <summary>
        /// 物联网
        /// </summary>
        IoT = 0x1 << 3,

        /// <summary>
        /// 其他
        /// </summary>
        Other = 0x1 << 15,
    }
}