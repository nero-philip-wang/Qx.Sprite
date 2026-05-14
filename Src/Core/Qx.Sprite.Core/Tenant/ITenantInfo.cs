// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 商户信息
    /// </summary>
    public interface ITenantInfo : IMultiTenant
    {
        /// <summary>
        /// Gets 商户名称
        /// </summary>
        string? Title { get; }

        /// <summary>
        /// Gets 商户配置
        /// </summary>
        object? Options { get; }
    }

    /// <summary>
    /// 商户信息
    /// </summary>
    /// <typeparam name="T">商户id的类型</typeparam>
    public interface ITenantInfo<T> : IMultiTenant<T>
    {
        /// <summary>
        /// Gets 商户名称
        /// </summary>
        string? Title { get; }

        /// <summary>
        /// Gets 商户配置
        /// </summary>
        object? Options { get; }
    }
}