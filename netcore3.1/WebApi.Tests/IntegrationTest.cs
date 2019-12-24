using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Data.Models;
using Xunit;

namespace WebApi.Tests
{
    public class IntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        protected CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly CustomDbContext _db;

        public IntegrationTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _serviceProvider = _factory.Services.CreateScope().ServiceProvider;
            _db = _serviceProvider.GetRequiredService<CustomDbContext>();
        }

        protected HttpClient CreateClient(Action<IWebHostBuilder> configuration) 
            => _factory.WithWebHostBuilder(configuration).CreateClient();

        protected void DetachAll()
        {
            _db.ChangeTracker.Entries()
                .ToList()
                .ForEach(e => e.State = EntityState.Detached);
        }

        protected async Task<Entity> AddTestEntity()
        {
            var model = new Entity
            {
                Name = "test entity"
            };
            await _db.AddAsync(model);
            await _db.SaveChangesAsync();
            return model;
        }
    }
}
