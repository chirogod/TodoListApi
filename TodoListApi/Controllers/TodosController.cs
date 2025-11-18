using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoListApi.Database.Interface;
using TodoListApi.DTOs;
using TodoListApi.Models;

namespace TodoListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase
    {
        private readonly ITodoItemRepository _repository;

        public TodosController(ITodoItemRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page, int limit) {

            var items = await _repository.GetAllAsync(page,limit);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ItemDTO item)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(UserId)) {
                return Unauthorized("No se pudo verificar al usuario desde el token.");
            }
            TodoItem i = new TodoItem()
            {
                Title = item.Title,
                Description = item.Description,
                UserId = UserId
            };
            TodoItem created = await _repository.AddItem(i);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody]ItemDTO item) {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existingItem = await _repository.GetItemById(id);
            if (existingItem == null) {
                return NotFound();
            }

            if(UserId != existingItem.UserId)
            {
                return Forbid();
            }
            existingItem.Title = item.Title;
            existingItem.Description = item.Description;

            TodoItem i = await _repository.UpdateItem(existingItem);
            return Ok(i);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var existingItem = await _repository.GetItemById(id);
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (existingItem == null)
            {
                return NotFound();
            }
            else if (existingItem.UserId != UserId) {
                return Forbid();
            }

            await _repository.DeleteItem(id);
            return NoContent();
        }


    }
}
