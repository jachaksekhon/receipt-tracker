using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.DTOs.Users;
using ReceiptTracker.Models;
using ReceiptTracker.Services.Users;
using System.Reflection.Metadata.Ecma335;

namespace ReceiptTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<User>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email cannot be empty.");

        var user = await _userService.FindByEmailAsync(email);

        if (user == null)
            return NotFound($"No user found with the email '{email}'");

        var userDto = new UserReadDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = email,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        if (id < 0)
            return BadRequest("Id must be a positive integer.");

        var user = await _userService.FindByIdAsync(id);

        if (user == null)
            return NotFound($"No user found with the id '{id}'");

        var userDto = new UserReadDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] UserCreateDto dto)
    {
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName  = dto.LastName,
            Email     = dto.Email,
            Password  = dto.Password,
        };

        var createdUser = await _userService.CreateUserAsync(user);

        var readDto = new UserReadDto
        {
            Id        = createdUser.Id,
            FirstName = createdUser.FirstName,
            LastName  = createdUser.LastName,
            Email     = createdUser.Email,
            CreatedAt = createdUser.CreatedAt
        };

        return Ok(new { id = createdUser.Id });

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        if (id < 0)
            return BadRequest("Id must be a positive integer.");

        var user = await _userService.FindByIdAsync(id);

        if (user == null)
            return BadRequest($"User with id {id} does not exist in the database");

        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, "Failed to delete user.");

        return NoContent(); 
    }
}
