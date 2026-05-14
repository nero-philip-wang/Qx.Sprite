// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 定义实体的主键
    /// </summary>
    /// <typeparam name="T">主键类型</typeparam>
    public interface IHasKey<T>
    {
        /// <summary>
        /// Gets or sets 主键
        /// </summary>
        T Id { get; set; }
    }
}