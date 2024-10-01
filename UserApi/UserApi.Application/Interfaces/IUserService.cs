using UserApi.Application.Contracts;

namespace UserApi.Application.Interfaces;

public interface IUserService
{
    Task<int> AddUserAsync(UserRequest userRequest);
    Task DeleteUserAsync(int id);
    Task<UserResponse> GetUserAsync(int id);
    Task UpdateUserAsync(int id, UserRequest userRequest);
    Task<bool> UserExistsAsync(int id);
}