using Microsoft.EntityFrameworkCore;
using SelfServicePortal.Data;
using SelfServicePortal.Models;

namespace SelfServicePortal.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        public UserService(AppDbContext db) { _db = db; }

        public async Task<User?> Login(string email, string password)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            bool isValid = BCrypt.Net.BCrypt
                .Verify(password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<bool> Register(
            string fullName, string email, string password)
        {
            if (await EmailExists(email)) return false;
            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = "User",
                CreatedAt = DateTime.Now
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetAllAdmins()
        {
            return await _db.Users
                .Where(u => u.Role == "Admin").ToListAsync();
        }

        public async Task<User?> GetUserById(int userId)
        {
            return await _db.Users.FindAsync(userId);
        }
    }
}