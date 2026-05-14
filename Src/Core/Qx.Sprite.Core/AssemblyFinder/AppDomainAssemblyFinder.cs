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
    using Microsoft.Extensions.DependencyModel;

    /// <summary>
    /// 加载所有非框架程序集
    /// </summary>
    public class AppDomainAssemblyFinder
    {
        private const string FrameNamespace = "Qx.Sprite.";
        private static Assembly[] allAssemblies = [];
        private static List<string> filenamePrefix = new List<string>();

        /// <summary>
        /// 初始化要加载的程序集前缀
        /// </summary>
        /// <param name="filenamePrefixList"></param>
        public static void InitFilePrefix(IEnumerable<string> filenamePrefixList)
        {
            if (!filenamePrefix.Contains(FrameNamespace))
                filenamePrefix.Add(FrameNamespace);
            filenamePrefix.AddRange(filenamePrefixList.Where(c => !filenamePrefix.Contains(c)));
        }

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        public Assembly[] GetAllAssemblies()
            => allAssemblies.Any() ? allAssemblies : allAssemblies = this.FindAllAssemblies();

        /// <summary>
        /// 从程序集中按照条件获取类型
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Type[] FindTypes(IEnumerable<Assembly> assemblies, Func<Type, bool> predicate)
            => assemblies.SelectMany(a => a.GetTypes().Where(predicate)).ToArray();

        /// <summary>
        /// 获取所有程序集中的制定类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeAbstract"></param>
        /// <returns></returns>
        public Type[] FindTypesInheritFrom(Type type, bool includeAbstract = false)
            => this.FindTypes(this.GetAllAssemblies(), t => type.IsAssignableFrom(t) && (includeAbstract || !t.IsAbstract));

        /// <summary>
        /// 获取所有程序集中的制定类型
        /// </summary>
        /// <typeparam name="T">任意类型</typeparam>
        /// <param name="includeAbstract"></param>
        /// <returns></returns>
        public Type[] FindTypesInheritFrom<T>(bool includeAbstract = false)
            => this.FindTypesInheritFrom(typeof(T), includeAbstract);

        private Assembly[] FindAllAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            DependencyContext? context = DependencyContext.Default;
            if (context != null)
            {
                var list = context.GetDefaultAssemblyNames()
                    .Where(m => m.FullName != null && filenamePrefix.Any(k => m.FullName.Contains(k)))
                    .Select(Assembly.Load);
                assemblies.AddRange(list);
            }

            if (!assemblies.Any())
            {
                assemblies.AddRange(
                    AppDomain.CurrentDomain.GetAssemblies()
                    .Where(m => m.FullName != null && filenamePrefix.Any(k => m.FullName.Contains(k))));
            }

            return assemblies.ToArray();
        }
    }
}