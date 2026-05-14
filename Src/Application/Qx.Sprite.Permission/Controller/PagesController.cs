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
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;

    /// <summary>
    /// Provides API endpoints for managing page entities, including operations for importing and editing pages within
    /// the authentication area.
    /// </summary>
    /// <remarks>This controller is versioned as 1.0 and is scoped to the 'auth' area. It inherits standard
    /// CRUD functionality from <see cref="AutoCurdService{TRepository, TEntity, TDetailDto, TListDto, TKey, TEditDto,
    /// TCreateDto}"/> and is intended for use with dependency injection in ASP.NET Core applications.</remarks>
    [ApiController]
    [Controller]
    [ApiVersion("1.0")]
    [Area("auth")]
    public class PagesController : AutoCurdService<IEfRepository<int, Page>, Page, PageDetailDto, PageDetailDto, int, PageEditDto, PageEditDto>
    {
        private readonly IStringLocalizer<PagesController> l;

        /// <summary>
        /// Initializes a new instance of the <see cref="PagesController"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="l"></param>
        public PagesController(IEfRepository<int, Page> repository, IMapper mapper, IStringLocalizer<PagesController> l)
            : base(repository, mapper)
        {
            this.l = l;
        }

        /// <summary>
        /// 修改页面操作项
        /// </summary>
        /// <param name="id"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpPost("{id}/operations")]
        public async Task UpdateOperations(int id, IEnumerable<OperationEditDto> options)
        {
            var page = this.repository.Get(id);
            var list = this.mapper.Map<IEnumerable<Operation>>(options);
            page.Operations = list.ToList();
            this.repository.Update(page, true);
        }

        /// <summary>
        /// 修改页面接入点
        /// </summary>
        /// <param name="id"></param>
        /// <param name="endPoints"></param>
        /// <returns></returns>
        [HttpPost("{id}/endpoints")]
        public async Task UpdateEndPoints(int id, IEnumerable<EndPointEditDto> endPoints)
        {
            var page = this.repository.Get(id);
            var list = this.mapper.Map<IEnumerable<EndPoint>>(endPoints);
            page.EndPoints = list.ToList();
            this.repository.Update(page, true);
        }

        /// <summary>
        /// 批量导入页面
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public async Task AddRange([FromBody] IEnumerable<PageEditDto> pages)
        {
            var list = this.mapper.Map<IEnumerable<Page>>(pages);
            this.repository.AddRange(list, true);
        }
    }
}