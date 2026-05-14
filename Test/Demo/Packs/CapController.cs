
namespace Demo.Packs
{
    using DotNetCore.CAP;
    using Microsoft.AspNetCore.Mvc;
    using Qx.Sprite.Mvc;

    public class CapController :ApiBaseController
    {
        private readonly ILogger<CapController> logger;

        public CapController(ILogger<CapController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("post-message")]
        public string PostMessage([FromServices] ICapPublisher capPublisher)
        {
            capPublisher.Publish("demo.message", "Hello CAP!");
            return "Hello CAP!";
        }

        [NonAction]
        [CapSubscribe("demo.message")]
        public string SubscribeMessage(string message, [FromCap] CapHeader header)
        {
            logger.LogInformation("Received message: {Message}", message);
            return $"Received message: {message}";
        }
    }
}
