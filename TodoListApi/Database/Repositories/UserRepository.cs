using Microsoft.EntityFrameworkCore;
using TodoListApi.Database.Interface;
using TodoListApi.Models;

namespace TodoListApi.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context) {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
             return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
