using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Data;

namespace WebApi.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        private const string TestDbConnectionString = "Server=.;Initial Catalog=WebApiTestDB_V2;Trusted_Connection=True;";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(_ => new CustomDbContext(TestDbConnectionString));

                var sp = services.BuildServiceProvider();
                var db = sp.GetRequiredService<CustomDbContext>();
                db.Database.Migrate();
            });
        }
    }
}
