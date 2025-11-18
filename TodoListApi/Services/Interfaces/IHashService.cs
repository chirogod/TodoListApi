using TodoListApi.Models;

namespace TodoListApi.Services.Interfaces
{
    public interface IHashService
    {
        string Hash(User user, string password);
        bool Verify(User user, string hashedPassword, string providedPassword);
    }
}
