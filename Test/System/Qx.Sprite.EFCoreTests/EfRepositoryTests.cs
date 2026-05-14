// ------------------------------------------------------------
// Copyright (c) Qx.
// Licensed under the MIT License.
// ------------------------------------------------------------
namespace Qx.Sprite.EFCore.Tests
{
    using Qx.Sprite.Core;
    using System.Security.Cryptography;
    using Xunit;

    public class EfRepositoryTests : DependencyTestBase
    {
        public EfRepositoryTests(DependencyInjectionFixture fixture) : base(fixture)
        {
        }

        [Fact()]
        public void CurdTest()
        {
#pragma warning disable xUnit1031 // Do not use blocking task operations in test method
            var repo = this.provider.GetService<IEfRepository<long, User>>();
            repo.CheckNotNull("IEfRepository<User>");

            var userRaw = new User()
            {
                Name = "Test",
                BirthDay = DateTime.Now,
                ExtraData = new Dictionary<string, object?>()
                {
                    { "Test", "Test" }
                },
                UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        Title = "TestRole"
                    }
                }
            };
            userRaw = repo.Add(userRaw, false);
            var line1 = repo.SaveChangesAsync().Result;
            Assert.True(line1 > 0);

            var user = repo.Get(userRaw.Id);
            user.BirthDay = DateTime.Now.AddDays(1);
            repo.Update(user, false);
            var line2 = repo.SaveChangesAsync().Result;
            Assert.True(line2 > 0);

            repo.Remove(user, true);
            Assert.Throws<BusinessException>(() => repo.Get(userRaw.Id));

#pragma warning restore xUnit1031 // Do not use blocking task operations in test method
        }

        [Fact()]
        public void TestRSA()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    var key = rsa.ExportRSAPrivateKeyPem();
                    var pkey = rsa.ExportRSAPublicKeyPem();
                    rsa.PersistKeyInCsp = true;

                    using (var rsa2 = new RSACryptoServiceProvider(2048))
                    {
                        try
                        {
                            var key2 = rsa2.ExportRSAPrivateKeyPem();
                            var pkey2 = rsa2.ExportRSAPublicKeyPem();
                            Assert.NotEqual(key, key2);
                            Assert.NotEqual(pkey, pkey2);
                            rsa2.ImportFromPem(key);
                            key2 = rsa2.ExportRSAPrivateKeyPem();
                            pkey2 = rsa2.ExportRSAPublicKeyPem();
                            Assert.Equal(key, key2);
                            Assert.Equal(pkey, pkey2);

                            var privateKeys = rsa2.ExportParameters(true);
                            var publicKeys = rsa2.ExportParameters(false);
                        }
                        finally
                        {
                            rsa.PersistKeyInCsp = false;
                        }
                    }
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}