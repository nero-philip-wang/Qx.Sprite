// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Swagger
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.Internal;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json.Linq;
    using Qx.Sprite.Core;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// 默认值 Schema 过滤器，用于在 Swagger 文档中为模型属性添加示例值。
    /// 读取模型类中的静态 Example 方法来生成示例值。
    /// </summary>
    public class DefaultValueSchemaFilter : ISchemaFilter
    {
        private static readonly Random Rand = new Random();
        private static readonly Dictionary<Type, object> Examples = [];

        /// <inheritdoc/>
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var dtoType = context.MemberInfo?.DeclaringType;
            var getExampleMethod = dtoType?.GetMethod("Example", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            if (getExampleMethod == null) return;

            var instance = getExampleMethod.Invoke(null, [Rand, Examples.ContainsKey(dtoType!) ? Examples[dtoType!] : default]);
            if (instance != null)
            {
                Examples[dtoType!] = instance;
                schema.Example = this.SetValue(context.Type, context.MemberInfo.GetMemberValue(instance));
            }
        }

        private IOpenApiAny SetValue(Type type, object value)
        {
            switch (type)
            {
                case Type t when t == typeof(string):
                    return new OpenApiString(value?.ToString() ?? string.Empty);

                case Type t when t == typeof(int) || t == typeof(int?):
                    return new OpenApiInteger(value is null ? 0 : Convert.ToInt32(value));

                default:
                    return OpenApiAnyFactory.CreateFromJson(value?.ToString() ?? string.Empty);
            }
        }
    }
}