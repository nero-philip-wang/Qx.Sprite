// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1601:Partial elements should be documented", Justification = "Reviewed.")]
    public abstract partial class Repository<TKey, TEntity>
    {
        /// <inheritdoc/>
        public Type ElementType => this.GetQuerySet(this.ignoreFlag).AsQueryable().ElementType;

        /// <inheritdoc/>
        public Expression Expression => this.GetQuerySet(this.ignoreFlag).AsQueryable().Expression;

        /// <inheritdoc/>
        public IQueryProvider Provider => this.GetQuerySet(this.ignoreFlag).AsQueryable().Provider;

        /// <inheritdoc/>
        public void SetQueryFilterIgnoreFlag(QueryFilterIgnoreFlag ignoreFlag)
        {
            this.ignoreFlag = ignoreFlag;
        }

        /// <inheritdoc/>
        public IEnumerator<TEntity> GetEnumerator() => this.GetQuerySet(this.ignoreFlag).AsQueryable().GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}