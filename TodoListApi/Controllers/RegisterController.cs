using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoListApi.Database.Interface;
using TodoListApi.Models;
using TodoListApi.Services.Interfaces;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IUserRepository _userRepository;
        private ITokenService _tokenService;
        private IHashService _hashService;
        
        public RegisterController(IConfiguration configuration, IUserRepository userRepository, ITokenService tokenService, IHashService hashService) { 
            _config = configuration;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hashService = hashService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var exist = await _userRepository.GetUserByEmailAsync(user.Email);
            if (exist != null) {
                return Conflict("El correo ya se encuentra registrado.");
            }

            User newuser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = _hashService.Hash(user, user.Password)
            };
            await _userRepository.AddUser(newuser);
            var token = _tokenService.GenerateJwtToken(newuser);
            return Ok(token);
        }
    }
}
