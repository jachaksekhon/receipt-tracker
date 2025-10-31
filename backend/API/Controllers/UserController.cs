using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Application.Services.Users;
using ReceiptTracker.Domain.Models;
using System.Reflection.Metadata.Ecma335;

namespace ReceiptTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
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

        var userDto = _mapper.Map<UserReadDto>(user);

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

        var userDto = _mapper.Map<UserReadDto>(user);

        return Ok(userDto);
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
