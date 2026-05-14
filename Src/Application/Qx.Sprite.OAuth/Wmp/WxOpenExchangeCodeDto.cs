// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using Senparc.Weixin;

    public class WxOpenExchangeCodeDto : OpenIdDto
    {
        public ReturnCode ErrorCode { get; set; }

        public string ErrorMsg { get; set; } = string.Empty;

        public string SessionKey { get; set; } = string.Empty;

        public string Unionid { get; set; } = string.Empty;
    }
}