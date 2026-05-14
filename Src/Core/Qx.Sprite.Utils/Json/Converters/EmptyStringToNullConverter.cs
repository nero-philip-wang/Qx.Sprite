// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// 空字符串转换为 null 转换器。
    /// </summary>
    /// <typeparam name="T">目标类型。</typeparam>
    public class EmptyStringToNullConverter<T> : JsonConverter<T?>
        where T : struct
    {
        /// <inheritdoc/>
        public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? value = reader.GetString();
                if (string.IsNullOrWhiteSpace(value))
                {
                    return null;
                }

                // 尝试将非空字符串转换为目标类型
                try
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return null; // 或抛出异常，取决于你希望的行为
                }
            }

            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                JsonSerializer.Serialize(writer, value.Value, options);
            }
        }
    }
}