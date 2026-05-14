// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;

    /// <summary>
    /// 标记该特性就不会包装该方法的输出结果
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class RawResultAttribute : Attribute
    {
    }
}