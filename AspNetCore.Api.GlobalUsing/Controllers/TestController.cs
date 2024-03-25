using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Api.GlobalUsing.Controllers
{
    public class TestController : ControllerBase
    {
        public IActionResult Index()
        {
            WriteLine($"{Environment.NewLine} test"); // using static静态的文件路径
            return Ok();
        }
    }
}
