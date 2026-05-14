// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Permission
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 用户角色详情数据传输对象
    /// </summary>
    public class UserDetailDto : UserEditDto
    {
        /// <summary>
        /// Gets or sets 角色关联列表
        /// </summary>
        public virtual ICollection<UserDetailDto> Users { get; set; } = new List<UserDetailDto>();

        /// <summary>
        /// Gets or sets 用户角色关联ID
        /// </summary>
        public long Id { get; set; }

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