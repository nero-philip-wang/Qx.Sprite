namespace Demo.Controllers
{
    using Demo.Services;
    using EasyCaching.Core;
    using EasyCaching.Core.Interceptor;
    using Microsoft.AspNetCore.Mvc;
    using Qx.Sprite.Core;
    using Qx.Sprite.Mvc;

    public class CacheController : ApiBaseController
    {
        private readonly IEasyCachingProvider _cachingProvider;
        private readonly IAservice service;

        public CacheController(IEasyCachingProvider cachingProvider, IAservice service)
        {
            _cachingProvider = cachingProvider;
            this.service = service;
        }

        [HttpGet("set/{key}/{value}")]
        public async Task<IActionResult> SetCache(string key, string value)
        {
            await _cachingProvider.SetAsync(key, value, TimeSpan.FromMinutes(10));
            return Ok($"缓存设置成功: {key} = {value}");
        }

        [HttpGet("get/{key}")]
        public async Task<IActionResult> GetCache(string key)
        {
            var result = await _cachingProvider.GetAsync<string>(key);
            if (result.HasValue)
            {
                return Ok($"缓存值: {key} = {result.Value}");
            }
            return NotFound($"缓存键 {key} 不存在");
        }

        [HttpGet("when")]
        public string SayHello()
        {
            return service.SayHello("me");
        }
    }
}