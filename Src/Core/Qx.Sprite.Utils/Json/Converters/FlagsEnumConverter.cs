// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// HasFlag 的 Enum 转为 字符串数组
    /// </summary>
    public class FlagsEnumConverter : JsonConverterFactory
    {
        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsEnum && typeToConvert.IsDefined(typeof(FlagsAttribute), false);

        /// <inheritdoc/>
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
            => (JsonConverter)Activator.CreateInstance(typeof(FlagsEnumConverterInner<>).MakeGenericType(typeToConvert))!;

        private class FlagsEnumConverterInner<T> : JsonConverter<T>
            where T : struct, Enum
        {
            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    T result = default(T);
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                            break;
                        if (reader.TokenType == JsonTokenType.String)
                        {
                            var value = Enum.Parse<T>(reader.GetString()!);
                            result = (T)(dynamic)result | (dynamic)value;
                        }
                    }

                    return result;
                }

                if (reader.TokenType == JsonTokenType.String)
                {
                    var str = reader.GetString()!.Trim('[', ']');
                    return str.Split(',')
                        .Select(s => Enum.Parse<T>(s.Trim(" \"")))
                        .Aggregate((a, b) => (dynamic)a | (dynamic)b);
                }

                return (T)Enum.ToObject(typeof(T), reader.GetInt32());
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                var values = Enum.GetValues<T>().Where(v => value.HasFlag(v) && !EqualityComparer<T>.Default.Equals(v, default(T)));
                writer.WriteStartArray();
                foreach (var v in values)
                {
                    writer.WriteStringValue(v.ToString());
                }

                writer.WriteEndArray();
            }
        }
    }
}