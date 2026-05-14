// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.ObjectMapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using Qx.Sprite.Core;

    /// <summary>
    /// 列表映射配置
    /// </summary>
    public static class ListMapper
    {
        /// <summary>
        /// 列表映射
        /// </summary>
        /// <typeparam name="TS">源类型</typeparam>
        /// <typeparam name="TD">目标类型</typeparam>
        /// <typeparam name="TComp">键类型</typeparam>
        /// <param name="src">源列表</param>
        /// <param name="dest">目标列表</param>
        /// <param name="context">映射上下文</param>
        /// <returns>映射后的目标列表</returns>
        public static IEnumerable<TD>? List<TS, TD, TComp>(IEnumerable<TS>? src, IEnumerable<TD> dest, ResolutionContext context)
            where TS : class, IHasKey<TComp>
            where TD : class, IHasKey<TComp>
            where TComp : IComparable<TComp>
        {
            // dest 必须收 IList
            List<TD>? list = null;
            if (src == null || src.Any())
            {
                return list;
            }
            else
            {
                list = new List<TD>();
            }

            // 检查目标数组是否 修改和删除
            if (dest != null)
            {
                foreach (var item in dest)
                {
                    var dto = src?.SingleOrDefault(c => c.Id.Equals(item.Id));

                    // 如果输入已经删除
                    if (dto == null)
                    {
                        if (item is ISoftDelete softItem)
                        {
                            softItem.IsDeleted = true;
                            list.Add(item);
                        }
                    }
                    else
                    {
                        context.Mapper.Map(dto, item);
                        list.Add(item);
                    }
                }
            }

            // 添加 新增的对象
            foreach (var dto in src!.Where(c => c.Id.Equals(default(TComp))))
            {
                var entity = context.Mapper.Map<TD>(dto);
                list.Add(entity);
            }

            return list;
        }
    }
}