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

    /// <summary>
    /// 默认映射配置
    /// </summary>
    public class DefaultMapper : IMapperConfiguration
    {
        /// <inheritdoc/>
        public void AddAutoMapper(IMapperConfigurationExpression configAction)
        {
            configAction.CreateMap<long, string>()
                .ConvertUsing(source => Convert.ToHexString(BitConverter.GetBytes(source).Reverse().ToArray()).ToLower().TrimStart('0'));
            configAction.CreateMap<string, long>()
                .ConvertUsing(source => BitConverter.ToInt64(Convert.FromHexString(source.PadLeft(16, '0')).Reverse().ToArray()));

            configAction.CreateMap<int, string>()
                .ConvertUsing(source => Convert.ToHexString(BitConverter.GetBytes(source).Reverse().ToArray()).ToLower().TrimStart('0'));
            configAction.CreateMap<string, int>()
                .ConvertUsing(source => BitConverter.ToInt32(Convert.FromHexString(source.PadLeft(8, '0')).Reverse().ToArray()));

            // 后端内部运算为分，到前端转为元
            configAction.CreateMap<int, decimal>().ConvertUsing(source => Math.Round(source / 100m, 2));
            configAction.CreateMap<decimal, int>().ConvertUsing(source => (int)(source * 100));
        }
    }
}