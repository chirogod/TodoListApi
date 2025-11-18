using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Models;
using TodoListApi.Services.Interfaces;

namespace TodoListApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration configuration) { 
            _config = configuration;
        }
        public string GenerateJwtToken(User user)
        {
            // A. Definir los Claims (información del usuario que viaja en el token)
            var claims = new List<Claim>
            {
                // Sub: Identificador único del usuario (es el claim más importante)
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // B. Obtener la Clave Secreta desde la configuración
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtConfig:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));

            // C. Crear las Credenciales (firma)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // D. Crear el Descriptor del Token (quién lo emite, para quién, expiración, etc.)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JwtConfig:Issuer"],
                Audience = _config["JwtConfig:Audience"]
            };

            // E. Crear y Serializar el Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Devuelve el token como string
        }
    }
}
