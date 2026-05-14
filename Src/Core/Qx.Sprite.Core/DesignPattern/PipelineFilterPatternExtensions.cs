// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core.DesignPattern
{
    using System;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// 管道过滤器模式
    /// </summary>
    public static class PipelineFilterPatternExtensions
    {
        /// <summary>
        /// 管道过滤器模式, 查找并执行
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="subject"></param>
        /// <param name="key"></param>
        /// <param name="predication"></param>
        /// <param name="arg"></param>
        public static void PipelineFilterRun<T>(this object subject, string key, T predication, object arg)
            where T : Enum
        {
            var list = subject.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(c => c.HasAttribute<PipelineFilterAttribute<T>>())
                        .Where(c =>
                        {
                            var attr = c.GetAttribute<PipelineFilterAttribute<T>>()?.FirstOrDefault();
                            if (attr == null)
                                return false;
                            else
                                return attr?.Key == key && attr.Predication.HasFlag(predication);
                        })
                        .OrderBy(c =>
                        {
                            var attr = c.GetAttribute<PipelineFilterAttribute<T>>()?.FirstOrDefault();
                            return attr?.Order;
                        })
                        .ToArray();
            foreach (var item in list)
            {
                item.Invoke(subject, [arg]);
            }
        }
    }
}