using Microsoft.AspNetCore.Mvc;

namespace LessonTool.API.Endpoint.Controllers;

[ApiController]
[Route("/api/polling/alive")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok("Api is alive");
    }
}
