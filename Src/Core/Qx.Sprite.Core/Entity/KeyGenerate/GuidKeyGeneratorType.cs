// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 有序Guid类型
    /// </summary>
    public enum GuidKeyGeneratorType
    {
        /// <summary>
        /// 无序的
        /// </summary>
        NoSequential,

        /// <summary>
        /// 将GUID当作字符串来处理的前排序，MySql，Sqlite，PostgreSQL中适用
        /// </summary>
        SequentialAsString,

        /// <summary>
        /// 将GUID作为二进制处理的排序，Oracle，PostgreSQL中适用
        /// </summary>
        SequentialAsBinary,

        /// <summary>
        /// 按GUID的最后一段来排序，SqlServer中适用
        /// </summary>
        SequentialAtEnd,
    }
}