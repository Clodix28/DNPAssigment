using ApiContracts.Users;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        bool usernameTaken = await _userRepository.GetMany()
            .AnyAsync(u => u.Username.ToLower() == request.Username.ToLower());
        if (usernameTaken)
        {
            return BadRequest($"Username '{request.Username}' is already taken.");
        }

        User user = new User
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email ?? string.Empty
        };

        User created = await _userRepository.AddAsync(user);
        UserDto dto = ToDto(created);
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string? userNameContains = null)
    {
        List<UserDto> users = await _userRepository.GetMany()
            .Where(u => userNameContains == null ||
                        u.Username.ToLower().Contains(userNameContains.ToLower()))
            .Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            })
            .ToListAsync();

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle([FromRoute] int id)
    {
        try
        {
            User user = await _userRepository.GetSingleAsync(id);
            return Ok(ToDto(user));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser([FromRoute] int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = new User
            {
                Id = id,
                Username = request.Username,
                Password = request.Password,
                Email = request.Email ?? string.Empty
            };
            User updated = await _userRepository.UpdateAsync(user);
            return Ok(ToDto(updated));
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser([FromRoute] int id)
    {
        try
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private static UserDto ToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };
    }
}
