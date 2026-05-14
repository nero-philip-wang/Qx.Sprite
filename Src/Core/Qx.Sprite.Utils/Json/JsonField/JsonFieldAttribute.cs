// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示实体类中的一个字段是Json格式的字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonFieldAttribute : NotMappedAttribute
    {
    }
}