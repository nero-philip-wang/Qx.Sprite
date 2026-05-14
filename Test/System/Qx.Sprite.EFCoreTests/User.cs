namespace Qx.Sprite.EFCore.Tests
{
    using Qx.Sprite.Core;
    using Qx.Sprite.Domain;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User : FullAuditedAggregateRoot<long>, IHasExtraData
    {
        [KeyGenerator(LongKeyGeneratorType.TwitterSnowFlake)]
        public override long Id { get => base.Id; set => base.Id = value; }

        public string Name { get; set; } = null!;
        public DateTime BirthDay { get; set; }

        public virtual IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    public class UserRole : Entity<long>
    {
        public string Title { get; set; } = null!;
    }
}