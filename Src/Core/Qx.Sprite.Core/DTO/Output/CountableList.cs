// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 列表结果
    /// </summary>
    public abstract class CountableList
    {
        /// <summary>
        /// Gets or sets 总条数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable GetData();
    }

    /// <summary>
    /// 带泛型的列表结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class CountableList<T> : CountableList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountableList{T}"/> class.
        /// </summary>
        public CountableList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableList{T}"/> class.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="total"></param>
        public CountableList(IEnumerable<T> data, int total)
        {
            this.Data = data;
            this.Total = total;
        }

        /// <summary>
        /// Gets or sets 数据
        /// </summary>
        public IEnumerable<T> Data { get; set; } = [];

        /// <inheritdoc/>
        public override IEnumerable GetData() => this.Data;
    }
}