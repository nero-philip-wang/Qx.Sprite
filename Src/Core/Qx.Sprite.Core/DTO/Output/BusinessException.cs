// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System;
    using System.Net;

    /// <summary>
    /// 业务逻辑错误
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="info"></param>
        public BusinessException(string message, HttpStatusCode code = HttpStatusCode.BadRequest, object? info = null)
            : base(message)
        {
            this.Code = code;
            this.ErrorInfo = info;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <param name="info"></param>
        public BusinessException(string message, int code, object? info = null)
            : base(message)
        {
            this.Code = (HttpStatusCode)code;
            this.ErrorInfo = info;
        }

        /// <summary>
        /// Gets or sets hTTP状态码
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// Gets or sets 错误信息
        /// </summary>
        public object? ErrorInfo { get; set; }
    }
}