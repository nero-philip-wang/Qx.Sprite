// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// IServiceProvider 扩展类
    /// </summary>
    public static class IServiceProviderLogExt
    {
        /// <summary>
        /// 获取日志记录器
        /// </summary>
        /// <typeparam name="T"> 调用类 </typeparam>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ILogger<T> GetLogger<T>(this IServiceProvider provider)
        {
            ILoggerFactory? factory = provider.GetService<ILoggerFactory>();
            factory.CheckNotNull(nameof(ILoggerFactory));
            return factory!.CreateLogger<T>();
        }
    }
}