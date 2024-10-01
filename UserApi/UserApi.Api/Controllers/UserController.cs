using Microsoft.AspNetCore.Mvc;
using UserApi.Aplication.Contracts;
using UserApi.Aplication.Interfaces;

namespace UserApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] UserRequest userRequest)
    {
        var userId = await _userService.AddUserAsync(userRequest);
        return Ok(userId);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var userResponse = await _userService.GetUserAsync(id);
        return Ok(userResponse);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateUserAsync(int id, UserRequest userRequest)
    {
        await _userService.UpdateUserAsync(id, userRequest);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUserAsync(int id)
    {
        await _userService.DeleteUserAsync(id);
        return Ok();
    }
}