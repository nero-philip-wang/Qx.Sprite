// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.ObjectMapper
{
    using AutoMapper;

    /// <summary>
    /// 映射配置接口
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        /// 添加映射配置
        /// </summary>
        /// <param name="configAction"></param>
        void AddAutoMapper(IMapperConfigurationExpression configAction);
    }
}