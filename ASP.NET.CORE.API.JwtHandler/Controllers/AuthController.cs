using ASP.NET.CORE.API.JwtHandler.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IJwtService _jwtService;

    public AuthController(IJwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(string userName, string password)
    {
        // 验证用户凭据（此处省略详细的用户验证过程）
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            var userId = "some-user-id-based-on-credentials";
            var token = _jwtService.GenerateToken(userId);
            return Ok(token);
        }

        return NotFound();


    }

    [HttpGet("protected")]
    [Authorize]
    public IActionResult Protected()
    {
        // 只有验证过的用户才能访问
        return Ok("Access to protected resource granted.");
    }
}
