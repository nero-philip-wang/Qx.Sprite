// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// 实现静态方法OnModelCreating的注册和调用
    /// </summary>
    public interface IOnModelCreating
    {
        /// <summary>
        /// 模型创建时调用
        /// </summary>
        /// <param name="modelBuilder"></param>
        void OnModelCreating(ModelBuilder modelBuilder);
    }
}