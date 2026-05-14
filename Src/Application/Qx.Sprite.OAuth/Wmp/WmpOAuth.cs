// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Newtonsoft.Json;
    using Qx.Sprite.Core;
    using Senparc.Weixin;
    using Senparc.Weixin.WxOpen.Helpers;

    /// <summary>
    /// 微信小程序登录
    /// </summary>
    [ServiceType]
    public class WmpOAuth : IScoped, IOAuth
    {
        private readonly ProviderType provider = ProviderType.WechatMiniProgram;
        private readonly IOAuthKeyProvider appInfoCollection;

        public WmpOAuth(IEnumerable<IOAuthKeyProvider> collections)
        {
            var manager = collections?.FirstOrDefault(x => x.Type == this.provider);
            if (manager == null)
            {
                throw new BusinessException("找不到微信小程序 APPID 的 Provider");
            }

            this.appInfoCollection = manager;
        }

        public T DecryptData<T>(string encryptedData, string iv, string sessionKey)
        {
            var jsonResult = EncryptHelper.DecodeEncryptedData(sessionKey, encryptedData, iv);
            return JsonConvert.DeserializeObject<T>(jsonResult)!;
        }

        public OpenIdDto ExchangeCode(string appid, string code)
        {
            var sercet = this.appInfoCollection.GetAppInfo(appid)?.AppSecret;
            var jsCodeResult = Senparc.Weixin.WxOpen.AdvancedAPIs.Sns.SnsApi.JsCode2Json(appid, sercet, code);
            if (jsCodeResult.errcode == ReturnCode.请求成功)
            {
                return new WxOpenExchangeCodeDto
                {
                    OpenId = jsCodeResult.openid,
                    ErrorCode = jsCodeResult.errcode,
                    Unionid = jsCodeResult.unionid,
                    SessionKey = jsCodeResult.session_key,
                };
            }
            else
            {
                return new WxOpenExchangeCodeDto
                {
                    ErrorCode = jsCodeResult.errcode,
                    ErrorMsg = jsCodeResult.errcode.ToString(),
                };
            }
        }

        public Stream GetWxaCode(string appid, string page, string sid, string pid)
        {
            var token = this.appInfoCollection.GetAccessToken(appid);
            var buff = new MemoryStream();
            var res = Senparc.Weixin.WxOpen.AdvancedAPIs.WxApp.WxAppApi.GetWxaCodeUnlimit(token, buff, $"{sid}&{pid}", page, width: 360);
            buff.Seek(0, SeekOrigin.Begin);
            return buff;
        }
    }
}