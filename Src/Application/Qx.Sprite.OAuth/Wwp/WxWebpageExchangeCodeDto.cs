// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.OAuth
{
    using Senparc.Weixin;

    public class WxWebpageExchangeCodeDto : OpenIdDto
    {
        public ReturnCode ErrorCode { get; set; }

        public string ErrorMsg { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public string AccessToken { get; set; } = string.Empty;
    }
}