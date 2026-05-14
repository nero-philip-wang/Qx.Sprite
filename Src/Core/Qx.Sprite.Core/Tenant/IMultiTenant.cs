// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    /// <summary>
    /// 多商户的商户标识
    /// </summary>
    public interface IMultiTenant
    {
        /// <summary>
        /// Gets or sets 商户id
        /// </summary>
        object? TenantId { get; set; }
    }

    /// <summary>
    /// 多商户的商户标识
    /// </summary>
    /// <typeparam name="T">商户id的类型</typeparam>
    public interface IMultiTenant<T> : IMultiTenant
    {
        /// <summary>
        /// Gets 商户id
        /// </summary>
        new T? TenantId { get; }
    }
}