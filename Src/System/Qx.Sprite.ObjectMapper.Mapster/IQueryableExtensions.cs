// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using Mapster;

    /// <summary>
    /// IQueryable 扩展方法
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 自动将对象分页/排序/查询输出为指定格式
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static CountableList<T> Page<T>(this IQueryable query, PagingArgs args)
        {
            var list = new CountableList<T>();

            // 如果是查询参数
            if (args is SearchArgs searchArgs)
            {
                if (!searchArgs.Filter.IsNullOrWhiteSpace())
                    query = query.Where(searchArgs.Filter!, searchArgs.FilterArgs);
            }

            // 如果要 求和
            if (args.NeedTotalReal)
                list.Total = query.Count();

            // 如果是排序参数
            if (args is SortingArgs sortingArgs)
                query = query.OrderBy(sortingArgs.OrderBy);

            // 如果是分页参数
            var entities = query.Skip(args.Skip).Take(args.Take);

            // 投影
            if (query is IQueryable<T> same)
                list.Data = same.ToArray();
            else
                list.Data = entities.ProjectToType<T>().ToArray();
            return list;
        }
    }
}