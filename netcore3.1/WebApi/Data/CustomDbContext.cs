using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Transactions;
using WebApi.Data.Configurations;
using WebApi.Data.Models;

namespace WebApi.Data
{
    public class CustomDbContext : DbContext
    {
        private const string DefaultConnectionString = "Server=.;Initial Catalog=WebApi;Trusted_Connection=True;";
        private readonly string _connectionString;

        public CustomDbContext()
        {
            _connectionString = DefaultConnectionString;
        }

        public CustomDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Entity> Entities { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EntityConfiguration());
        }

        public async Task Save<TModel>(TModel model)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            {
                Update(model);
                await SaveChangesAsync();
                scope.Complete();
            }
        }
    }
}
