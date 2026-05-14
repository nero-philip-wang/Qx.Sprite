// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// DateTime 扩展方法
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 时间戳原点
        /// </summary>
        public static readonly DateTime T1970 = new DateTime(1970, 1, 1).ToUniversalTime();

        /// <summary>
        /// 将 DateTime 转换为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long Timestamp(this DateTime time)
        {
            return (long)(time.ToUniversalTime() - T1970).TotalSeconds;
        }

        /// <summary>
        /// 将时间戳转换为 DateTime
        /// </summary>
        /// <param name="stamp"></param>
        /// <returns></returns>
        public static DateTime ParseFromTimestamp(long stamp)
        {
            return T1970.AddSeconds(stamp).ToLocalTime();
        }
    }
}