using UserApi.Aplication.Contracts;
using UserApi.Aplication.Interfaces;
using UserApi.Aplication.Mappers;
using UserApi.Core.Exceptions;
using UserApi.DataAccess.Interfaces;

namespace UserApi.Aplication.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> AddUserAsync(UserRequest userRequest)
    {
        return await _userRepository.AddUserAsync(userRequest.MapToCore());
    }

    public async Task DeleteUserAsync(int id)
    {
        var isUserExists = await UserExistsAsync(id);
        if (!isUserExists)
        {
            throw new UserNotFoundException(id);
        }
        await _userRepository.DeleteUserAsync(id);
    }

    public async Task<UserResponse> GetUserAsync(int id)
    {
        var isUserExists = await UserExistsAsync(id);
        if (!isUserExists)
        {
            throw new UserNotFoundException(id);
        }
        var user = await _userRepository.GetUserAsync(id);
        return user.MapToContract();
    }

    public async Task UpdateUserAsync(int id, UserRequest userRequest)
    {
        var isUserExists = await UserExistsAsync(id);
        if (!isUserExists)
        {
            throw new UserNotFoundException(id);
        }
        await _userRepository.UpdateUserAsync(id, userRequest.MapToCore());
    }

    public async Task<bool> UserExistsAsync(int id)
    {
        return await _userRepository.UserExistsAsync(id);
    }
}