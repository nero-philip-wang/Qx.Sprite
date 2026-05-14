// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Swagger
{
    using System;
    using System.Linq;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// EnumSchemaFilter
    /// </summary>
    public class EnumSchemaFilter : ISchemaFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type == null)
                return;

            var enumType = Nullable.GetUnderlyingType(type) ?? type;
            if (!enumType.IsEnum)
                return;

            // 将枚举描述为字符串，并列出枚举成员的字符串名
            schema.Type = "string";
            schema.Enum = enumType
                .GetEnumValues()
                .Cast<object>()
                .Select(n => (IOpenApiAny)new OpenApiInteger(n.GetHashCode()))
                .ToList();

            // 添加 x-enumNames 扩展，供前端/生成器使用
            var nameArray = new OpenApiArray();
            nameArray.AddRange(enumType.GetEnumNames().Select(n => new OpenApiString(n)));
            schema.Extensions["x-enumNames"] = nameArray;
        }
    }
}