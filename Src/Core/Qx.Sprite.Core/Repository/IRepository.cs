// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="TKey">实体主键类型</typeparam>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IRepository<TKey, TEntity> : IQueryable<TEntity>, IUnitOfWork
        where TEntity : class, IHasKey<TKey>
    {
        /// <summary>
        /// 设置查询过滤忽略标志
        /// </summary>
        /// <param name="ignoreFlag"></param>
        void SetQueryFilterIgnoreFlag(QueryFilterIgnoreFlag ignoreFlag);

        /// <summary>
        /// 获取查询数据集
        /// </summary>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuerySet(QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        #region 同步方法

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        TEntity Get(TKey id, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        /// <summary>
        /// 获取实体列表
        /// </summary>
        /// <typeparam name="TListItemDto">列表项DTO类型</typeparam>
        /// <param name="args"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        CountableList<TListItemDto> GetList<TListItemDto>(SearchArgs args, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        TEntity Add(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 添加实体列表
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        void AddRange(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                this.Add(item, false);
            }

            if (autoSave)
                this.SaveChanges();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        TEntity Update(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 更新实体列表
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        void UpdateRange(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                this.Update(item, false);
            }

            if (autoSave)
                this.SaveChanges();
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        void Remove(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 删除实体列表
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        void RemoveRange(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                this.Remove(item, false);
            }

            if (autoSave)
                this.SaveChanges();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        void BatchAddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="predicate"></param>
        void BatchDelete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        void BatchUpdate(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 截断
        /// </summary>
        void Truncate();

        #endregion 同步方法

        #region 异步方法

        /// <summary>
        /// 获取实体异步
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TKey id, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        /// <summary>
        /// 获取实体列表异步
        /// </summary>
        /// <typeparam name="TListItemDto">列表项DTO类型</typeparam>
        /// <param name="args"></param>
        /// <param name="ignoreFlag"></param>
        /// <returns></returns>
        Task<CountableList<TListItemDto>> GetListAsync<TListItemDto>(SearchArgs args, QueryFilterIgnoreFlag ignoreFlag = QueryFilterIgnoreFlag.NoIgnore);

        /// <summary>
        /// 添加实体异步
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task<TEntity> AddAsync(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 添加实体列表异步
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        async Task AddRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                await this.AddAsync(item, false);
            }

            if (autoSave)
                await this.SaveChangesAsync();
        }

        /// <summary>
        /// 更新实体异步
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 更新实体列表异步
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        async Task UpdateRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                await this.UpdateAsync(item, false);
            }

            if (autoSave)
                await this.SaveChangesAsync();
        }

        /// <summary>
        /// 删除实体异步
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity, bool autoSave = false);

        /// <summary>
        /// 删除实体列表异步
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="autoSave"></param>
        /// <returns></returns>
        async Task RemoveRangeAsync(IEnumerable<TEntity> entities, bool autoSave = false)
        {
            foreach (var item in entities)
            {
                await this.RemoveAsync(item, false);
            }

            if (autoSave)
                await this.SaveChangesAsync();
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task BatchAddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="predicate"></param>
        Task BatchDeleteAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updateExpression"></param>
        Task BatchUpdateAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity>> updateExpression);

        /// <summary>
        /// 截断
        /// </summary>
        Task TruncateAsync();

        #endregion 异步方法
    }
}