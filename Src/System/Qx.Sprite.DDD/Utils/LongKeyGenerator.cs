// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Qx.Sprite.Core;
    using Yitter.IdGenerator;

    /// <summary>
    /// Twitter的分布式全局唯一ID算法，雪花（snowflake）算法.
    /// 项目地址 https://gitee.com/jinjinge/idgenerator/
    /// </summary>
    [ServiceType(typeof(IKeyGenerator), nameof(Int64))]
    public class LongKeyGenerator : IKeyGenerator<long>, ISingleton
    {
        private readonly DefaultIdGenerator snowWorker;

        /// <summary>
        /// Initializes a new instance of the <see cref="LongKeyGenerator"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        public LongKeyGenerator(IConfiguration configuration)
        {
            var workerId = ushort.Parse(configuration["SnowFlake:WorkerId"] ?? "1");
            var datacenterId = ushort.Parse(configuration["SnowFlake:DatacenterId"] ?? "1");

            // 创建 IdGeneratorOptions 对象，请在构造函数中输入 WorkerId：
            var options = new IdGeneratorOptions(workerId);
            options.WorkerIdBitLength = 2; // WorkerIdBitLength 默认值6，支持的 WorkerId 最大值为2^6-1，若 WorkerId 超过64，可设置更大的 WorkerIdBitLength
            options.DataCenterIdBitLength = 2;
            options.SeqBitLength = 5;
            this.snowWorker = new DefaultIdGenerator(options);
        }

        /// <inheritdoc/>
        public long Next(Enum algorithm)
        {
            var type = (LongKeyGeneratorType)algorithm;
            return type switch
            {
                LongKeyGeneratorType.AutoIncrease => 0,
                LongKeyGeneratorType.TwitterSnowFlake => this.snowWorker.NewLong(),
                _ => 0,
            };
        }
    }
}