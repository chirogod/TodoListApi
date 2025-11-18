using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Database.Interface;
using TodoListApi.DTOs;
using TodoListApi.Models;
using TodoListApi.Services.Interfaces;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IUserRepository _userRepository;
        private ITokenService _tokenService;
        private IHashService _hashService;
        public LoginController(IConfiguration configuration, IUserRepository userRepository, ITokenService tokenService, IHashService hashService)
        {
            _config = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var user = await _userRepository.GetUserByEmailAsync(login.Email);
            if (user == null)
            {
                return Unauthorized("Usuario no encontrado.");
            }

            var pass = _hashService.Verify(user, user.Password, login.Password);
                        
            if (!pass)
            {
                return Unauthorized("Contrasena incorrecta");
            }
            var token = _tokenService.GenerateJwtToken(user);
            return Ok(token);
        }

        

        
    }
}
