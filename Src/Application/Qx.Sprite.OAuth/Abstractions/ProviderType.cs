// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum ProviderType : byte
    {
        /// <summary>
        /// 微信小程序
        /// </summary>
        WechatMiniProgram = 0x01,

        /// <summary>
        /// 微信公众号
        /// </summary>
        WechatWebPage = 0x01 << 1,

        /// <summary>
        /// 企业微信
        /// </summary>
        WechatWork = 0x04,

        /// <summary>
        /// 钉钉
        /// </summary>
        DingTalk = 0x08,
    }
}