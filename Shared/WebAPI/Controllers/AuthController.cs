using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public AuthController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginRequest request)
    {
        User? user = userRepo.GetMany()
            .FirstOrDefault(u => u.UserName.Equals(request.UserName, StringComparison.OrdinalIgnoreCase));

        if (user == null)
        {
            return Unauthorized("Check da username or password idk");
        }

        if (!user.Password.Equals(request.Password))
        {
            return Unauthorized("Check da username or password idk");
        }

        UserDto dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };

        return Ok(dto);
    }
}