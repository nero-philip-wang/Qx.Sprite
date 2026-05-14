// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Qx.Sprite.Core;

    /// <summary>
    /// Guid 主键生成器
    /// </summary>
    /// <remarks>
    /// 源自：https://github.com/jhtodd/SequentialGuid/blob/master/SequentialGuid/Classes/SequentialGuid.cs
    /// </remarks>
    [ServiceType(typeof(IKeyGenerator), nameof(Guid))]
    public class GuidKeyGenerator : IKeyGenerator<Guid>, ISingleton
    {
        /// <summary>
        /// 为GUID的创建提供加密强随机数据。
        /// </summary>
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <inheritdoc/>
        public Guid Next(Enum algorithm)
        {
            return (GuidKeyGeneratorType)algorithm switch
            {
                GuidKeyGeneratorType.SequentialAsString => this.Create((GuidKeyGeneratorType)algorithm),
                GuidKeyGeneratorType.SequentialAsBinary => this.Create((GuidKeyGeneratorType)algorithm),
                GuidKeyGeneratorType.SequentialAtEnd => this.Create((GuidKeyGeneratorType)algorithm),
                _ => Guid.NewGuid(),
            };
        }

        /// <summary>
        /// 生成指定类型的GUID
        /// </summary>
        private Guid Create(GuidKeyGeneratorType guidType)
        {
            byte[] randomBytes = new byte[10];
            Rng.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];
            switch (guidType)
            {
                case GuidKeyGeneratorType.SequentialAsString:
                case GuidKeyGeneratorType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to reverse the order
                    // of the Data1 and Data2 blocks on little-endian systems.
                    if (guidType == GuidKeyGeneratorType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }

                    break;

                case GuidKeyGeneratorType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(guidType), guidType, null);
            }

            return new Guid(guidBytes);
        }
    }
}