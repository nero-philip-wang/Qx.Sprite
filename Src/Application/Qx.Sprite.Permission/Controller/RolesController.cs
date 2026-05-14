// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission.Controller
{
    using System.Linq;
    using System.Threading.Tasks;
    using Asp.Versioning;
    using MapsterMapper;
    using Microsoft.AspNetCore.Mvc;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;

    /// <summary>
    /// Provides API endpoints for managing role entities within the authentication area. Supports standard CRUD
    /// operations for roles using the underlying repository.
    /// </summary>
    /// <remarks>This controller is versioned as 1.0 and is part of the 'auth' area. It leverages the
    /// AutoCurdService base class to expose create, read, update, and delete functionality for roles. All endpoints
    /// require appropriate authorization and are intended for administrative use.</remarks>
    [ApiController]
    [Controller]
    [ApiVersion("1.0")]
    [Area("auth")]
    public class RolesController : AutoCurdService<IEfRepository<int, Role>, Role, RoleListDto, RoleListDto, int, RoleEditDto, RoleEditDto>
    {
        private readonly IEfRepository<long, User> userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesController"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public RolesController(IEfRepository<int, Role> repository, IEfRepository<long, User> userRepository, IMapper mapper)
            : base(repository, mapper)
        {
            this.userRepository = userRepository;
        }

        /// <inheritdoc/>
        public override async Task<RoleListDto> CreateAsync([FromBody] RoleEditDto input)
        {
            var entity = this.mapper.Map<Role>(input);
            entity.Users = this.userRepository.Where(u => input.UserIds.Contains(u.Id)).ToList();
            entity = await Task.Run(() => this.repository.Add(entity, true));
            return this.mapper.Map<RoleListDto>(entity);
        }

        /// <inheritdoc/>
        public override async Task<RoleListDto> UpdateAsync([FromRoute] int id, [FromBody] RoleEditDto input)
        {
            var entity = this.repository.Get(id);
            this.mapper.Map(input, entity);
            entity.Users = this.userRepository.Where(u => input.UserIds.Contains(u.Id)).ToList();
            entity = this.repository.Update(entity, true);
            return this.mapper.Map<RoleListDto>(entity);
        }
    }
}