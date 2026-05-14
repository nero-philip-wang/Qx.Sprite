// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Qx.Sprite.Mvc;

    /// <summary>
    /// 枚举
    /// </summary>
    public class EnumsController : ApiBaseController
    {
        private readonly IEnumToDictionaryService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumsController"/> class.
        /// </summary>
        /// <param name="service"></param>
        public EnumsController(IEnumToDictionaryService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 获取枚举下拉列表和枚举类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("ddl")]
        public async Task<object> GetDDL()
        {
            var enums = this.service.GetJsEnum();
            var options = this.service.GetSelectOptions();
            return new { options, enums };
        }
    }
}
