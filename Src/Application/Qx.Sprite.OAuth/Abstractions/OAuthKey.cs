// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    /// <summary>
    /// 记录第三方的AppId、AppSecret
    /// </summary>
    public class OAuthKey : Entity<int>
    {
        public virtual ProviderType Provider { get; set; }

        public virtual string AppId { get; set; } = null!;

        public virtual string AppSecret { get; set; } = null!;
    }
}