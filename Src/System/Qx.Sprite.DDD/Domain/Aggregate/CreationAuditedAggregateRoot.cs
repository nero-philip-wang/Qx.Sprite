// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Qx.Sprite.Core;

    /// <summary>
    /// 审计聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAudited
    {
        /// <inheritdoc/>
        public virtual DateTime CreationTime { get; set; }

        /// <inheritdoc/>
        [MaxLength(100)]
        public virtual string? Creator { get; set; }
    }
}