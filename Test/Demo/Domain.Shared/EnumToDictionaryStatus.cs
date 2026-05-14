namespace Demo.Domain.Shared
{
    using Qx.Sprite.EnumToDictionary;
    using System.ComponentModel;

    [SaveToDictionary]
    public enum EnumToDictionaryStatus
    {
        [Description("待处理")]
        Pending = 0,
        [Description("已处理")]
        Active = 1,
        [Description("已取消")]
        Canceled = 2,
    }
}
