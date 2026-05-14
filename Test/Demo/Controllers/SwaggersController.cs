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

    public class SwaggersController : ApiBaseController
    {
        [HttpGet("Example")]
        public Info GetExample([FromQuery] Info a, [FromServices] IUserInfo user)
        {
            return a;
        }

    }

    public class Info
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int Age { get; set; }
        public long Id { get; set; }
        public TypeNameHandling TypeNameHandling { get; set; }

        public static Info Example(Random random, Info example) => example ?? (example =
                                 new Info
                                 {
                                     Id = random.NextInt64(1, 10000),
                                     Name = random.NextSurName(),
                                     PhoneNumber = random.NextPhoneNumber(),
                                     Age = 18
                                 });
    }
}