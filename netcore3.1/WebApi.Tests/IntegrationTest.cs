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
        protected readonly HttpClient _client;
        protected readonly IServiceProvider _serviceProvider;
        protected readonly CustomDbContext _db;

        public IntegrationTest(CustomWebApplicationFactory factory)
        {
            factory.Server.PreserveExecutionContext = true; // this line fixed the issue with nested transaction with HttpClient
            _client = factory.CreateClient();
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _db = _serviceProvider.GetRequiredService<CustomDbContext>();
        }

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
