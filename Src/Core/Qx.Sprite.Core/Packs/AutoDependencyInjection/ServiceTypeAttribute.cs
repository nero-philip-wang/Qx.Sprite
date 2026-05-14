// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 指定注册为特定服务；type为null时忽略接口，将整个类作为服务注入
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ServiceTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTypeAttribute"/> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        public ServiceTypeAttribute(Type? type = null, object? key = null)
        {
            this.Type = type;
            this.Key = key;
        }

        /// <summary>
        /// Gets or sets 服务类型
        /// </summary>
        public Type? Type { get; set; }

        /// <summary>
        /// Gets or sets 服务名称
        /// </summary>
        public object? Key { get; set; }
    }
}