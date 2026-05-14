// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq.Expressions;
    using System.Text;
    using Qx.Sprite.Core;

    /// <summary>
    /// 审计聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAudited
    {
        /// <inheritdoc/>
        public virtual bool IsDeleted { get; set; }

        /// <inheritdoc/>
        [MaxLength(100)]
        public virtual string? Deleter { get; set; }

        /// <inheritdoc/>
        public virtual DateTime? DeletionTime { get; set; }
    }
}