using Microsoft.Extensions.Configuration;
using Qx.Sprite.Core;
using Qx.Sprite.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Qx.Sprite.Domain.Tests
{
    public class LongKeyGeneratorTests
    {
        [Fact()]
        public void NextTest()
        {
            var gen = new LongKeyGenerator(new ConfigurationBuilder().Build());
            var id1 = gen.Next(LongKeyGeneratorType.TwitterSnowFlake);
            var id2 = gen.Next(LongKeyGeneratorType.TwitterSnowFlake);

            Assert.True(id1 < id2);
        }
    }
}