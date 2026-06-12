using SelfServicePortal.Models;

namespace SelfServicePortal.Services
{
    public interface IUserService
    {
        Task<User?> Login(string email, string password);
        Task<bool> Register(string fullName, string email, string password);
        Task<bool> EmailExists(string email);
        Task<List<User>> GetAllAdmins();
        Task<User?> GetUserById(int userId);
    }
}