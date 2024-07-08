using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Se130RPGGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get(string? model)
        {
            Console.WriteLine(model.Length);
            return default;
        }
    }
}
