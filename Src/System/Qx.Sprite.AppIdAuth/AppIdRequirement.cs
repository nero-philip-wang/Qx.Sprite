// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// 应用Id授权需求
    /// </summary>
    public class AppIdRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdRequirement"/> class.
        /// </summary>
        public AppIdRequirement()
        {
        }
    }
}