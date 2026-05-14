// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionaryTests
{
    using Qx.Sprite.Core;
    using Qx.Sprite.EnumToDictionary;
    using Xunit;

    public sealed class DictionaryServiceTest
    {
        [Fact()]
        public void TestMethod1()
        {
            var list = new List<Option>();
            var dic = new EnumToDictionaryService(list);

            AppDomainAssemblyFinder.InitFilePrefix(["Qx.Sprite.EnumToDictionaryTests"]);

            var idc = dic.FindAllEnum();
            list.AddRange(idc);
            Assert.Contains(idc, c => c.Type == nameof(MyEnum));

            var enus = dic.GetJsEnum();
            var opt = dic.GetSelectOptions();

            Assert.NotNull(enus);
            Assert.NotNull(opt);
        }
    }
}