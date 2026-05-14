// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Json;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text.Json;

    /// <summary>
    /// 对象扩展方法
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="subject"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T? As<T>(this object subject, T? defaultValue = default)
        {
            try
            {
                var conversionType = typeof(T);
                if (subject == null)
                    return default;
                if (conversionType.IsEnum)
                    return (T)Enum.Parse(conversionType, subject.ToString() ?? string.Empty);
                if (conversionType.IsClass || conversionType.IsInterface)
                    return (T)subject;
                if (conversionType == typeof(Guid))
                    return (T)(object)Guid.Parse(subject.ToString() ?? string.Empty);
                else
                    return (T)Convert.ChangeType(subject, conversionType);
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// 检查是否为Null
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="paramName"></param>
        public static void CheckNotNull([NotNull] this object? subject, string paramName)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 检查是否为空或者Null
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] this object? subject)
        {
            if (subject == null)
            {
                return true;
            }
            else if (subject is string s && string.IsNullOrEmpty(s))
            {
                return true;
            }
            else if (subject is Array a && a.Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 非空检查
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static bool IsNotNull([NotNullWhen(true)] this object? subject)
        {
            return subject != null;
        }

        /// <summary>
        /// Json格式化输出
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static string ToJsonString(this object subject)
        {
            return JsonSerializer.Serialize(subject, JsonUtils.JsonOptions);
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        [Obsolete]
        public static T DeepClone<T>(this T instance)
            where T : class
        {
            var formatter = new BinaryFormatter();
            var buffer = new MemoryStream();
            formatter.Serialize(buffer, instance);
            return (T)formatter.Deserialize(buffer);
        }
    }
}