using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // BASE controller where other class will be inheriting from here
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {

    }
}