using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using TodoListApi.Database.Interface;
using TodoListApi.Models;
using TodoListApi.Services.Interfaces;

namespace TodoListApi.Services
{
    public class HashService : IHashService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public HashService()
        {
            _passwordHasher = new PasswordHasher<User>();
        }
        public string Hash(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool Verify(User user, string hashedPassword, string password) {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
