// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Asp.Versioning;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// API基础控制器
    /// </summary>
    [ApiController]
    [Controller]
    [ApiVersion("1.0")]
    [Area("default")]
    [Route("api/v{version:apiVersion}/[area]/[controller]/")]
    public abstract class ApiBaseController : ControllerBase
    {
    }
}