// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Asp.Versioning;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;

    /// <summary>
    /// Provides API endpoints for managing user roles and retrieving role-based page permissions within the
    /// authentication area.
    /// </summary>
    /// <remarks>This controller is versioned as 1.0 and requires JWT authorization for its endpoints. It
    /// extends the generic AutoCurdService to support standard CRUD operations for users, and adds functionality to
    /// query the pages and permissions associated with a user's roles. Use this controller to access and manage user
    /// role assignments and their related permissions in the system.</remarks>
    [ApiController]
    [Controller]
    [ApiVersion("1.0")]
    [Area("auth")]
    public class UsersController : AutoCurdService<IEfRepository<long, User>, User, UserDetailDto, UserDetailDto, long, UserEditDto, UserEditDto>
    {
        private readonly IEfRepository<int, Operation> operationRepo;
        private readonly IUserInfo user;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="operationRepo"></param>
        /// <param name="user"></param>
        public UsersController(IEfRepository<long, User> repository, IMapper mapper, IEfRepository<int, Operation> operationRepo, IUserInfo user)
            : base(repository, mapper)
        {
            this.operationRepo = operationRepo;
            this.user = user;
        }

        /// <summary>
        /// 获取我的路由权限
        /// </summary>
        /// <returns></returns>
        [Authorize("jwt")]
        [HttpGet("/getRoute")]
        public async Task<IEnumerable<PageDetailDto>> GetRoute()
        {
            var userid = this.user.Id;
            userid.CheckNotNull("userid");

            var user = this.repository.AsNoTracking()
                .Where(c => c.Id == (long)userid)
                .Include(c => c.Roles)
                .ThenInclude(c => c.Permissions)
                .ThenInclude(c => c.Page)
                .AsSplitQuery()
                .FirstOrDefault();
            user.CheckNotNull("user");
            var roles = user.Roles.ToList();

            var dic = new Dictionary<int, Permission>();
            foreach (var role in roles)
            {
                foreach (var permission in role.Permissions)
                {
                    if (!dic.ContainsKey(permission.PageId))
                    {
                        dic.Add(permission.PageId, permission);
                    }
                    else
                    {
                        var page = dic[permission.PageId];
                        page.ActionPermission = page.ActionPermission.Union(permission.ActionPermission).Distinct().ToArray();
                        page.EndPoints = page.EndPoints.Union(permission.EndPoints).Distinct().ToArray();
                    }
                }
            }

            var operations = this.operationRepo.ToList();
            foreach (var item in dic)
            {
                item.Value.Page.Meta.Permission = operations
                    .Where(c => item.Value.ActionPermission.Contains(c.Id))
                    .Select(c => c.Code)
                    .ToArray();
            }

            return this.mapper.Map<IEnumerable<PageDetailDto>>(dic.Values.Select(c => c.Page));
        }
    }
}