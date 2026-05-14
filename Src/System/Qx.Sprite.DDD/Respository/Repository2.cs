using AutoMapper;
using Microsoft.Extensions.Options;
using Qx.ApiFx.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Qx.ApiFx.DDD
{
    /// <summary>
    /// 仓库基类（抽象EF实体和MongoDB）
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<Key, TEntity> : IAsyncRepository<Key, TEntity>, IUnitOfWork
        where TEntity : class, IHasKey<Key>
    {
        private const string Id = "Id";
        private readonly ISnowFlake snowFlake;
        private readonly IUser<long> user;
        private readonly IMapper mapper;
        private readonly AppInfo appInfo;

        public Repository(ISnowFlake snowFlake, IMapper mapper, IUser<long> user, IOptions<AppInfo> options)
        {
            this.snowFlake = snowFlake;
            this.user = user;
            this.mapper = mapper;
            this.appInfo = options.Value;
        }

        #region IQueryable

        public Type ElementType => QuerySet(includeDetails, isIgnoreQueryFilters).AsQueryable().ElementType;

        public Expression Expression => QuerySet(includeDetails, isIgnoreQueryFilters).AsQueryable().Expression;

        public IQueryProvider Provider => QuerySet(includeDetails, isIgnoreQueryFilters).AsQueryable().Provider;

        public IEnumerator<TEntity> GetEnumerator() => QuerySet(includeDetails, isIgnoreQueryFilters).AsQueryable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion IQueryable

        #region Protected

        protected bool isIgnoreQueryFilters { get; set; } = false;
        protected bool includeDetails { get; set; } = false;

        /// <summary>
        /// 设置实体主键
        /// </summary>
        /// <param name="entity"></param>
        protected void SetKey(TEntity entity)
        {
            var idField = typeof(TEntity).GetProperty(Id);
            //Id为默认值，就设置id
            if (idField.PropertyType == typeof(long) && idField.GetValue(entity).Equals((long)0) && idField.HasAttribute<SnowFlakeAttribute>())
                idField.SetValue(entity, snowFlake.GenericId());
            else if (idField.PropertyType == typeof(string) && idField.GetValue(entity).IsNullOrEmpty() && idField.HasAttribute<SnowFlakeAttribute>())
                idField.SetValue(entity, snowFlake.GenericId().ToString());
            else if (idField.PropertyType == typeof(Guid) && idField.GetValue(entity).Equals(Guid.Empty))
                idField.SetValue(entity, Guid.NewGuid());
        }

        #endregion Protected

        #region CURD

        /// <summary>
        /// 获取查询集合
        /// </summary>
        /// <param name="isIgnoreQueryFilters"></param>
        /// <returns></returns>
        public abstract IQueryable<TEntity> QuerySet(bool includeDetails, bool isIgnoreQueryFilters = false);

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isIgnoreQueryFilters"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetAsync(Key id, bool isIgnoreQueryFilters = false, bool includeDetails = true)
        {
            var collection = QuerySet(includeDetails, isIgnoreQueryFilters);
            var dbone = collection.Where($"{Id}==@0", id).FirstOrDefault();
            return dbone ?? throw new BusinessException(System.Net.HttpStatusCode.NotFound, $"未找到 {typeof(TEntity)} {id}");
        }

        /// <summary>
        /// 按条件查询获取集合
        /// </summary>
        /// <typeparam name="TListItemEntityDto"></typeparam>
        /// <param name="args"></param>
        /// <param name="isIgnoreQueryFilters"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public virtual async Task<CountableList<TListItemEntityDto>> GetListAsync<TListItemEntityDto>(SearchArgs args, bool isIgnoreQueryFilters = false, bool includeDetails = false)
        {
            var collection = QuerySet(includeDetails, isIgnoreQueryFilters);
            return collection.Page<TListItemEntityDto>(args, mapper);
        }

        /// <summary>
        /// 按条件查询获取集合
        /// </summary>
        /// <typeparam name="TListItemEntityDto"></typeparam>
        /// <param name="func"></param>
        /// <param name="isIgnoreQueryFilters"></param>
        /// <param name="includeDetails"></param>
        /// <returns></returns>
        public virtual async Task<CountableList<TListItemEntityDto>> GetListAsync<TListItemEntityDto>(IQueryable<TEntity> query, bool isIgnoreQueryFilters = false, bool includeDetails = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> CreateAsync(TEntity entity, bool autoSave = false)
        {
            SetKey(entity);
            if (entity is ICreationAudited)
            {
                var create = entity as ICreationAudited;
                create.CreationTime = DateTime.Now;
                create.Creator = $"[{user.Id},\"{user.Title}\"]";
            }
            if (entity is IMultiTenant)
            {
                var tenant = entity as IMultiTenant;
                if (appInfo.TenantId.HasValue && tenant.GeneralTenantId == default)
                    tenant.GeneralTenantId = appInfo.TenantId;
            }
            return entity;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false)
        {
            if (entity is IModificationAudited)
            {
                var create = entity as IModificationAudited;
                create.LastModificationTime = DateTime.Now;
                create.LastModifier = $"[{user.Id},\"{user.Title}\"]";
            }
            return entity;
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TEntity entity, bool autoSave = false)
        {
            //如果有软删除执行软删除
            if (entity is ISoftDelete)
            {
                (entity as ISoftDelete).IsDeleted = true;
                if (entity is IFullAudited)
                {
                    var create = entity as IFullAudited;
                    create.DeletionTime = DateTime.Now;
                    create.Deleter = $"[{user.Id},\"{user.Title}\"]";
                }
            }
        }

        #endregion CURD

        public abstract bool HasCommitted { get; }
        public abstract bool IsEnabledTransaction { get; }

        public abstract void EnableTransaction();

        public abstract void Commit(CancellationToken cancellationToken = default);

        public abstract void Rollback(CancellationToken cancellationToken = default);

        public abstract void Dispose();
    }
}