// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Extension methods for <see cref="IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Gets a configuration section as a strongly typed object.
        /// </summary>
        /// <typeparam name="T">待反序列化类型</typeparam>
        /// <param name="configuration"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T? Get<T>(this IConfiguration configuration, string key)
            => configuration.GetSection(key).Get<T>();
    }
}