using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using WebApi.Data;
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
            _client = factory.CreateClient();
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _db = _serviceProvider.GetRequiredService<CustomDbContext>();
        }
    }
}
