using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UsersController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserDto request)
    {
        await VerifyUserNameIsAvailableAsync(request.UserName);
        
        User user = new User
        {
            UserName = request.UserName,
            Password = request.Password
        };
        User created = await userRepo.AddAsync(user);
        
        UserDto dto = new UserDto
        {
            Id = created.Id,
            UserName = created.UserName
        };
        return Created($"/users/{dto.Id}", dto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers([FromQuery] string? username)
    {
        var users = userRepo.GetMany();
        
        if (!string.IsNullOrEmpty(username))
        {
            users = users.Where(u => u.UserName.Contains(username, StringComparison.OrdinalIgnoreCase));
        }

        var userDtos = users.Select(u => new UserDto
        {
            Id = u.Id,
            UserName = u.UserName
        }).ToList();

        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            UserDto dto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
            return Ok(dto);
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            User user = await userRepo.GetSingleAsync(id);
            user.UserName = request.UserName;
            user.Password = request.Password;
            await userRepo.UpdateAsync(user);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        try
        {
            await userRepo.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }
    }

    private async Task VerifyUserNameIsAvailableAsync(string userName)
    {
        var existingUser = userRepo.GetMany().FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Username '{userName}' is already taken");
        }
    }
}