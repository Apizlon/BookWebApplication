using UserApi.Core.Models;

namespace UserApi.DataAccess.Interfaces;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<User> GetUserAsync(int id);
    Task UpdateUserAsync(int id, User user);
    Task<bool> UserExistsAsync(int id);
}