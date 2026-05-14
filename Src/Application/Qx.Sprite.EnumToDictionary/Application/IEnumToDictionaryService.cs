// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    using System.Collections.Generic;

    /// <summary>
    /// 字典服务
    /// </summary>
    public interface IEnumToDictionaryService
    {
        /// <summary>
        /// 查找所有枚举
        /// </summary>
        /// <returns></returns>
        List<Option> FindAllEnum();

        /// <summary>
        /// 获取 js 枚举
        /// </summary>
        /// <returns></returns>
        object GetJsEnum();

        /// <summary>
        /// 获取下拉列表选项
        /// </summary>
        /// <returns></returns>
        object GetSelectOptions();

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        void UpdateList(IEnumerable<IDictionary> src, IList<IDictionary> dest);
    }
}