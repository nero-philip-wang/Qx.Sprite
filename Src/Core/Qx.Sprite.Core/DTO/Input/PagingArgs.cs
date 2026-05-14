// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Core
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// 分页参数
    /// </summary>
    public class PagingArgs
    {
        /// <summary>
        /// Gets or sets 跳过多少条数据
        /// </summary>
        public int Skip { get; set; } = 0;

        /// <summary>
        /// Gets or sets 取多少条数据
        /// </summary>
        public int Take { get; set; } = 20;

        /// <summary>
        /// Gets or sets a value indicating whether 是否需要提供总数，仅当skip=0时有效
        /// </summary>
        public bool NeedTotal { get; set; } = false;

        /// <summary>
        /// Gets a value indicating whether 是否需要提供总数，只有在NeedTotal为true且Skip为0时才为true
        /// </summary>
        [JsonIgnore]
        public bool NeedTotalReal => this.NeedTotal && this.Skip == 0;
    }
}