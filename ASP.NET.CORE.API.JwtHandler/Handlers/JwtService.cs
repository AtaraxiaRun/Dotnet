// 引入命名空间，用于处理令牌和加密操作
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

// 定义服务所在的命名空间
namespace ASP.NET.CORE.API.JwtHandler.Handlers
{
    // 定义JwtService类，实现IJwtService接口
    public class JwtService : IJwtService
    {
        // 定义私钥用于令牌签名
        private readonly SymmetricSecurityKey _key;
        // 定义令牌的发行者
        private readonly string _issuer;

        // 类构造器，初始化密钥和发行者
        public JwtService(string key, string issuer)
        {
            // 通过提供的密钥字符串创建加密密钥
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            _issuer = issuer;
        }

        // 方法：生成针对特定用户ID的令牌
        public string GenerateToken(string userId)
        {
            // 创建用于生成令牌的处理程序
            var tokenHandler = new JwtSecurityTokenHandler();
            // 创建签名凭据
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);
            // 定义令牌中的声明
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Iss, _issuer),
                new Claim(JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds().ToString()),
                // ... 其他声明
            };

            // 创建令牌描述符，包含了上述信息
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credentials
            };

            // 生成令牌
            var token = tokenHandler.CreateToken(tokenDescriptor);
            // 将令牌转换为可传输/存储的字符串形式
            return tokenHandler.WriteToken(token);
        }

        // 方法：从提供的令牌字符串中解析出ClaimsPrincipal
        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            // 验证令牌并提取主体
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,  //这个是验证的密钥
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            // 返回ClaimsPrincipal实例
            return principal;
        }

        /// <summary>
        ///  // 方法：生成一个随机密钥：QUhlI9YTiXAZOxyFUH7a+gOMmUjwWb/Y/cS+1DXSgMg=
        /// </summary>
        /// <returns></returns>
        public string GenerateSecretKey()
        {
            var randomNumber = new byte[32]; // 256位随机数
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber); // 使用加密服务提供者填充随机数
                return Convert.ToBase64String(randomNumber); // 将随机数转换为Base64字符串
            }
        }
    }
}
