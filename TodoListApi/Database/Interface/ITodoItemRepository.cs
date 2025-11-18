using TodoListApi.Models;

namespace TodoListApi.Database.Interface
{
    public interface ITodoItemRepository
    {
        Task<List<TodoItem>> GetAllAsync(int page, int limit);
        Task<TodoItem?> GetItemById(int id);
        Task<TodoItem> AddItem(TodoItem item);
        Task<TodoItem> UpdateItem(TodoItem item);

        Task DeleteItem(int id);

    }
}
