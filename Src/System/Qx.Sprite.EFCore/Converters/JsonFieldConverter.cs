// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EFCore
{
    using System;
    using System.Linq.Expressions;
    using System.Text.Json;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Qx.Sprite.Core;

    /// <summary>
    /// 数据库Json字段转换器
    /// </summary>
    public class JsonFieldConverter : ValueConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonFieldConverter"/> class.
        /// </summary>
        /// <param name="propertyType"></param>
        public JsonFieldConverter(Type propertyType)
            : base(
                  (object? v) => v == null ? null : JsonSerializer.Serialize(v, propertyType, JsonUtils.JsonOptions),
                  (string v) => v == null ? null : JsonSerializer.Deserialize(v, propertyType, JsonUtils.JsonOptions))
        {
            this.ModelClrType = propertyType;
            this.ProviderClrType = typeof(string);
            this.ConvertToProvider = (object? v) => v == null ? null : JsonSerializer.Serialize(v, propertyType, JsonUtils.JsonOptions);
            this.ConvertFromProvider = (object? v) => !(v is string json) ? null : JsonSerializer.Deserialize(json, propertyType, JsonUtils.JsonOptions);
        }

        /// <inheritdoc/>
        public override Func<object?, object?> ConvertToProvider { get; }

        /// <inheritdoc/>
        public override Func<object?, object?> ConvertFromProvider { get; }

        /// <inheritdoc/>
        public override Type ModelClrType { get; }

        /// <inheritdoc/>
        public override Type ProviderClrType { get; }

        /// <inheritdoc/>
        public override Expression ConstructorExpression => throw new NotImplementedException();
    }
}