// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 检查某个 MemberInfo 有没有被标记 某个特性.
    /// </summary>
    public static class AttributeFinder
    {
        /// <summary>
        /// 检查某个 MemberInfo 有没有被标记 某个特性.
        /// </summary>
        /// <typeparam name="T">Attribute</typeparam>
        /// <param name="member"></param>
        /// <returns></returns>
        public static bool HasAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), true).Any();
        }

        /// <summary>
        /// 获取某个 MemberInfo 上的特性.
        /// </summary>
        /// <typeparam name="T">Attribute</typeparam>
        /// <param name="member"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public static T[] GetAttribute<T>(this MemberInfo member, bool inherit = true)
            where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit) as T[] ?? [];
        }
    }
}