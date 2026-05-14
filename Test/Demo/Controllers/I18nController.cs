namespace Demo.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using Qx.Sprite.Mvc;

    public class I18nController : ApiBaseController
    {
        private readonly IStringLocalizer<I18nController> l;

        public I18nController(IStringLocalizer<I18nController> l)
        {
            this.l = l;
        }

        /// <summary>
        /// 通过 header/Accept-Language 或者 query/culture 来切换语言
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string Get()
        {
            return l["a"];
        }
    }
}
