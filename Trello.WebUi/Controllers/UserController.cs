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
    [Authorize(Roles = "1,2")]
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
    [Authorize(Roles = "1,2")]
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
    [Authorize(Roles = "1,2")]
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
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await userService.GetAll();
        var result = mapper.Map<IList<UserDto>>(users);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "2")]
    public async Task<IActionResult> AssingTask([FromQuery] int TaskId, [FromQuery] int AssignedToId)
    {
        var user = await userService.GetByIdAsync(AssignedToId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var task = await taskService.GetByIdAsync(TaskId);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        task.AssigneeId = user.Id;

        await taskService.UpdateAsync(task);
        return Ok("Task successfully assigned.");
    }

    [HttpPost]
    [Authorize(Roles = "3")]
    public async Task<ActionResult> GetMyTasks()
    {
        var userId = userContext.MustGetUserId();
        var tasks = await taskService.GetAllAsync(t => t.AssigneeId == userId);
        var result = mapper.Map<IList<TaskDto>>(tasks);

        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = "3")]

    public async Task<ActionResult> UpdateTaskStatus([FromQuery] int TaskId, [FromQuery] int StatusId)
    {
        // var userId = userContext.MustGetUserId();
        var task = await taskService.GetByIdAsync(TaskId);
        if (task != null)
        {
            task.Status = (Status)(StatusId);
            await taskService.UpdateAsync(task);
            return Ok("Status successfully updated.");
        }
        return NotFound("Task not found");
    }

    [HttpGet]
    [Authorize(Roles = "2")]
    public async Task<ActionResult<TaskStatisticsDto>> GetTaskStatistics()
    {
        var statistics = await taskService.GetTaskStatisticsAsync();
        var dto = mapper.Map<TaskStatisticsDto>(statistics);
        return Ok(statistics);
    }

    [HttpPut]
    [Authorize(Roles = "3")]
    public async Task<ActionResult<TaskStatisticsDto>> ChangePassword([FromQuery] int UserId,
        [FromQuery] string newPassword)
    {
        var user = await userService.GetByIdAsync(UserId);
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
        user.PasswordHash = hashedPassword;
        userService.Update(null,user);
        return Ok("Password changed successfully");
    }
}