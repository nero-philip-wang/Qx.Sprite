// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx. Sprite.EFCore
{
    using System;
    using System. Collections.Generic;
    using System. Linq;
    using System. Linq. Dynamic. Core;
    using System. Linq. Expressions;
    using System. Threading;
    using System. Threading. Tasks;
    using AutoMapper;
    using global::EFCore. BulkExtensions;
    using Microsoft. EntityFrameworkCore;
    using Microsoft. Extensions. Options;
    using Qx. Sprite. Core;
    using Qx. Sprite. Domain;

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
            var tContext = AppDbContext. EntityInDbContext.ContainsKey(typeof(TEntity).GUID) ? AppDbContext.EntityInDbContext[typeof(TEntity).GUID] : null;
            tContext.CheckNotNull("DbContext Type");
            var dbContext = provider.GetService(tContext) as AppDbContext;
            dbContext.CheckNotNull("dbContext");
            this. DbContext = dbContext;
        }

        /// <inheritdoc/>
        public DbContext DbContext { get; init; }

        /// <inheritdoc/>
        public DbSet<TKey, TEntity> DbSet => this. DbContext.Set< TEntity>();

        /// <inheritdoc/>
        public override IQueryable<TCriteria> GetQuerySet<TCriteria>( QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore) where TCriteria : class
        {
            IQueryable<TCriteria> set = this. DbSet;
            if (ignoreFlag == QueryFilterIgnoreFlag.IgnoreAll)
            {
                return set;
            }
            else
            {
                var hasSoftDelete = typeof(TEntity). GetInterface(nameof(ISoftDelete)) != null && !ignoreFlag.HasFlag(QueryFilterIgnoreFlag.IgnoreSoftDelete);
                var hasTenant = typeof(TEntity). GetInterfaces().Any(c => c == typeof(IMultiTenant)) && !ignoreFlag.HasFlag(QueryFilterIgnoreFlag.IgnoreTenant);

                if (hasTenant && (this.tenant?.TenantId != null))
                {
                    set = set. Where("TenantId == @0 || TenantId == null", this.tenant. TenantId);
                }

                return hasSoftDelete ? set.Where("!IsDeleted") : set;
            }
        }

        /// <inheritdoc/>
        public override TEntity Add( TEntity entity, bool autoSave = false)
        {
            entity = base. Add(entity, autoSave);
            var savedEntity = this. DbSet.Add(entity).Entity;
            if (autoSave) this. SaveChanges();
            return savedEntity;
        }

        /// <inheritdoc/>
        public override TEntity Update( TEntity entity, bool autoSave = false)
        {
            entity = base. Update(entity, autoSave);
            this. DbSet.Attach(entity);
            var updatedEntity = this. DbSet.Update(entity).Entity;
            if (autoSave) this. SaveChanges();
            return updatedEntity;
        }

        /// <inheritdoc/>
        public override void Remove( TEntity entity, bool autoSave = false)
        {
            base. Remove(entity, autoSave);

            // 如果有软删除执行软删除
            if (entity is ISoftDelete)
            {
                this. DbSet.Attach(entity);
                var updatedEntity = this. DbSet.Update(entity).Entity;
                if (autoSave) this. SaveChanges();
            }
            else
            {
                this. DbSet.Remove(entity);
                if (autoSave) this. SaveChanges();
            }
        }

        /// <inheritdoc/>
        public override async Task< TEntity> AddAsync( TEntity entity, bool autoSave = false)
        {
            entity = await base. AddAsync(entity, false);
            var savedEntity = this. DbSet.Add(entity).Entity;
            if (autoSave) await this. SaveChangesAsync();
            return savedEntity;
        }

        /// <inheritdoc/>
        public override async Task< TEntity> UpdateAsync( TEntity entity, bool autoSave = false)
        {
            entity = await base. UpdateAsync(entity, false);
            this. DbSet.Attach(entity);
            var updatedEntity = this. DbSet.Update(entity).Entity;
            if (autoSave) await this. SaveChangesAsync();
            return updatedEntity;
        }

        /// <inheritdoc/>
        public override async Task RemoveAsync( TEntity entity, bool autoSave = false)
        {
            await base. RemoveAsync(entity, false);

            // 如果有软删除执行软删除
            if (entity is ISoftDelete)
            {
                this. DbSet.Attach(entity);
                var updatedEntity = this. DbSet.Update(entity).Entity;
            }
            else
            {
                this. DbSet.Remove(entity);
            }
            if (autoSave) await this. SaveChangesAsync();
        }

        /// <inheritdoc/>
        public override void Commit()
        {
            if (this. DbContext.Database.CurrentTransaction != null)
                this. DbContext.Database.CommitTransaction();
        }

        /// <inheritdoc/>
        public override void Rollback()
        {
            if (this. DbContext.Database.CurrentTransaction != null)
                this. DbContext.Database.RollbackTransaction();
        }

        /// <inheritdoc/>
        public override int SaveChanges()
        {
            try
            {
                return this. DbContext.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new BusinessException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc/>
        public override async Task CommitAsync()
        {
            if (this. DbContext.Database.CurrentTransaction != null)
                await this. DbContext.Database.CommitTransactionAsync();
        }

        /// <inheritdoc/>
        public override async Task RollbackAsync()
        {
            if (this. DbContext.Database.CurrentTransaction != null)
                await this. DbContext.Database.RollbackTransactionAsync();
        }

        /// <inheritdoc/>
        public override Task< int> SaveChangesAsync()
        {
            try
            {
                return this. DbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                throw new BusinessException(e.InnerException?.Message ?? e.Message);
            }
        }

        /// <inheritdoc/>
        public override void ResetChanges()
        {
            this. DbContext.ChangeTracker.Clear();
        }

        /// <inheritdoc/>
        public override IDisposable EnableTransaction()
        {
            if (this. DbContext.Database.CurrentTransaction == null)
                return this. DbContext.Database.BeginTransaction();
            else
                return this. DbContext.Database.CurrentTransaction;
        }

        /// <inheritdoc/>
        public override void BatchDelete<TCriteria>( Expression< Func<TCriteria, bool>> predicate) where TCriteria : class
        {
            this. DbSet.OfType<TCriteria>().Where(predicate).ExecuteDelete();
        }

        /// <inheritdoc/>
        public override void BatchUpdate<TCriteria>( Expression< Func<TCriteria, TEntity>> predicate, Expression< Func<TCriteria, TEntity>> updateExpression) where TCriteria : class
        {
            var properties = ParseUpdateExpression(updateExpression);
            if (properties.Count == 0) return;

            this. DbSet.OfType<TCriteria>().Where(predicate).ExecuteUpdate(s => BuildSetPropertyCalls(s, properties));
        }

        private static Dictionary< string, (Expression Expression, Type PropertyType)> ParseUpdateExpression<TCriteria>(
            Expression< Func<TCriteria, TEntity>> updateExpression) where TCriteria : class
        {
            var result = new Dictionary< string, (Expression, Type)>();

            if (updateExpression.Body is not NewExpression newExpression)
                return result;

            var members = newExpression.Members;
            var arguments = newExpression.Arguments;

            for (int i = 0; i < members.Count; i++)
            {
                var propertyName = members[i].Name;
                var valueExpr = arguments[i];
                var entityProperty = typeof(TEntity).GetProperty(propertyName);
                if (entityProperty == null) continue;

                result[propertyName] = (valueExpr, entityProperty. PropertyType);
            }

            return result;
        }

        private static SetPropertyCalls<TCriteria, TEntity> BuildSetPropertyCalls<TCriteria>(
            SetPropertyCalls<TCriteria, TEntity> setPropertyCalls,
            Dictionary< string, (Expression Expression, Type PropertyType)> properties) where TCriteria : class
        {
            foreach (var (propertyName, (valueExpr, propertyType)) in properties)
            {
                var entityProperty = typeof(TEntity).GetProperty(propertyName);
                if (entityProperty == null) continue;

                if (valueExpr is MemberExpression me && me.Expression is ParameterExpression)
                {
                    setPropertyCalls. SetProperty(
                        entityProperty. GetGetMethod()!,
                        entityProperty. GetGetMethod()!);
                }
                else if (valueExpr is ConstantExpression ce)
                {
                    var convertedValue = ConvertValue(ce.Constant, propertyType);
                    setPropertyCalls. SetProperty(entityProperty. GetGetMethod()!, convertedValue);
                }
            }

            return setPropertyCalls;
        }

        private static object? ConvertValue( object? value, Type targetType)
        {
            if (value == null) return null;
            if (targetType. IsAssignableFrom(value. GetType())) return value;
            return Convert. ChangeType(value, targetType);
        }

        /// <inheritdoc/>
        public override void Truncate()
        {
            this. DbContext.Truncate< TEntity>();
        }

        /// <inheritdoc/>
        public override Task BatchDeleteAsync<TCriteria>( Expression< Func<TCriteria, bool>> predicate) where TCriteria : class
        {
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public override Task BatchUpdateAsync<TCriteria>( Expression< Func<TCriteria, TEntity>> predicate, Expression< Func<TCriteria, TEntity>> updateExpression) where TCriteria : class
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task TruncateAsync()
        {
            this. DbContext.TruncateAsync< TEntity>();
            return Task.CompletedTask;
        }
    }
}