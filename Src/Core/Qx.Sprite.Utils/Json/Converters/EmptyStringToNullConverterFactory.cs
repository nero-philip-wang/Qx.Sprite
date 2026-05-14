// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <inheritdoc/>
    public class EmptyStringToNullConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            // 处理所有可空类型
            return Nullable.GetUnderlyingType(typeToConvert) != null;
        }

        /// <inheritdoc/>
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var underlyingType = Nullable.GetUnderlyingType(typeToConvert);
            underlyingType.CheckNotNull(nameof(underlyingType));
            var converterType = typeof(EmptyStringToNullConverter<>).MakeGenericType(underlyingType);
            converterType.CheckNotNull(nameof(converterType));
            return (JsonConverter?)Activator.CreateInstance(converterType) ?? throw new InvalidOperationException();
        }
    }
}