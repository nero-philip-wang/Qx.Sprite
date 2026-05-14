// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MapsterMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Qx.Sprite.Core;
    using Qx.Sprite.DDD.Respository;

    /// <summary>
    /// 仓库基类
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TEntity">实体</typeparam>
    public abstract partial class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IHasKey<TKey>
    {
        /// <summary>
        /// 主键名称
        /// </summary>
        protected const string Id = nameof(IHasKey<TKey>.Id);

        /// <summary>
        /// 容器
        /// </summary>
        protected readonly IServiceProvider provider;

        /// <summary>
        /// 映射器
        /// </summary>
        protected readonly IMapper mapper;

        /// <summary>
        /// 多租户
        /// </summary>
        protected readonly IMultiTenant? tenant;

        /// <summary>
        /// 用户
        /// </summary>
        protected readonly IUserInfo? user;

        /// <summary>
        /// 查询过滤忽略标志
        /// </summary>
        protected QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="mapper"></param>
        /// <param name="tenant"></param>
        /// <param name="user"></param>
        public Repository(IServiceProvider provider, IMapper mapper, IMultiTenant tenant, IUserInfo user)
        {
            this.provider = provider;
            this.mapper = mapper;
            this.tenant = tenant;
            this.user = user;
        }

        #region CURD

        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        public abstract IQueryable<TEntity> GetQuerySet(QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        public virtual TEntity Get(TKey id, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore)
        {
            var collection = this.GetQuerySet(ignoreFlag);
            var dbone = collection.Where($"{Id}==@0", id).FirstOrDefault();
            return dbone ?? throw new BusinessException($"未找到 {typeof(TEntity)} {id}", System.Net.HttpStatusCode.NotFound);
        }

        /// <summary>
        /// 按条件查询获取集合
        /// </summary>
        /// <typeparam name="TListItemDto">列表类型</typeparam>
        /// <param name="args"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        public virtual CountableList<TListItemDto> GetList<TListItemDto>(SearchArgs args, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore)
        {
            var collection = this.GetQuerySet(ignoreFlag);
            return collection.Page<TListItemDto>(args);
        }

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public virtual TEntity Add(TEntity entity, bool autoSave = false)
        {
            this.SetKey(entity);
            this.SetEntityForAdd(entity);

            var props = EmitPropertyAccessor.CreateAccessors(typeof(TEntity), p => p.DeclaringType?.IsClass == true && p.PropertyType != typeof(string));
            foreach (var item in props)
            {
                var field = item.Value.Getter(entity);
                if (field is IEnumerable array && !(field is string))
                {
                    foreach (var c in array)
                    {
                        this.SetEntityForAdd(c);
                    }
                }
                else
                {
                    this.SetEntityForAdd(field);
                }
            }

            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public virtual TEntity Update(TEntity entity, bool autoSave = false)
        {
            this.SetEntityForUpdate(entity);
            var props = EmitPropertyAccessor.CreateAccessors(typeof(TEntity), p => p.DeclaringType?.IsClass == true && p.PropertyType != typeof(string));
            foreach (var item in props)
            {
                var field = item.Value.Getter(entity);
                if (field is IEnumerable array && !(field is string))
                {
                    foreach (var c in array)
                    {
                        this.SetEntityForUpdate(c);
                    }
                }
                else
                {
                    this.SetEntityForUpdate(field);
                }
            }

            return entity;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        public virtual void Remove(TEntity entity, bool autoSave = false)
        {
            this.SetEntityForRemove(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> GetAsync(TKey id, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore)
        {
            return await this.GetAsync(id, ignoreFlag);
        }

        /// <inheritdoc/>
        public virtual async Task<CountableList<TListItemDto>> GetListAsync<TListItemDto>(SearchArgs args, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore)
        {
            return await Task.FromResult(this.GetList<TListItemDto>(args, ignoreFlag));
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> AddAsync(TEntity entity, bool autoSave = false)
        {
            this.Add(entity, autoSave);
            return await Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            this.Update(entity, autoSave);
            return await Task.FromResult(entity);
        }

        /// <inheritdoc/>
        public virtual Task RemoveAsync(TEntity entity, bool autoSave = false)
        {
            this.Remove(entity, autoSave);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public abstract void BatchAddRange(IEnumerable<TEntity> entities);

        /// <inheritdoc/>
        public abstract void BatchDelete(Expression<Func<TEntity, bool>> predicate);

        /// <inheritdoc/>
        public abstract void BatchUpdate(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <inheritdoc/>
        public abstract void Truncate();

        /// <inheritdoc/>
        public abstract Task BatchAddRangeAsync(IEnumerable<TEntity> entities);

        /// <inheritdoc/>
        public abstract Task BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <inheritdoc/>
        public abstract Task BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <inheritdoc/>
        public abstract Task TruncateAsync();

        #endregion CURD

        #region UnitOfWork

        /// <inheritdoc/>
        public abstract IDisposable? EnableTransaction();

        /// <inheritdoc/>
        public abstract void Commit();

        /// <inheritdoc/>
        public abstract Task CommitAsync();

        /// <inheritdoc/>
        public abstract void Rollback();

        /// <inheritdoc/>
        public abstract Task RollbackAsync();

        /// <inheritdoc/>
        public abstract int SaveChanges();

        /// <inheritdoc/>
        public abstract Task<int> SaveChangesAsync();

        /// <inheritdoc/>
        public abstract void ResetChanges();

        #endregion UnitOfWork

        /// <summary>
        /// 设置实体主键
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetKey(TEntity entity)
        {
            var idField = typeof(TEntity).GetProperty(Id);
            idField.CheckNotNull($"{typeof(TEntity).Name} does not contains {Id}");
            if (idField.HasAttribute<KeyGeneratorAttribute>())
            {
                var keyGenerator = this.provider.GetKeyedService<IKeyGenerator>(idField.PropertyType.Name);
                keyGenerator.CheckNotNull(nameof(keyGenerator));

                var longCondition = idField.PropertyType == typeof(long) && 0L.Equals(idField.GetValue(entity));
                var guidCondition = idField.PropertyType == typeof(Guid) && Guid.Empty.Equals(idField.GetValue(entity));
                var stringCondition = idField.PropertyType == typeof(string) && idField.GetValue(entity).IsNullOrEmpty();

                // Id为默认值，就设置id
                if (longCondition || stringCondition || guidCondition)
                {
                    var keyType = idField.GetAttribute<KeyGeneratorAttribute>().FirstOrDefault()!.Algorithm;
                    idField.SetValue(entity, keyGenerator.Next(keyType));
                }
            }
        }

        /// <summary>
        /// 设置实体的创建信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void SetEntityForAdd(object? entity)
        {
            if (entity is ICreationAudited create)
            {
                create.CreationTime = DateTime.Now;
                create.Creator = this.user?.ToString();
            }

            if (entity is IMultiTenant multiTenant)
            {
                if (multiTenant.TenantId == default)
                    multiTenant.TenantId = this.tenant?.TenantId;
            }
        }

        /// <summary>
        /// 设置实体的更新信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        protected virtual void SetEntityForUpdate(object? entity)
        {
            if (entity is IModificationAudited create)
            {
                create.LastModificationTime = DateTime.Now;
                create.LastModifier = this.user?.ToString();
            }
        }

        /// <summary>
        /// 设置实体的删除信息
        /// </summary>
        /// <param name="entity"></param>
        protected virtual void SetEntityForRemove(TEntity entity)
        {
            // 如果有软删除执行软删除
            if (entity is ISoftDelete delete)
            {
                delete.IsDeleted = true;
                if (entity is IFullAudited fullAudited)
                {
                    fullAudited.DeletionTime = DateTime.Now;
                    fullAudited.Deleter = this.user?.ToString();
                }
            }
        }
    }
}