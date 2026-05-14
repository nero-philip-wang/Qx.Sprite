// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.TenantManager.Service
{
    using MapsterMapper;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;
    using Qx.Sprite.TenantManager;

    /// <summary>
    /// 商户信息管理
    /// </summary>
    public class TenantService : AutoCurdService<IEfRepository<string, StringKeyTenant>, StringKeyTenant, StringKeyTenantDetailDTO, StringKeyTenantListDTO, string, StringKeyTenantAddDTO, StringKeyTenantUpdateDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TenantService"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public TenantService(IEfRepository<string, StringKeyTenant> repository, IMapper mapper)
            : base(repository, mapper)
        {
        }
    }
}
