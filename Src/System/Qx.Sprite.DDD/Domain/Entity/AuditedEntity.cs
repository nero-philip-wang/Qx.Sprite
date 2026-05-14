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
    /// 审计实体
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    public class AuditedEntity<TKey> : CreationAuditedEntity<TKey>, IModificationAudited
    {
        /// <inheritdoc/>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <inheritdoc/>
        [MaxLength(100)]
        public virtual string? LastModifier { get; set; }
    }
}