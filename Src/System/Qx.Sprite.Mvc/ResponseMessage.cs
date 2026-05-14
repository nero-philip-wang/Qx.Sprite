// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    /// <summary>
    /// 响应消息
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// Gets or sets 消息
        /// </summary>
        public string Message { get; set; } = "ok";

        /// <summary>
        /// Gets or sets 总数
        /// </summary>
        public int? Total { get; set; }

        /// <summary>
        /// Gets or sets 数据详情
        /// </summary>
        public object? Data { get; set; }
    }
}