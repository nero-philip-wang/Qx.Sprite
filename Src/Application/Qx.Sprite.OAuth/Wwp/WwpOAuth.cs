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
    using Senparc.Weixin;
    using Senparc.Weixin.MP;

    /// <summary>
    /// 微信网页登录
    /// </summary>
    [ServiceType]
    public class WwpOAuth : IScoped, IOAuth
    {
        private readonly ProviderType provider = ProviderType.WechatWebPage;
        private readonly IOAuthKeyProvider appInfoCollection;

        public WwpOAuth(IEnumerable<IOAuthKeyProvider> collections)
        {
            var manager = collections?.FirstOrDefault(x => x.Type == this.provider);
            if (manager == null)
            {
                throw new BusinessException("找不到微信网页登录 APPID 的 Provider");
            }

            this.appInfoCollection = manager;
        }

        public T DecryptData<T>(string encryptedData, string iv, string sessionKey)
        {
            throw new NotImplementedException();
        }

        public OpenIdDto ExchangeCode(string appid, string code)
        {
            var sercet = this.appInfoCollection.GetAppInfo(appid)?.AppSecret;
            var jsCodeResult = Senparc.Weixin.MP.AdvancedAPIs.OAuthApi.GetAccessToken(appid, sercet, code);
            if (jsCodeResult.errcode == ReturnCode.请求成功)
            {
                return new WxWebpageExchangeCodeDto
                {
                    OpenId = jsCodeResult.openid,
                    ErrorCode = jsCodeResult.errcode,
                    AccessToken = jsCodeResult.access_token,
                    ExpiresIn = jsCodeResult.expires_in,
                };
            }
            else
            {
                return new WxWebpageExchangeCodeDto
                {
                    ErrorCode = jsCodeResult.errcode,
                    ErrorMsg = jsCodeResult.errcode.ToString(),
                };
            }
        }
    }
}