// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    /// <summary>
    /// 客户端信息
    /// </summary>
    [ServiceType(typeof(IClientInfo))]
    public class ClientAppInfo : Entity<string>, IClientInfo, IScoped
    {
        /// <inheritdoc/>
        public virtual Version Version { get; set; } = new Version(1, 0, 0);

        /// <summary>
        /// Gets or sets 最低版本
        /// </summary>
        public Version MinVersion { get; set; } = new Version(1, 0, 0);

        /// <summary>
        /// Gets or sets appSecret
        /// </summary>
        public virtual string AppSecret { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string Title { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual ClientType Type { get; set; }

        /// <summary>
        /// Gets or sets 有效期开始时间
        /// </summary>
        public virtual DateTime ExpirationStart { get; set; }

        /// <summary>
        /// Gets or sets 有效期结束时间
        /// </summary>
        public virtual DateTime ExpirationEnd { get; set; }

        /// <inheritdoc/>
        [NotMapped]
        public virtual string AppId
        {
            get => this.Id;
            set => this.Id = value;
        }

        /// <inheritdoc/>
        [NotMapped]
        public bool IsValidVersion =>
            this.Version >= this.MinVersion && DateTime.Now >= this.ExpirationStart && DateTime.Now <= this.ExpirationEnd;
    }
}