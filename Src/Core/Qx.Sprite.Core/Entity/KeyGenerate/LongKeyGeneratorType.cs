// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 长整型主键生成器类型
    /// </summary>
    public enum LongKeyGeneratorType
    {
        /// <summary>
        /// 自动增长
        /// </summary>
        AutoIncrease = 0,

        /// <summary>
        /// Twitter SnowFlake
        /// </summary>
        TwitterSnowFlake,

        /// <summary>
        /// 每日序列号
        /// </summary>
        DailySerialNo,

        /// <summary>
        /// 序列号
        /// </summary>
        SerialNo,
    }
}