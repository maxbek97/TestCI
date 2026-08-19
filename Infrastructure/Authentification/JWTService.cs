using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TestCI.Aplication.Auth;
using TestCI.Domain.Users;

namespace TestCI.Infrastructure.Authentification
{
    public class JwtService: IJwtService
    {
        private readonly AuthSettings _settings;
        public JwtService(IOptions<AuthSettings> options)
        {
            _settings = options.Value;
        }
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new("userLogin", user.Login),
            new("userId", user.Id.ToString())
        };

            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.Add(_settings.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_settings.SecretKey)),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(jwtToken);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
