using ApiContracts.Auth;
using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        // usernames are unique ignoring case (see UsersController), so match the same way here
        User? user = await _userRepository.GetMany()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == request.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        if (user.Password != request.Password)
        {
            return Unauthorized("Wrong password.");
        }

        // send back a dto so the password never leaves the server
        UserDto dto = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
        return Ok(dto);
    }
}
