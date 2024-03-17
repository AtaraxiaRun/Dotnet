using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASP.NET.CORE.API.JwtHandler.Handlers
{
    public interface IJwtService
    {
        string GenerateToken(string userId);
        ClaimsPrincipal GetPrincipalFromToken(string token);

        string GenerateSecretKey();
    }


}
