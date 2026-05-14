// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Json 转换器，用于处理包含 ExtraData 的对象，将 ExtraData 中的属性序列化到实体主表中
    /// </summary>
    public class ExtraDataJsonConverter : JsonConverter<object>
    {
        private static readonly Dictionary<string, Dictionary<string, PropertyInfo>> PropertiesCache = [];

        /// <inheritdoc/>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IHasExtraData).IsAssignableFrom(typeToConvert);
        }

        /// <inheritdoc/>
        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!typeof(IHasExtraData).IsAssignableFrom(typeToConvert))
            {
                throw new ArgumentException($"Type {typeToConvert.Name} must implement IHasExtraData", nameof(typeToConvert));
            }

            var instance = Activator.CreateInstance(typeToConvert) ?? throw new NullReferenceException($"Type {typeToConvert.Name} can not create");
            var party = (IHasExtraData)instance;
            var properties = GetTypeProperties(typeToConvert);

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return party;
                }

                if (reader.TokenType != JsonTokenType.PropertyName)
                {
                    throw new JsonException();
                }

                var propertyName = reader.GetString();
                if (propertyName == null)
                {
                    continue;
                }

                reader.Read();
                if (properties.TryGetValue(propertyName, out PropertyInfo? prop))
                {
                    var value = JsonSerializer.Deserialize(ref reader, prop.PropertyType, options);
                    prop.SetValue(party, value);
                }
                else
                {
                    party.ExtraData[propertyName] = JsonSerializer.Deserialize<object>(ref reader, options);
                }
            }

            return party;
        }

        /// <inheritdoc/>
        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            var type = value.GetType();
            var extraDataValue = (IHasExtraData)value;
            var properties = GetTypeProperties(type);

            writer.WriteStartObject();

            foreach (var prop in properties.Values)
            {
                var propValue = prop.GetValue(value);

                var propertyName = options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name;
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
            }

            if (extraDataValue.ExtraData != null)
            {
                foreach (var item in extraDataValue.ExtraData)
                {
                    writer.WritePropertyName(item.Key);
                    JsonSerializer.Serialize(writer, item.Value, options);
                }
            }

            writer.WriteEndObject();
        }

        private static Dictionary<string, PropertyInfo> GetTypeProperties(Type type)
        {
            // 使用类型的完全限定名作为缓存键，用于存储和检索该类型的属性信息
            var cacheKey = type.FullName;
            if (cacheKey == null)
            {
                return [];
            }
            else if (!PropertiesCache.ContainsKey(cacheKey))
            {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.Name != nameof(IHasExtraData.ExtraData))
                    .ToDictionary(p => p.Name);
                PropertiesCache[cacheKey] = properties;
            }

            return PropertiesCache[cacheKey];
        }
    }
}