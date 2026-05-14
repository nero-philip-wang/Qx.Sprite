// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Threading.Tasks;

    /// <summary>
    /// 异步CURD服务
    /// </summary>
    /// <typeparam name="TEntityDto">实体DTO</typeparam>
    /// <typeparam name="TListItemEntityDto">列表项实体DTO</typeparam>
    /// <typeparam name="TKey">主键类型</typeparam>
    /// <typeparam name="TCreateInput">创建输入参数</typeparam>
    /// <typeparam name="TUpdateInput">更新输入参数</typeparam>
    public interface IAsyncCurdService<TEntityDto, TListItemEntityDto, TKey, TCreateInput, TUpdateInput>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityDto> CreateAsync(TCreateInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(TKey id);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntityDto> GetAsync(TKey id);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="cond"></param>
        /// <returns></returns>
        Task<CountableList<TListItemEntityDto>> GetListAsync(SearchArgs cond);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="cond"></param>
        /// <returns></returns>
        Task<CountableList<TListItemEntityDto>> GetListAsync<T>(T cond)
            where T : SortingArgs;

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntityDto> UpdateAsync(TKey id, TUpdateInput input);
    }
}