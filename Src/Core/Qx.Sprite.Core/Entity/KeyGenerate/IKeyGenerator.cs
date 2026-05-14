// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 主键生成器
    /// </summary>
    public interface IKeyGenerator
    {
        /// <summary>
        /// 获取一个TKey类型的主键数据
        /// </summary>
        /// <returns></returns>
        object Next(Enum algorithm);
    }

    /// <summary>
    /// 主键生成器接口
    /// </summary>
    /// <typeparam name="TKey">主键类型</typeparam>
    public interface IKeyGenerator<out TKey> : IKeyGenerator
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 获取一个TKey类型的主键数据
        /// </summary>
        /// <returns></returns>
        new TKey Next(Enum algorithm);

        /// <inheritdoc/>
        object IKeyGenerator.Next(Enum algorithm) => this.Next(algorithm);
    }
}