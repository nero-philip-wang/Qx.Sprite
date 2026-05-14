// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Json字段接口
    /// </summary>
    public interface IHasExtraData
    {
        /// <summary>
        /// Gets or sets 用于存储额外的数据字段
        /// </summary>
        [JsonField]
        Dictionary<string, object?> ExtraData { get; set; }

        /// <summary>
        /// 设置属性值，优先设置实体属性，如果属性不存在则存入ExtraData
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="value">属性值</param>
        public void SetProperty(string propertyName, object? value)
        {
            var property = this.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(this, value);
            }
            else
            {
                this.ExtraData[propertyName] = value;
            }
        }

        /// <summary>
        /// 获取属性值，优先获取实体属性，如果属性不存在则从ExtraData中获取
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public object? GetProperty(string propertyName)
        {
            var property = this.GetType().GetProperty(propertyName);
            if (property != null && property.CanRead)
            {
                return property.GetValue(this);
            }
            else
            {
                return this.ExtraData.ContainsKey(propertyName) ? this.ExtraData[propertyName] : default;
            }
        }
    }
}