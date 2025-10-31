using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ReceiptTracker.Application.Constants;
using ReceiptTracker.Application.DTOs.Users;
using ReceiptTracker.Application.Helpers;
using ReceiptTracker.Application.Services.Users;
using ReceiptTracker.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace ReceiptTracker.API.Controllers;

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
    public async Task<ActionResult<IReadOnlyList<UserReadDto>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("by-email")]
    public async Task<ActionResult<UserReadDto>> GetUserByEmail(string email)
    {
        try
        {
            email = InputSanitizer.NormalizeEmail(email);

            var userDto = await _userService.FindByEmailAsync(email);

            if (userDto == null)
                return NotFound(ErrorMessages.UserNotFoundByEmail(email));

            return Ok(userDto);
        }
        catch (Exception ex)

        {   // UPDATE WITH LOGGING IN FUTURE
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserReadDto>> GetUserById(int id)
    {
        if (id < 0)
            return BadRequest(ErrorMessages.InvalidId);

        var userDto = await _userService.FindByIdAsync(id);

        if (userDto == null)
            return NotFound(ErrorMessages.UserNotFoundbyId(id));

        return Ok(userDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUserAsync(int id)
    {
        if (id < 0)
            return BadRequest(ErrorMessages.InvalidId);

        var user = await _userService.FindByIdAsync(id);

        if (user == null)
            return BadRequest(ErrorMessages.UserNotFoundbyId(id));

        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, ErrorMessages.FailedToDeleteUser);

        return NoContent(); 
    }
}
