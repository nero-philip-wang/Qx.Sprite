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
    /// OAuth服务
    /// </summary>
    public interface IOAuth
    {
        /// <summary>
        /// 通过code换取openid和一个辅助信息（可能是access_token或者session_key）
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        OpenIdDto ExchangeCode(string appid, string code);

        /// <summary>
        /// 解密数据
        /// </summary>
        /// <typeparam name="T">Json格式化目标类型</typeparam>
        /// <param name="encryptedData">加密数据</param>
        /// <param name="iv">加密向量</param>
        /// <param name="sessionKey">sessionKey</param>
        /// <returns>解密后的数据</returns>
        T DecryptData<T>(string encryptedData, string iv, string sessionKey);
    }
}