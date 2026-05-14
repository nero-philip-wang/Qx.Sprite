// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core.DesignPattern
{
    using System;

    /// <summary>
    /// 方法执行条件和排序
    /// </summary>
    /// <typeparam name="T">枚举</typeparam>
    [AttributeUsage(AttributeTargets.Method)]
    public class PipelineFilterAttribute<T> : Attribute
        where T : Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineFilterAttribute{T}"/> class.
        /// </summary>
        /// <param name="key">集合名称</param>
        /// <param name="predication">运行条件</param>
        /// <param name="order">运行排序</param>
        public PipelineFilterAttribute(string key, T predication, int order)
        {
            this.Key = key;
            this.Predication = predication;
            this.Order = order;
        }

        /// <summary>
        /// Gets 集合名称
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets 运行条件
        /// </summary>
        public T Predication { get; }

        /// <summary>
        /// Gets 运行排序
        /// </summary>
        public int Order { get; }
    }
}