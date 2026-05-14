// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.ObjectMapper
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using AutoMapper;

    /// <summary>
    /// DTO映射配置
    /// </summary>
    public static class DtosMapper
    {
        /// <summary>
        /// 添加映射配置
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <typeparam name="TEntityDto">实体DTO类型</typeparam>
        /// <typeparam name="TListItemEntityDto">列表项实体DTO类型</typeparam>
        /// <typeparam name="TCreateInput">创建输入类型</typeparam>
        /// <typeparam name="TUpdateInput">更新输入类型</typeparam>
        /// <param name="configAction">映射配置表达式</param>
        public static void AddAutoMapper<TEntity, TEntityDto, TListItemEntityDto, TCreateInput, TUpdateInput>(IMapperConfigurationExpression configAction)
        {
            configAction.CreateMap<TEntity, TEntityDto>();
            configAction.CreateMap<TEntity, TListItemEntityDto>();
            configAction.CreateMap<TCreateInput, TEntity>();
            configAction.CreateMap<TUpdateInput, TEntity>();
        }
    }
}