using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TodoListApi.Database.Interface;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository) { 
            _userRepository = userRepository;
        }

        public async Task<ActionResult> Index() {
            List<User> users = await _userRepository.GetAllAsync();
            return Ok(users);
        }
    }
}
