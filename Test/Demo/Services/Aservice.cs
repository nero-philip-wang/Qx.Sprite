namespace Demo.Services
{
    using EasyCaching.Core.Interceptor;
    using Qx.Sprite.Caching;
    using Qx.Sprite.Core;

    [EasyCachingMark]
    public interface IAservice
    {
        [EasyCachingAble(Expiration = 30)]
        public string SayHello(string name);
    }

    public class Aservice : IAservice, IScoped
    {
        public string SayHello(string name)
        {
            return $"Hello {name}, now is {DateTime.Now}";
        }
    }
}