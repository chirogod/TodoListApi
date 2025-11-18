using TodoListApi.Models;

namespace TodoListApi.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
    }
}
