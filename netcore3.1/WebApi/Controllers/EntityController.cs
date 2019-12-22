using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Data.Models;
using WebApi.Data.Services;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _service;

        public EntityController(IEntityService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Save(Entity model)
        {
            await _service.Save(model);
            return Ok();
        }
    }
}
