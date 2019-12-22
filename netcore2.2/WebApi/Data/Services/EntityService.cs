using System.Threading.Tasks;
using WebApi.Data.Models;

namespace WebApi.Data.Services
{
    public class EntityService : IEntityService
    {
        private readonly CustomDbContext _db;

        public EntityService(CustomDbContext db)
        {
            _db = db;
        }

        public async Task Save(Entity model) => await _db.Save(model);
    }
}
