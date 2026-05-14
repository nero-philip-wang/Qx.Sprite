// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Caching
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using EasyCaching.Core;
    using EasyCaching.Core.Configurations;
    using EasyCaching.Core.Interceptor;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    /// <summary>
    /// 缓存拦截器动态代理类
    /// </summary>
    public class CacheInterceptorProxy : DispatchProxy
    {
        private object target = null!;
        private IEasyCachingProvider cacheProvider = null!;
        private IEasyCachingKeyGenerator keyGenerator = null!;
        private IOptions<EasyCachingInterceptorOptions> options = null!;

        /// <summary>
        /// 创建代理实例
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="type"></param>
        /// <param name="cacheProvider">内存缓存</param>
        /// <param name="options"></param>
        /// <param name="keyGenerator"></param>
        /// <returns>代理实例</returns>
        public static object Create(object target, Type type, IEasyCachingProvider? cacheProvider, IOptions<EasyCachingInterceptorOptions>? options, IEasyCachingKeyGenerator? keyGenerator)
        {
            var proxy = DispatchProxy.Create(type, typeof(CacheInterceptorProxy));
            proxy.CheckNotNull(nameof(proxy));
            cacheProvider.CheckNotNull(nameof(cacheProvider));
            options.CheckNotNull(nameof(options));
            keyGenerator.CheckNotNull(nameof(keyGenerator));

            var instance = proxy as CacheInterceptorProxy;
            instance?.target = target;
            instance?.cacheProvider = cacheProvider;
            instance?.options = options;
            instance?.keyGenerator = keyGenerator;
            return proxy;
        }

        /// <summary>
        /// 拦截方法调用
        /// </summary>
        /// <param name="targetMethod">目标方法</param>
        /// <param name="args">方法参数</param>
        /// <returns>方法返回值</returns>
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            if (targetMethod?.GetAttribute<EasyCachingAbleAttribute>().FirstOrDefault() is EasyCachingAbleAttribute attribute)
            {
                // Process any cache interceptor
                return this.ProceedAbleAsync(attribute, targetMethod, args);
            }
            else if (targetMethod?.GetAttribute<EasyCachingEvictAttribute>().FirstOrDefault() is EasyCachingEvictAttribute eAttribute)
            {
                // Process any cache interceptor
                return this.ProcessEvictAsync(eAttribute, targetMethod, args);
            }
            else if (targetMethod?.GetAttribute<EasyCachingPutAttribute>().FirstOrDefault() is EasyCachingPutAttribute pAttribute)
            {
                // Process any cache interceptor
                return this.ProcessPutAsync(pAttribute, targetMethod, args);
            }
            else
            {
                return targetMethod?.Invoke(this.target, args);
            }
        }

        /// <summary>
        /// Proceeds the able async.
        /// </summary>
        private object? ProceedAbleAsync(EasyCachingAbleAttribute attribute, MethodInfo targetMethod, object?[]? args)
        {
            var cacheKey = this.keyGenerator.GetCacheKey(targetMethod, args, attribute.CacheKeyPrefix);

            var cacheValue = default(object);
            var isAvailable = true;
            try
            {
                if (attribute.IsHybridProvider)
                {
                    // cacheValue = await HybridCachingProvider.GetAsync(cacheKey, returnType);
                }
                else
                {
                    cacheValue = this.cacheProvider.GetAsync(cacheKey, targetMethod.ReturnType).Result;
                }
            }
            catch (Exception)
            {
                if (!attribute.IsHighAvailability)
                {
                    throw;
                }
                else
                {
                    isAvailable = false;
                }
            }

            if (cacheValue != null)
            {
                return cacheValue;
            }
            else
            {
                // Invoke the method if we don't have a cache hit
                var returnValue = targetMethod.Invoke(this.target, args);

                if (isAvailable)
                {
                    // should we do something when method return null?
                    // 1. cached a null value for a short time
                    // 2. do nothing
                    if (returnValue != null)
                    {
                        if (attribute.IsHybridProvider)
                        {
                            // await HybridCachingProvider.SetAsync(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                        }
                        else
                        {
                            this.cacheProvider.Set(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                        }
                    }
                }

                return returnValue;
            }
        }

        /// <summary>
        /// Processes the put async.
        /// </summary>
        private object? ProcessPutAsync(EasyCachingPutAttribute attribute, MethodInfo targetMethod, object?[]? args)
        {
            var cacheKey = this.keyGenerator.GetCacheKey(targetMethod, args, attribute.CacheKeyPrefix);

            // get the result
            var returnValue = targetMethod.Invoke(this.target, args);
            try
            {
                if (attribute.IsHybridProvider)
                {
                    // await HybridCachingProvider.SetAsync(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                }
                else
                {
                    this.cacheProvider.Set(cacheKey, returnValue, TimeSpan.FromSeconds(attribute.Expiration));
                }
            }
            catch (Exception)
            {
                if (!attribute.IsHighAvailability) throw;
            }

            return returnValue;
        }

        /// <summary>
        /// Processes the evict async.
        /// </summary>
        private object? ProcessEvictAsync(EasyCachingEvictAttribute attribute, MethodInfo targetMethod, object?[]? args)
        {
            var returnValue = default(object);
            if (!attribute.IsBefore)
                returnValue = targetMethod.Invoke(this.target, args);
            try
            {
                if (attribute.IsAll)
                {
                    // If is all , clear all cached items which cachekey start with the prefix.
                    var cachePrefix = this.keyGenerator.GetCacheKeyPrefix(targetMethod, attribute.CacheKeyPrefix);

                    if (attribute.IsHybridProvider)
                    {
                        // await HybridCachingProvider.RemoveByPrefixAsync(cachePrefix);
                    }
                    else
                    {
                        this.cacheProvider.RemoveByPrefix(cachePrefix);
                    }
                }
                else
                {
                    // If not all , just remove the cached item by its cachekey.
                    var cacheKey = this.keyGenerator.GetCacheKey(targetMethod, args, attribute.CacheKeyPrefix);

                    if (attribute.IsHybridProvider)
                    {
                        // await HybridCachingProvider.RemoveAsync(cacheKey);
                    }
                    else
                    {
                        this.cacheProvider.Remove(cacheKey);
                    }
                }
            }
            catch (Exception)
            {
                if (!attribute.IsHighAvailability) throw;
            }

            if (attribute.IsBefore)
                returnValue = targetMethod.Invoke(this.target, args);
            return returnValue;
        }
    }
}