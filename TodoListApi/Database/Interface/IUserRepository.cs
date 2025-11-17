using TodoListApi.Models;

namespace TodoListApi.Database.Interface
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task AddUser(User user);
    }
}
