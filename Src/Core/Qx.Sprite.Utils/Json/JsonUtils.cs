// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Text.Json.Serialization.Metadata;

    /// <summary>
    /// json 格式化配置工具
    /// </summary>
    public static partial class JsonUtils
    {
        /// <summary>
        /// Gets or sets a value indicating whether 枚举输出为字符串
        /// </summary>
        public static bool IsEnumAsString { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether 使用ExtraDataJsonConverter
        /// </summary>
        public static bool IsExtraDataConverter { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether long 输出为字符串
        /// </summary>
        public static bool IsLongAsString { get; set; } = true;

        /// <summary>
        /// Gets 获取json系列化配置
        /// </summary>
        public static JsonSerializerOptions JsonOptions
        {
            get
            {
                field = field ?? new JsonSerializerOptions()
                {
                    // 不转义html
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

                    // 小驼峰
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                    // 忽略循环引用
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,

                    // 保留默认值
                    DefaultIgnoreCondition = JsonIgnoreCondition.Never,

                    // long 读取
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,

                    // 序列化字段
                    IncludeFields = false,

                    TypeInfoResolver = new DefaultJsonTypeInfoResolver(),
                };
                if (IsLongAsString)
                {
                    field.Converters.Add(new LongStringConverter());
                    field.Converters.Add(new NullableLongStringConverter());
                }

                if (IsEnumAsString)
                {
                    field.Converters.Add(new JsonStringEnumConverter());
                }

                if (IsExtraDataConverter)
                {
                    field.Converters.Add(new ExtraDataJsonConverter());
                }

                return field;
            }
        }
    }
}