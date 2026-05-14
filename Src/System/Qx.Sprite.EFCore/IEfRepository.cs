// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Domain;

    /// <summary>
    /// EF数据库仓储接口
    /// </summary>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEfRepository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IHasKey<TKey>
    {
        /// <summary>
        /// Gets EF数据库上下文
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Gets 数据库表
        /// </summary>
        DbSet<TEntity> DbSet { get; }
    }
}