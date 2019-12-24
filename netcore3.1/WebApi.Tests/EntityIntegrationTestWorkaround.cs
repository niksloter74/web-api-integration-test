using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data.Models;
using WebApi.Data.Services;
using Xunit;

namespace WebApi.Tests
{
    public class EntityIntegrationTestWorkaround : IntegrationTest
    {
        private readonly EntityController _controller;
        public EntityIntegrationTestWorkaround(CustomWebApplicationFactory factory) : base(factory)
        {
            var service = _serviceProvider.GetRequiredService<IEntityService>();
            _controller = new EntityController(service);
        }

        [Fact]
        public async Task CanPost()
        {
            // arrange
            var apiUrl = "/entity";
            var model = new Entity
            {
                Name = "new entity"
            };
            var serviceMock = new Mock<IEntityService>();
            serviceMock.Setup(service => service.Save(It.Is<Entity>(e => e.Name == model.Name)));
            var client = CreateClient(builder =>
            {
                builder.ConfigureTestServices(services => 
                {
                    services.AddScoped(provider => serviceMock.Object);
                });
            });
            var content = HttpContentHelper.GetJsonContent(model);

            // act
            var response = await client.PostAsync(apiUrl, content);

            // assert
            response.EnsureSuccessStatusCode();
            serviceMock.VerifyAll();
        }

        [Fact, AutoRollback]
        public async Task CanAdd()
        {
            // arrange
            var model = new Entity
            {
                Name = "new entity"
            };

            // act
            await _controller.Save(model);

            // assert
            var result = await _db.Entities.FirstOrDefaultAsync();
            Assert.Equal(model.Name, result.Name);
        }

        [Fact, AutoRollback]
        public async Task CanUpdate()
        {
            // arrange
            var model = await AddTestEntity();
            model.Name = "updated entity";

            // act
            await _controller.Save(model);

            // assert
            var result = await _db.Entities.FirstOrDefaultAsync();
            Assert.Equal(model.Id, result.Id);
            Assert.Equal(model.Name, result.Name);
        }

        [Fact, AutoRollback]
        public async Task CannotInsertDuplicate()
        {
            // arrange
            var entity = await AddTestEntity();
            var model = new Entity
            {
                Name = entity.Name
            };

            // act
            var ex = await Assert.ThrowsAnyAsync<Exception>(async () => await _controller.Save(model));

            // assert
            Assert.StartsWith("Cannot insert duplicate", ex.InnerException.Message);
        }
    }
}
