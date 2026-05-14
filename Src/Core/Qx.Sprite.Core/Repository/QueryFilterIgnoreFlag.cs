// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 查询过滤忽略标志
    /// </summary>
    [Flags]
    public enum QueryFilterIgnoreFlag : ushort
    {
        /// <summary>
        /// 不忽略
        /// </summary>
        NoIgnore = 0,

        /// <summary>
        /// 忽略软删除
        /// </summary>
        IgnoreSoftDelete = 1 << 0,

        /// <summary>
        /// 忽略租户
        /// </summary>
        IgnoreTenant = 1 << 1,

        /// <summary>
        /// 忽略所有
        /// </summary>
        IgnoreAll = 0xffff,
    }
}