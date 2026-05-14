namespace Demo.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Qx.Sprite.Auth;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;
    using Qx.Sprite.Swagger;
    using System.ComponentModel;

    public class AuthController : ApiBaseController
    {
        [HttpGet("jwtuser")]
        [Authorize("jwt")]
        [AllowAnonymous]
        public string Get([FromServices] IUserInfo user)
        {
            return "Hello, World! " + user.Title;
        }

        [HttpGet("token")]
        public string Token([FromServices] IUserInfo user)
        {
            return new JwtUser<long, long>()
            {
                Id = 1,
                Title = "test",
                ExtraData = new Dictionary<string, object?> {
                  { "Name", "test" },}
            }.CreateToken(DateTime.Now.AddDays(30), "all");
        }

        [Authorize("appid")]
        [HttpGet("appid")]
        public string GetAppId([FromServices] IClientInfo info)
        {
            return "Hello, World! " + info.Title;
        }
    }
}