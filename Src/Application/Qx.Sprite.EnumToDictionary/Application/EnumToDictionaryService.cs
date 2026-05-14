// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionary
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Qx.Sprite.Core;

    /// <summary>
    /// 字典服务
    /// </summary>
    public class EnumToDictionaryService : IEnumToDictionaryService, IScoped
    {
        private readonly IEnumerable<IDictionary> dictionaries;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumToDictionaryService"/> class.
        /// </summary>
        /// <param name="dictionaries"></param>
        public EnumToDictionaryService(IEnumerable<IDictionary> dictionaries)
        {
            this.dictionaries = dictionaries;
        }

        /// <summary>
        /// 查找所有枚举
        /// </summary>
        /// <returns></returns>
        public List<Option> FindAllEnum()
        {
            var finder = new AppDomainAssemblyFinder();
            var types = finder.FindTypes(finder.GetAllAssemblies(), c => c.IsEnum && c.HasAttribute<SaveToDictionaryAttribute>());
            var nameSet = new HashSet<string>();
            var result = new List<Option>();
            foreach (var type in types)
            {
                var typeName = nameSet.Contains(type.Name) ? type.FullName : type.Name;
                if (typeName == null) continue;
                foreach (var key in Enum.GetValues(type))
                {
                    if (key == null) continue;
                    nameSet.Add(typeName);
                    result.Add(new Option
                    {
                        Type = typeName,
                        Key = key.ToString()!,
                        Value = (int)key,
                        Label = type.GetField(key.ToString()!)?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? key.ToString()!,
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 更新列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public void UpdateList(IEnumerable<IDictionary> src, IList<IDictionary> dest)
        {
            foreach (var item in src)
            {
                var d = dest.FirstOrDefault(c => c.Type == item.Type && c.Key == item.Key);
                if (d != null)
                {
                    d.Value = item.Value;
                    d.Label = d.Label ?? item.Label;
                }
                else
                {
                    dest.Add(item);
                }
            }
        }

        /// <summary>
        /// 获取下拉列表选项
        /// </summary>
        /// <returns></returns>
        public object GetSelectOptions()
        {
            var list = this.dictionaries.Where(c => c.IsEnum).ToList();
            var express =
                from item in list
                group item by item.Type into g
                select KeyValuePair.Create(g.Key, g.Select(c => new { c.Label, c.Value }).ToList());
            var result = express.ToDictionary();
            return result;
        }

        /// <summary>
        /// 获取 js 枚举
        /// </summary>
        /// <returns></returns>
        public object GetJsEnum()
        {
            var list = this.dictionaries.Where(c => c.IsEnum).ToList();
            var express =
                from item in list
                group item by item.Type into g
                select KeyValuePair.Create(
                    g.Key,
                    g.Select(c => KeyValuePair.Create(c.Key, c.Value)).ToDictionary());
            var result = express.ToDictionary();
            return result;
        }
    }
}