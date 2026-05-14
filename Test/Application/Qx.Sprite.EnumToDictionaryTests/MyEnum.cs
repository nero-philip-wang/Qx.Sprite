// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------

namespace Qx.Sprite.EnumToDictionaryTests
{
    using System.ComponentModel;
    using Qx.Sprite.EnumToDictionary;

    [SaveToDictionary]
    public enum MyEnum
    {
        [Description("选项一")]
        Option1 = 1,

        [Description("选项二")]
        Option2 = 2,

        [Description("选项三")]
        Option3 = 3,
    }

    [SaveToDictionary]
    public enum OrderStatus
    {
        [Description("待支付")]
        PendingPayment = 1,

        [Description("已支付")]
        Paid = 2,

        [Description("已发货")]
        Shipped = 3,

        [Description("已完成")]
        Completed = 4,

        [Description("已取消")]
        Canceled = 5,
    }
}