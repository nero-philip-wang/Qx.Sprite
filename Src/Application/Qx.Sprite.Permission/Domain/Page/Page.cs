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
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using Qx.Sprite.EFCore;

    /// <summary>
    /// 页面
    /// </summary>
    public partial class Page : FullAuditedEntity<int>, IMultiTenant<string>, IOnModelCreating
    {
        /// <summary>
        /// Gets or sets 页面归属于哪个客户端
        /// </summary>
        public string AppId { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面路径
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// Gets or sets 页面重定向路径
        /// </summary>
        public string Redirect { get; set; } = string.Empty!;

        /// <summary>
        /// Gets or sets 页面元数据
        /// </summary>
        [JsonField]
        public PageMeta Meta { get; set; } = new PageMeta();

        /// <summary>
        /// Gets or sets 父页面ID
        /// </summary>
        public int? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the parent page of this page, if one exists.
        /// </summary>
        public virtual Page? Parent { get; set; }

        /// <summary>
        /// Gets or sets 子页面
        /// </summary>
        public virtual ICollection<Page> Children { get; set; } = new List<Page>();

        /// <summary>
        /// Gets or sets 页面中包含的接入点
        /// </summary>
        public virtual ICollection<EndPoint> EndPoints { get; set; } = new List<EndPoint>();

        /// <summary>
        /// Gets or sets 页面中包含的操作
        /// </summary>
        public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();

        /// <summary>
        /// Gets or sets the unique identifier of the tenant associated with the current context.
        /// </summary>
        public string? TenantId { get; set; }

        /// <inheritdoc/>
        object? IMultiTenant.TenantId { get => this.TenantId; set => this.TenantId = (string?)value; }

        /// <inheritdoc/>
        public void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Page>()
               .HasMany(e => e.Children)
               .WithOne(e => e.Parent)
               .HasForeignKey(e => e.ParentId);
        }
    }
}