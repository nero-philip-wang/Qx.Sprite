// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Text;
    using System.Threading.Tasks;
    using MapsterMapper;
    using Microsoft.AspNetCore.Mvc;
    using Qx.Sprite.Core;

    /// <summary>
    /// Provides a base implementation for asynchronous CRUD (Create, Read, Update, Delete) operations on entities using a
    /// repository and object mapping. Intended for use in API controllers to simplify standard data access patterns.
    /// </summary>
    /// <remarks>This abstract service is designed to be inherited by API controllers to provide standardized CRUD
    /// endpoints. It leverages dependency injection for repository and mapping functionality, and supports API versioning
    /// and area routing. Thread safety and transaction management depend on the underlying repository
    /// implementation.</remarks>
    /// <typeparam name="TRepository">The type of repository used to access and manage entities. Must implement <see cref="IRepository{TKey, TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity">The type of entity managed by the service. Must implement <see cref="IHasKey{TKey}"/>.</typeparam>
    /// <typeparam name="TEntityDto">The type of data transfer object (DTO) used to represent the entity in API responses.</typeparam>
    /// <typeparam name="TListItemEntityDto">The type of DTO used for listing entities, typically a lightweight representation for collection results.</typeparam>
    /// <typeparam name="TKey">The type of the key used to uniquely identify entities.</typeparam>
    /// <typeparam name="TCreateInput">The type of input model used when creating a new entity.</typeparam>
    /// <typeparam name="TUpdateInput">The type of input model used when updating an existing entity.</typeparam>
    [Area("default")]
    [Route("api/v{version:apiVersion}/[area]/[controller]/")]
    public abstract class AutoCurdService<TRepository, TEntity, TEntityDto, TListItemEntityDto, TKey, TCreateInput, TUpdateInput> :
        IAsyncCurdService<TEntityDto, TListItemEntityDto, TKey, TCreateInput, TUpdateInput>
        where TEntity : class, IHasKey<TKey>
        where TRepository : IRepository<TKey, TEntity>
    {
        /// <summary>
        /// 自动CRUD服务
        /// </summary>
        protected IRepository<TKey, TEntity> repository;

        /// <summary>
        /// 自动CRUD服务的映射器
        /// </summary>
        protected IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoCurdService{TRepository, TEntity, TEntityDto, TListItemEntityDto, TKey, TCreateInput, TUpdateInput}"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public AutoCurdService(TRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        [Route("{id}")]
        [HttpGet]
        public virtual async Task<TEntityDto> GetAsync([FromRoute] TKey id)
        {
            var entity = await Task.Run(() => this.repository.Get(id));
            return this.mapper.Map<TEntityDto>(entity);
        }

        /// <inheritdoc/>
        [HttpGet]
        public virtual async Task<CountableList<TListItemEntityDto>> GetListAsync([FromQuery] SearchArgs cond)
        {
            return await Task.Run(() => this.repository.GetList<TListItemEntityDto>(cond));
        }

        /// <inheritdoc/>
        [HttpPost]
        public virtual async Task<TEntityDto> CreateAsync([FromBody] TCreateInput input)
        {
            input.CheckNotNull(nameof(input));
            var entity = this.mapper.Map<TEntity>(input);
            entity = await Task.Run(() => this.repository.Add(entity, true));
            return this.mapper.Map<TEntityDto>(entity);
        }

        /// <inheritdoc/>
        [HttpPut("{id}")]
        public virtual async Task<TEntityDto> UpdateAsync([FromRoute] TKey id, [FromBody] TUpdateInput input)
        {
            var entity = this.repository.Get(id);
            this.mapper.Map(input, entity);
            entity = await this.repository.UpdateAsync(entity, true);
            return this.mapper.Map<TEntityDto>(entity);
        }

        /// <inheritdoc/>
        [Route("{id}")]
        [HttpDelete]
        public virtual async Task DeleteAsync([FromRoute] TKey id)
        {
            var entity = this.repository.Get(id);
            await this.repository.RemoveAsync(entity, true);
        }

        /// <inheritdoc/>
        public async Task<CountableList<TListItemEntityDto>> GetListAsync<T>(T cond)
            where T : SortingArgs
        {
            var query = this.repository.GetQuerySet();
            query = this.ApplyQueryFilter(cond, query);
            query = query.OrderBy(cond.OrderBy);
            return await Task.FromResult(query.Page<TListItemEntityDto>(cond));
        }

        /// <summary>
        /// 使过滤条件生效
        /// </summary>
        /// <typeparam name="T">条件</typeparam>
        /// <param name="cond"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        protected virtual IQueryable<TEntity> ApplyQueryFilter<T>(T cond, IQueryable<TEntity> query)
            where T : SortingArgs
        {
            return query.AsQueryable();
        }
    }
}