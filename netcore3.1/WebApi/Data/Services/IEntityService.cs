using System.Threading.Tasks;
using WebApi.Data.Models;

namespace WebApi.Data.Services
{
    public interface IEntityService
    {
        Task Save(Entity model);
    }
}
