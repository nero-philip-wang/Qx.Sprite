// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    /// <summary>
    /// 聚合根
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    [Serializable]
    public abstract class AggregateRoot<TKey> : Entity<TKey>, IConcurrencyCheck, IHasExtraData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TKey}"/> class.
        /// </summary>
        protected AggregateRoot()
        {
            this.ConcurrencyStamp = DateTime.UtcNow.Ticks;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateRoot{TKey}"/> class.
        /// </summary>
        /// <param name="id"></param>
        protected AggregateRoot(TKey id)
            : base(id)
        {
            this.ConcurrencyStamp = DateTime.UtcNow.Ticks;
        }

        /// <inheritdoc/>
        [ConcurrencyCheck]
        public virtual long ConcurrencyStamp { get; set; }

        /// <inheritdoc/>
        [JsonField]
        public Dictionary<string, object?> ExtraData { get; set; } = [];

        /// <inheritdoc/>
        public void SetProperty(string propertyName, object? value)
        {
            (this as IHasExtraData).SetProperty(propertyName, value);
        }

        /// <inheritdoc/>
        public object? GetProperty(string propertyName)
        {
            return (this as IHasExtraData).GetProperty(propertyName);
        }
    }
}