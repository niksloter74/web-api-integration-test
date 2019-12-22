using Microsoft.EntityFrameworkCore;
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
            var response = await _client.PostAsync(apiUrl, content);

            // assert
            response.EnsureSuccessStatusCode();
            var result = await _db.Entities.FirstOrDefaultAsync();
            Assert.Equal(model.Name, result.Name);
        }

        [Fact]
        public async Task CanUpdate()
        {
            // arrange
            var model = await AddTestEntity();
            DetachAll(); // detach all entries because posting to api would create a new model, saving a new object with existing key throws entity already tracked exception
            model.Name = "updated entity";
            var content = HttpContentHelper.GetJsonContent(model);

            // act
            var response = await _client.PostAsync(apiUrl, content);

            // assert
            response.EnsureSuccessStatusCode();
            var result = await _db.Entities.FirstOrDefaultAsync();
            Assert.Equal(model.Id, result.Id);
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
