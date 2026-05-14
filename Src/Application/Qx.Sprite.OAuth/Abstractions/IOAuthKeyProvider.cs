// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// OAuth Key管理器
    /// </summary>
    public interface IOAuthKeyProvider
    {
        ProviderType Type { get; }

        OAuthKey? GetAppInfo(string appId);

        string GetAccessToken(string appId);
    }
}