// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 定义一个单元操作内的功能，管理单元操作内涉及的所有上下文对象及其事务
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 启用事务，事务代码写在 UnitOfWork.EnableTransaction() 与 UnitOfWork.Commit() 之间
        /// </summary>
        IDisposable? EnableTransaction();

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        void Commit();

        /// <summary>
        /// 提交当前上下文的事务更改
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// 回滚所有事务
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// 持久化到数据库
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 持久化到数据库
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 放弃当前上下文的所有更改
        /// </summary>
        void ResetChanges();
    }
}