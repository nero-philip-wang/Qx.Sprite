// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using global::Mapster;

    /// <summary>
    /// 映射配置接口
    /// </summary>
    public interface IMapperConfiguration
    {
        /// <summary>
        /// 添加Mapster映射配置
        /// </summary>
        /// <param name="cfg"></param>
        void AddMapper(TypeAdapterConfig cfg);
    }
}
