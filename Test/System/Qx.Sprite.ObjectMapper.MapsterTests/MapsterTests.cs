namespace Qx.Sprite.EFCore.Tests
{
    using Mapster;
    using MapsterMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Qx.Sprite.Core;
    using Qx.Sprite.ObjectMapper.Mapster;
    using System.Net;
    using Xunit;

    public class MapsterTests : DependencyTestBase
    {
        private readonly IMapper? map;

        public MapsterTests(DependencyInjectionFixture fixture) : base(fixture)
        {
            this.map = fixture.Service.GetService<IMapper>();
        }

        [Fact()]
        public void CurdTest()
        {
            var dog = new Dog { Name = "旺财", Type = "狗", Breed = "金毛" };
            var dto = map.Map<DogDto>(dog);
            Assert.Equal(dog.Name, dto.Name);
        }
    }

    public class MapperConfiguration : IMapperConfiguration
    {
        public void AddMapper(TypeAdapterConfig cfg)
        {
            cfg.NewConfig<Animal, AnimalDto>().Include<Dog, DogDto>();
        }
    }

    public class Animal
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Dog : Animal
    {
        public string Breed { get; set; }
    }

    public class AnimalDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class DogDto : AnimalDto
    {
        public string Breed { get; set; }
    }
}