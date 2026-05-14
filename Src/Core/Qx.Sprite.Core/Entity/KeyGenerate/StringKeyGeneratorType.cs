// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 字符串主键生成器类型
    /// </summary>
    public enum StringKeyGeneratorType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

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