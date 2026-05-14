// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// 用户信息
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// Gets or sets 用户在当前系统的id，一般是member表
        /// </summary>
        object? Id { get; set; }

        /// <summary>
        /// Gets 用户的全局id，一般是user表
        /// </summary>
        object? UnionId { get; }

        /// <summary>
        /// Gets 用户在当前系统的用户昵称
        /// </summary>
        string? Title { get; }

        /// <summary>
        /// Gets 用户角色
        /// </summary>
        List<object> Role { get; }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns>字符串</returns>
        string? ToString()
        {
            return $"[\"{this.Id}\",\"{this.Title}\"]";
        }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    /// <typeparam name="T">用户id类型</typeparam>
    public interface IUserInfo<T> : IUserInfo
    {
        /// <summary>
        /// Gets 用户在当前系统的id，一般是member表
        /// </summary>
        new T? Id { get; }

        /// <summary>
        /// Gets 用户的全局id，一般是user表
        /// </summary>
        new T? UnionId { get; }
    }
}