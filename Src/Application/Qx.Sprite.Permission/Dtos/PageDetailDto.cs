// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 页面
    /// </summary>
    public class PageDetailDto : PageEditDto
    {
        /// <summary>
        /// Gets or sets 页面ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets 创建时间
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// Gets or sets 创建人用户ID
        /// </summary>
        public long? CreatorUserId { get; set; }

        /// <summary>
        /// Gets or sets 最后修改时间
        /// </summary>
        public DateTimeOffset? LastModificationTime { get; set; }

        /// <summary>
        /// Gets or sets 最后修改人用户ID
        /// </summary>
        public long? LastModifierUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets 删除时间
        /// </summary>
        public DateTimeOffset? DeletionTime { get; set; }

        /// <summary>
        /// Gets or sets 删除人用户ID
        /// </summary>
        public long? DeleterUserId { get; set; }
    }
}