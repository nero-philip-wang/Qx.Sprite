// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Auth
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using MapsterMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Options;
    using Qx.Sprite.Core;

    /// <summary>
    /// 应用Id授权处理程序
    /// </summary>
    public class AppIdHandler : AuthorizationHandler<AppIdRequirement>, IScoped
    {
        private readonly IHostEnvironment env;
        private readonly IClientInfo clientInfo;
        private readonly IEnumerable<ClientAppInfo> set;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<AppIdHandler> l;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppIdHandler"/> class.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="mapper"></param>
        /// <param name="l"></param>
        /// <param name="clientInfo"></param>
        /// <param name="set"></param>
        public AppIdHandler(IHostEnvironment env, IMapper mapper, IStringLocalizer<AppIdHandler> l, IClientInfo clientInfo, IEnumerable<ClientAppInfo> set)
        {
            this.env = env;
            this.clientInfo = clientInfo;
            this.set = set;
            this.mapper = mapper;
            this.l = l;
        }

        /// <inheritdoc/>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AppIdRequirement requirement)
        {
            try
            {
                var request = context.Resource?.As<HttpContext>()?.Request;

                var appid = request?.Headers["AppId"];
                var stamp = request?.Headers["Stamp"];
                var nonce = request?.Headers["Nonce"];
                var sign = request?.Headers["Sign"];

                if (appid.ToString().IsNullOrEmpty())
                {
                    throw new BusinessException("需要提供AppId", HttpStatusCode.Forbidden);
                }

                if (!this.env.IsDevelopment())
                {
                    var info = this.CheckSign(appid!, stamp!, nonce!, sign!);
                    this.mapper.Map(info, this.clientInfo);
                }
                else
                {
                    var info = new ClientAppInfo() { Id = appid.ToString()!, Title = "开发环境" };
                    this.mapper.Map(info, this.clientInfo);
                }

                context.Succeed(requirement);
            }
            catch (Exception)
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// 生产签名的算法
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="stamp"></param>
        /// <param name="nonce"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"> 签名错误 </exception>
        protected virtual ClientAppInfo? CheckSign(string appid, string stamp, string nonce, string sign)
        {
            if (stamp.As<DateTime>().Subtract(DateTime.Now).TotalMinutes > 5)
                throw new BusinessException("时间戳不匹配", HttpStatusCode.Forbidden);
            var clientinfo = this.set.FirstOrDefault(c => c.Id == appid);
            var appSecret = clientinfo?.AppSecret;
            if (appSecret == null)
                throw new BusinessException("AppId不匹配", HttpStatusCode.Forbidden);

            var serveSign = $"{appid}{stamp}{nonce}{appSecret}".ToMd5Hash();
            if (sign != serveSign)
                throw new BusinessException("签名不匹配", HttpStatusCode.Forbidden);

            return clientinfo;
        }
    }
}