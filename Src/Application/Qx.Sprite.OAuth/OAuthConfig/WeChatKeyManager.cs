// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Qx.Sprite.Core;
    using Senparc.Weixin.MP.Containers;

    public class WeChatKeyManager(IEnumerable<OAuthKey> oAuthKeys) : IOAuthKeyProvider, IScoped
    {
        public ProviderType Type => ProviderType.WechatMiniProgram | ProviderType.WechatWebPage | ProviderType.WechatWork;

        public OAuthKey? GetAppInfo(string appId)
        {
            var result = oAuthKeys.Where(w => w.AppId == appId).FirstOrDefault();
            return result;
        }

        public string GetAccessToken(string appId)
        {
            if (!AccessTokenContainer.CheckRegistered(appId))
            {
                var sercet = this.GetAppInfo(appId)?.AppSecret;
                AccessTokenContainer.RegisterAsync(appId, sercet).Wait();
            }

            return AccessTokenContainer.GetAccessToken(appId);
        }
    }
}