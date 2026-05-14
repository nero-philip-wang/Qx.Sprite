// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Text.Json;

    /// <summary>
    /// 可空长整型字符串转换
    /// </summary>
    public class NullableLongStringConverter : System.Text.Json.Serialization.JsonConverter<long?>
    {
        /// <inheritdoc/>
        public override long? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var val = reader.GetString();
            long result;
            if (long.TryParse(val, out result))
                return result;
            else
                return null;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, long? value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToString());
        }
    }
}