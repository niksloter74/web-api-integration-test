using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Data.Services;
using Xunit;

namespace WebApi.Tests
{
    [AutoRollback]
    public class EntityServiceTest : IntegrationTest
    {
        private readonly IEntityService service;

        public EntityServiceTest(CustomWebApplicationFactory factory) : base(factory)
        {
            service = _serviceProvider.GetRequiredService<IEntityService>();
        }

        private async Task<Entity> AddTestEntity()
        {
            var model = new Entity
            {
                Name = "test entity"
            };
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return model;
        }

        [Fact]
        public async Task CanAdd()
        {
            // arrange
            var model = new Entity
            {
                Name = "new entity"
            };

            // act
            await service.Save(model);

            // assert
            Assert.True(model.Id > 0);
            var result = await _db.Entities.FindAsync(model.Id);
            Assert.Equal(model.Name, result.Name);
        }

        [Fact]
        public async Task CanUpdate()
        {
            // arrange
            var model = await AddTestEntity();
            model.Name = "updated entity";

            // act
            await service.Save(model);

            // assert
            var result = await _db.Entities.FindAsync(model.Id);
            Assert.Equal(model.Name, result.Name);
        }

        [Fact]
        public async Task CannotInsertDuplicate()
        {
            // arrange
            var entity = await AddTestEntity();
            var model = new Entity
            {
                Name = entity.Name
            };

            // act
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await service.Save(model));

            // assert
            Assert.StartsWith("Cannot insert duplicate", ex.InnerException.Message);
        }
    }
}
