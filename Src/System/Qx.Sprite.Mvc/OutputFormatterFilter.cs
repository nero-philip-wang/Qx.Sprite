// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Mvc
{
    using System;
    using System.Buffers;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Qx.Sprite.Core;

    /// <summary>
    /// 格式化Controller输出
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="OutputFormatterFilter"/> class.
    /// </remarks>
    /// <param name="logger"></param>
    /// <param name="env"></param>
    /// <param name="localizer"></param>
    public class OutputFormatterFilter(ILogger<OutputFormatterFilter> logger, IHostEnvironment env, IStringLocalizer<OutputFormatterFilter> localizer)
        : IAsyncResultFilter, IAsyncExceptionFilter
    {
        /// <summary>
        /// Gets or sets a value indicating whether 捕获后抛出异常
        /// </summary>
        public static bool IsThrowException { get; set; }

        /// <inheritdoc/>
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.Exception is BusinessException exception)
            {
                context.Result = new BadRequestObjectResult(new ResponseMessage
                {
                    Message = localizer[context.Exception.Message],
                    Data = exception.ErrorInfo,
                });
            }
            else
            {
                logger.LogError(context.Exception, "未捕获错误");
                string message;
                string contentType;
                if (env.IsProduction())
                {
                    message = new ResponseMessage()
                    {
                        Message = localizer["服务器开小差了"],
                        Data = $"请求id {context.HttpContext.Response.Headers["HttpReports-Trace-SpanId"]}",
                    }.ToJsonString();
                    contentType = "application/json; charset=utf-8";
                }
                else
                {
                    message = new ResponseMessage()
                    {
                        Message = localizer["服务器开小差了"],
                        Data = context.Exception.ToString(),
                    }.ToJsonString();
                    contentType = "application/json; charset=utf-8";
                }

                if (!context.HttpContext.Response.HasStarted)
                {
                    var bytes = message.ToBytes();
                    context.HttpContext.Response.StatusCode = 500;
                    context.HttpContext.Response.Headers["Content-Type"] = contentType;
                    context.HttpContext.Response.ContentLength = bytes.Length;
                    await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
                    await context.HttpContext.Response.Body.FlushAsync();
                }

                if (IsThrowException)
                    throw new Exception($"{context.Exception.Message}{Environment.NewLine}{context.Exception.StackTrace}");
            }
        }

        /// <inheritdoc/>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // 如果模型验证不通过就报错
            if (!context.ModelState.IsValid)
            {
                var dic = context.ModelState.ToDictionary(c => c.Key, c => c.Value?.Errors?.Select(d => d.ErrorMessage));
                context.Result = new BadRequestObjectResult(new ResponseMessage
                {
                    Message = localizer["请检查数据格式"],
                    Data = dic,
                });
            }
            else
            {
                // RawObject特性
                var isRawObject = false;
                if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
                {
                    var method = descriptor.MethodInfo;
                    isRawObject = method.HasAttribute<RawResultAttribute>();
                }

                // 包装输出
                if (!isRawObject && context.Result is ObjectResult objectResult)
                {
                    var data = objectResult.Value;
                    if (data is CountableList)
                    {
                        context.Result = new ObjectResult(new ResponseMessage
                        {
                            Message = localizer["ok"],
                            Total = data.As<CountableList>()?.Total,
                            Data = data.As<CountableList>()?.GetData(),
                        });
                    }
                    else
                    {
                        context.Result = new ObjectResult(new ResponseMessage
                        {
                            Message = localizer["ok"],
                            Data = data,
                        });
                    }
                }

                // 返回为空的情况
                else if (!isRawObject && context.Result is EmptyResult)
                {
                    context.Result = new ObjectResult(new ResponseMessage
                    {
                        Message = localizer["ok"],
                    });
                }
            }

            if (!(context.Result is EmptyResult))
            {
                await next();
            }
            else
            {
                context.Cancel = true;
            }
        }
    }
}