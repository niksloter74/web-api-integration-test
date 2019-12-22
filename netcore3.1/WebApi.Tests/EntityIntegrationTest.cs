using System.Threading.Tasks;
using WebApi.Data.Models;
using Xunit;

namespace WebApi.Tests
{
    [AutoRollback]
    public class EntityIntegrationTest : IntegrationTest
    {
        private const string apiUrl = "/entity";
        public EntityIntegrationTest(CustomWebApplicationFactory factory) : base(factory)
        {
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
            var content = HttpContentHelper.GetJsonContent(model);

            // act
            await _client.PostAsync(apiUrl, content);

            // assert
            Assert.True(model.Id > 0);
            var result = await _db.Entities.FindAsync(model.Id);
            Assert.Equal(model.Name, result.Name);
        }

        [Fact]
        public async Task CanUpdate()
        {
            // arrange
            var entity = await AddTestEntity();
            var model = new Entity
            {
                Id = entity.Id,
                Name = "updated entity"
            };
            var content = HttpContentHelper.GetJsonContent(model);

            // act
            await _client.PostAsync(apiUrl, content);

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
            var content = HttpContentHelper.GetJsonContent(model);

            // act
            var response = await _client.PostAsync(apiUrl, content);

            // assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Cannot insert duplicate", result);
        }
    }
}
