using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json")]
    [ApiExplorerSettings(GroupName = "Home")]
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return StatusCode(StatusCodes.Status200OK, "Home");
        }
    }
}
