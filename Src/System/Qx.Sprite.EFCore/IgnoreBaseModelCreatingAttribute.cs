// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;

    /// <summary>
    /// 用于标注实体忽略 onModelCreating 方法
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreBaseModelCreatingAttribute : Attribute
    {
    }
}