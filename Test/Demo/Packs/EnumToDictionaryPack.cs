namespace Demo
{
    using Qx.Sprite.EnumToDictionary;

    /// <summary>
    /// EnumToDictionaryPack
    /// </summary>
    public class EnumToDictionaryPack : Qx.Sprite.EnumToDictionary.EnumToDictionaryPack
    {
        public static List<IDictionary> list = new();

        public EnumToDictionaryPack(IConfiguration configuration, IHostEnvironment env) : base(configuration, env)
        {
        }

        protected override IList<IDictionary> GetItems(IServiceProvider provider)
        {
            return list;
        }

        protected override void SetItems(IEnumerable<IDictionary> dictionary)
        {
            list = dictionary.ToList();
        }
    }
}
