// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EFCore
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Qx.Sprite.Core;

    /// <summary>
    /// 将TProperty转换为string，保存到数据库
    /// </summary>
    /// <typeparam name="TProperty">对象</typeparam>
    public class JsonValueConverter<TProperty> : ValueConverter<TProperty?, string?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonValueConverter{TProperty}"/> class.
        /// </summary>
        public JsonValueConverter()
            : base(
                v => v == null ? null : v.ToJsonString(),
                v => v == null ? default : v.FromJsonString<TProperty>())
        {
        }
    }
}