using System.Text.Json;
using Xunit;
using Qx.Sprite.Core;
using Newtonsoft.Json.Linq;

namespace Qx.Sprite.Core.Tests
{
    public class ExtraDataJsonConverterTests : IHasExtraData
    {
        public long LongId { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object?> ExtraData { get; set; } = new();
    }

    public class JsonHasExtraDataTests
    {
        [Fact()]
        public void JsonHasExtraDataTest()
        {
            var obj = new ExtraDataJsonConverterTests() ;
            var inter = obj as IHasExtraData;

            inter.SetProperty(nameof(obj.LongId), 1);
            Assert.Equal(1, obj.LongId);
            Assert.Equal(1, (long)inter.GetProperty(nameof(obj.LongId)));

            var key = "nickname";
            var value = "doge";
            inter.SetProperty(key, value);
            Assert.Equal(value, inter.GetProperty(key));
        }

        [Fact()]
        public void JsonSerialize_ExtraDataToRoot_Test()
        {
            var obj = new ExtraDataJsonConverterTests
            {
                LongId = 1,
                Name = "test"
            };
            obj.ExtraData["nickname"] = "doge";
            obj.ExtraData["age"] = 18;

            var json = JsonSerializer.Serialize(obj, JsonUtils.JsonOptions);
            var jobject = JObject.Parse(json);

            Assert.NotNull(json);
            Assert.Equal(obj.ExtraData["nickname"], jobject["nickname"].ToString());
            Assert.Equal(obj.ExtraData["age"], (int)jobject["age"]);
        }

        [Fact()]
        public void JsonSerialize_PropertyNamingPolicy_Test()
        {
            var obj = new ExtraDataJsonConverterTests
            {
                LongId = 1,
                Name = "test"
            };
            obj.ExtraData["CustomKey"] = "extraValue";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new ExtraDataJsonConverter());

            var json = JsonSerializer.Serialize(obj, options);
            var jobject = JObject.Parse(json);

            Assert.NotNull(json);
            Assert.NotNull(jobject["longId"]);
            Assert.NotNull(jobject["name"]);
            Assert.Null(jobject["LongId"]);
            Assert.Null(jobject["Name"]);
            Assert.Equal("extraValue", jobject["CustomKey"].ToString());
            Assert.Null(jobject["customKey"]);
        }

        [Fact()]
        public void JsonSerialize_PropertyNamingPolicy_SnakeCase_Test()
        {
            var obj = new ExtraDataJsonConverterTests
            {
                LongId = 1,
                Name = "test"
            };
            obj.ExtraData["CustomKey"] = "extraValue";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
            options.Converters.Add(new ExtraDataJsonConverter());

            var json = JsonSerializer.Serialize(obj, options);
            var jobject = JObject.Parse(json);

            Assert.NotNull(json);
            Assert.NotNull(jobject["long_id"]);
            Assert.NotNull(jobject["name"]);
            Assert.Null(jobject["LongId"]);
            Assert.Null(jobject["longId"]);
            Assert.Equal("extraValue", jobject["CustomKey"].ToString());
            Assert.Null(jobject["custom_key"]);
        }

        [Fact()]
        public void JsonSerialize_PropertyNamingPolicy_Null_Test()
        {
            var obj = new ExtraDataJsonConverterTests
            {
                LongId = 1,
                Name = "test"
            };
            obj.ExtraData["CustomKey"] = "extraValue";

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null
            };
            options.Converters.Add(new ExtraDataJsonConverter());

            var json = JsonSerializer.Serialize(obj, options);
            var jobject = JObject.Parse(json);

            Assert.NotNull(json);
            Assert.NotNull(jobject["LongId"]);
            Assert.NotNull(jobject["Name"]);
            Assert.Null(jobject["longId"]);
            Assert.Equal("extraValue", jobject["CustomKey"].ToString());
        }
    }
}