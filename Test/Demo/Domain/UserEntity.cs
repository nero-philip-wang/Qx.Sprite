namespace Demo.Domain
{
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;

    public class UserEntity : FullAuditedAggregateRoot<Guid>
    {
        [KeyGenerator(GuidKeyGeneratorType.SequentialAsBinary)]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        public string Name { get; set; }

        public int Age { get; set; } = 0;
    }
}
