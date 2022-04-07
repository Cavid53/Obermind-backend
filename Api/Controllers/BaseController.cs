using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Api.Controllers
{
    [ApiController]
    [Route("v{ver:apiVersion}/[controller]")]
    [OpenApiTag("App")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BaseController : ControllerBase
    {

    }
}
