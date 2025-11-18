using TodoListApi.Models;

namespace TodoListApi.Database.Interface
{
    public interface ITodoItemRepository
    {
        Task<TodoItem?> GetItemById(int id);
        Task<TodoItem> AddItem(TodoItem item);
        Task<TodoItem> UpdateItem(TodoItem item);

        Task DeleteItem(int id);

    }
}
