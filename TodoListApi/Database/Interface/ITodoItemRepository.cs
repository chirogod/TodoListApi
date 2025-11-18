using TodoListApi.DTOs;
using TodoListApi.Models;

namespace TodoListApi.Database.Interface
{
    public interface ITodoItemRepository
    {
        Task<PaginationDTO<TodoItem>> GetAllAsync(string idUser, int page, int limit);
        Task<TodoItem?> GetItemById(int id);
        Task<TodoItem> AddItem(TodoItem item);
        Task<TodoItem> UpdateItem(TodoItem item);
        Task DeleteItem(int id);

    }
}
