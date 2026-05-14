// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Caching
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 标记需要被 EasyCaching 拦截的类
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class EasyCachingMarkAttribute : Attribute
    {
    }
}
