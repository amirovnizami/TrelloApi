using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Abstract;
using Trello.Application.DTOs;
using Trello.Application.DTOs.Task;
using Trello.Application.Security;
using Trello.Common.Security;
using Trello.Domain.Entities;
using Trello.Domain.Enums;

namespace Trello.WebUi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController(
    IUserService userService,
    ITaskService taskService,
    IMapper mapper,
    IUserContext userContext) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        registerDto.Password = hashedPassword;

        var newUser = mapper.Map<User>(registerDto);
        newUser.PasswordHash = hashedPassword;

        var result = await userService.RegisterAsync(newUser);
        if (result.IsSuccess)
        {
            var user = mapper.Map<UserDto>(newUser);
            return Ok(user);
        }

        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await userService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var result = await userService.Remove(id);
        if (result.IsSuccess)
        {
            return Ok("User deleted");
        }

        return BadRequest(result);
    }

    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update([FromQuery] int id, UserDto userDto)
    {
        var user = await userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var validator = new UserDtoValidator();
        var results = validator.Validate(userDto);

        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return BadRequest(ModelState);
        }

        user.Username = userDto.Username;
        user.Email = userDto.Email;
        try
        {
            var result = await userService.Update(id, user);
            if (result.IsSuccess)
            {
                return Ok("User updated");
            }
            else
            {
                return BadRequest(result);
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Data has changed, please refresh and try again.");
        }
    }

    [HttpGet]
    [Authorize("Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll();
        var result = mapper.Map<IList<UserDto>>(users);
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "Assignee")]
    public async Task<ActionResult<TaskStatisticsDto>> ChangePassword([FromQuery] int UserId,
        [FromQuery] string newPassword)
    {
        var user = await userService.GetByIdAsync(UserId);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordHash = hashedPassword;
        await userService.Update(null, user);
        return Ok("Password changed successfully");
    }
}