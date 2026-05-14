// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EFCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using global::EFCore.BulkExtensions;
    using MapsterMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    /// <summary>
    /// EF数据库仓储实现
    /// </summary>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    [ServiceType(typeof(IEfRepository<,>))]
    public class EfRepository<TKey, TEntity> : Repository<TKey, TEntity>, IEfRepository<TKey, TEntity>, IScoped
        where TEntity : class, IHasKey<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{Key, TEntity}"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="mapper"></param>
        /// <param name="tenant"></param>
        /// <param name="user"></param>
        public EfRepository(IServiceProvider provider, IMapper mapper, IMultiTenant tenant, IUserInfo user)
            : base(provider, mapper, tenant, user)
        {
            // 获取 DbContext
            var tContext = AppDbContext.EntityInDbContext.ContainsKey(typeof(TEntity).GUID) ? AppDbContext.EntityInDbContext[typeof(TEntity).GUID] : null;
            tContext.CheckNotNull("DbContext Type");
            var dbContext = provider.GetService(tContext) as AppDbContext;
            dbContext.CheckNotNull("dbContext");
            this.DbContext = dbContext;
        }

        /// <inheritdoc/>
        public DbContext DbContext { get; init; }

        /// <inheritdoc/>
        public DbSet<TEntity> DbSet => this.DbContext.Set<TEntity>();

        /// <inheritdoc/>
        public override IQueryable<TEntity> GetQuerySet(QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore)
        {
            IQueryable<TEntity> set = this.DbSet;
            if (ignoreFlag == QueryFilterIgnoreFlag.IgnoreAll)
            {
                return set;
            }
            else
            {
                var hasSoftDelete = typeof(TEntity).GetInterface(nameof(ISoftDelete)) != null && !ignoreFlag.HasFlag(QueryFilterIgnoreFlag.IgnoreSoftDelete);
                var hasTenant = typeof(TEntity).GetInterfaces().Any(c => c == typeof(IMultiTenant)) && !ignoreFlag.HasFlag(QueryFilterIgnoreFlag.IgnoreTenant);

                if (hasTenant && (this.tenant?.TenantId != null))
                {
                    set = set.Where("TenantId == @0 || TenantId == null", this.tenant.TenantId);
                }

                return hasSoftDelete ? set.Where("!IsDeleted") : set;
            }
        }

        /// <inheritdoc/>
        public override TEntity Add(TEntity entity, bool autoSave = false)
        {
            entity = base.Add(entity, autoSave);
            var savedEntity = this.DbSet.Add(entity).Entity;
            if (autoSave) this.SaveChanges();
            return savedEntity;
        }

        /// <inheritdoc/>
        public override TEntity Update(TEntity entity, bool autoSave = false)
        {
            entity = base.Update(entity, autoSave);
            this.DbSet.Attach(entity);
            if (autoSave) this.SaveChanges();
            return entity;
        }

        /// <inheritdoc/>
        public override void Remove(TEntity entity, bool autoSave = false)
        {
            base.Remove(entity, autoSave);

            // 如果有软删除执行软删除
            if (entity is ISoftDelete)
            {
                this.DbSet.Attach(entity);
                if (autoSave) this.SaveChanges();
            }
            else
            {
                this.DbSet.Remove(entity);
                if (autoSave) this.SaveChanges();
            }
        }

        /// <inheritdoc/>
        public override async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false)
        {
            entity = await base.AddAsync(entity, false);
            var savedEntity = this.DbSet.Add(entity).Entity;
            if (autoSave) await this.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public override async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            entity = await base.UpdateAsync(entity, false);
            this.DbSet.Attach(entity);
            if (autoSave) await this.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc/>
        public override async Task RemoveAsync(TEntity entity, bool autoSave = false)
        {
            await base.RemoveAsync(entity, false);

            // 如果有软删除执行软删除
            if (entity is ISoftDelete)
            {
                this.DbSet.Attach(entity);
            }
            else
            {
                this.DbSet.Remove(entity);
            }

            if (autoSave) await this.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public override void Commit()
        {
            if (this.DbContext.Database.CurrentTransaction != null)
                this.DbContext.Database.CommitTransaction();
        }

        /// <inheritdoc/>
        public override void Rollback()
        {
            if (this.DbContext.Database.CurrentTransaction != null)
                this.DbContext.Database.RollbackTransaction();
        }

        /// <inheritdoc/>
        public override int SaveChanges()
        {
            try
            {
                return this.DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new BusinessException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc/>
        public override async Task CommitAsync()
        {
            if (this.DbContext.Database.CurrentTransaction != null)
                await this.DbContext.Database.CommitTransactionAsync();
        }

        /// <inheritdoc/>
        public override async Task RollbackAsync()
        {
            if (this.DbContext.Database.CurrentTransaction != null)
                await this.DbContext.Database.RollbackTransactionAsync();
        }

        /// <inheritdoc/>
        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return this.DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new BusinessException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc/>
        public override void ResetChanges()
        {
            this.DbContext.ChangeTracker.Clear();
        }

        /// <inheritdoc/>
        public override IDisposable EnableTransaction()
        {
            if (this.DbContext.Database.CurrentTransaction == null)
                return this.DbContext.Database.BeginTransaction();
            else
                return this.DbContext.Database.CurrentTransaction;
        }

        /// <inheritdoc/>
        public override void BatchDelete(Expression<Func<TEntity, bool>> predicate)
        {
            this.DbSet.Where(predicate).ExecuteDelete();
        }

        /// <inheritdoc/>
        public override void BatchUpdate(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
#pragma warning disable CS0618 // 类型或成员已过时
            this.DbSet.Where(predicate).BatchUpdate(updateExpression);
#pragma warning restore CS0618 // 类型或成员已过时
        }

        /// <inheritdoc/>
        public override void Truncate()
        {
            this.DbContext.Truncate<TEntity>();
        }

        /// <inheritdoc/>
        public override Task BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return this.DbSet.Where(predicate).ExecuteDeleteAsync();
        }

        /// <inheritdoc/>
        public override async Task BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression)
        {
#pragma warning disable CS0612 // 类型或成员已过时
#pragma warning disable CS0618 // 类型或成员已过时
            await this.DbSet.Where(predicate).BatchUpdateAsync(updateExpression);
#pragma warning restore CS0618 // 类型或成员已过时
#pragma warning restore CS0612 // 类型或成员已过时
            return;
        }

        /// <inheritdoc/>
        public override Task TruncateAsync()
        {
            return this.DbContext.TruncateAsync<TEntity>();
        }

        /// <inheritdoc/>
        public override void BatchAddRange(IEnumerable<TEntity> entities)
        {
            this.DbContext.BulkInsert(entities);
        }

        /// <inheritdoc/>
        public override Task BatchAddRangeAsync(IEnumerable<TEntity> entities)
        {
            return this.DbContext.BulkInsertAsync(entities);
        }
    }
}