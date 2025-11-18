using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Database.Interface;
using TodoListApi.DTOs;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IUserRepository _userRepository;

        public LoginController(IConfiguration configuration, IUserRepository userRepository)
        {
            _config = configuration;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _userRepository.GetUserByEmailAsync(login.Email);
            if (user == null || user.Password != login.Password)
            {
                return Unauthorized();
            }
            var token = GenerateJwtToken(user);
            return Ok(token);
        }

        private string GenerateJwtToken(User user)
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
