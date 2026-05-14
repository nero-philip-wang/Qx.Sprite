// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.ApiFx.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mapster;
    using Qx.Sprite.Auth;
    using Qx.Sprite.Core;
    using Qx.Sprite.ObjectMapper;

    /// <summary>
    /// Map配置
    /// </summary>
    public class MapperConfiguration : IMapperConfiguration
    {
        /// <inheritdoc/>
        public void AddMapper(TypeAdapterConfig cfg)
        {
            cfg.NewConfig<ClientAppInfo, IClientInfo>();
        }
    }
}
