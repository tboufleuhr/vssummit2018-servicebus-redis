using VsSummit2018.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VsSummit2018.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class MaintenanceController : Controller
    {
        private readonly VsSummit2018Context VsSummit2018Context;

        public MaintenanceController(VsSummit2018Context VsSummit2018Context)
        {
            this.VsSummit2018Context = VsSummit2018Context;
        }

        [HttpPost("migratedb")]
        public IActionResult MigrateDatabase()
        {
            VsSummit2018Context.Migrate();

            return Ok();
        }
    }
}