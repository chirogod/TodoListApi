using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoListApi.Database.Interface;
using TodoListApi.Models;

namespace TodoListApi.Database.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly DatabaseContext _context;
        public TodoItemRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<TodoItem?> GetItemById(int id)
        {
            return await _context.TodoItems.FirstOrDefaultAsync(t=>t.Id == id);

        }


        public async Task<TodoItem> AddItem(TodoItem item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<TodoItem> UpdateItem(TodoItem item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task DeleteItem(int id)
        {
            var ToDelete = _context.TodoItems.Find(id);
            if (ToDelete != null) { 
                _context.Remove(ToDelete);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
