using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoListApi.Database.Interface;
using TodoListApi.DTOs;
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
        public async Task<PaginationDTO<TodoItem>> GetAllAsync(string idUser, int page, int limit)
        {
            //establecer valores por defecto si no vienen
            if (page < 1) page = 1;
            if (limit < 1) limit = 10;

            //el offset es la cantidad q se saltea antes de empezar a mostrar
            int offset = (page - 1) * limit;

            int totalItems = await _context.TodoItems.Where(p => p.UserId == idUser).CountAsync();

            var items = await _context.TodoItems.Where(p => p.UserId == idUser).Skip(offset).Take(limit).ToListAsync();//skipea la cantidad q diga offset y toma la cantidad que dice el limite donde coincidad id users.

            //creamos el objeto para pasar la metadata
            return new PaginationDTO<TodoItem>()
            {
                Data = items,
                Page = page,
                Limit = limit,
                Total = totalItems
            };

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
