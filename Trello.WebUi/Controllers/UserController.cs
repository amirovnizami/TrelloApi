using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Abstract;
using Trello.Application.DTOs;
using Trello.Application.Security;
using Trello.Common.Security;
using Trello.Domain.Entities;
using Trello.Domain.Enums;

namespace Trello.WebUi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class UserController(IUserService userService,ITaskService taskService,IMapper  mapper,IUserContext userContext) : ControllerBase
{
   
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var validator = new RegisterDtoValidator();
        var results = validator.Validate(registerDto);
        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(ModelState);
        }
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
        registerDto.Password = hashedPassword;

        var newUser = mapper.Map<User>(registerDto);
        newUser.PasswordHash = hashedPassword;  
        
        await userService.RegisterAsync(newUser);
        var result = mapper.Map<UserDto>(newUser);
        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var validator = new LoginDtoValidator();
        var results = validator.Validate(loginDto);
        if (!results.IsValid)
        {
            foreach (var error in results.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return BadRequest(ModelState);
        }
        var result = await userService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] int id)
    {
        var user = await userService.GetByIdAsync(id);
        await userService.Remove(id);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> Update( [FromQuery] int id ,UserDto userDto)
    {
        var user = await userService.GetByIdAsync(id);
        if (user ==null)
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
            userService.Update(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Data has changed, please refresh and try again.");
        }

        return Ok("User updated successfully");
    }

    [HttpGet]
    [Authorize]

    public async Task<IActionResult> GetAll()
    {
        var users =  userService.GetAll();
        var result = mapper.Map<IList<UserDto>>(users);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> AssingTask([FromQuery] int TaskId, [FromQuery] int AssignedToId)
    {
        var user = await userService.GetByIdAsync(AssignedToId);
        if (user == null)
        {
            return NotFound("User not found");
        }
        var task  =  await taskService.GetByIdAsync(TaskId);
        if (task == null)
        {
            return NotFound("Task not found");
        }
        task.AssigneeId = user.Id;

        await taskService.UpdateAsync(task); 
        return Ok("Task successfully assigned.");
        
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> GetMyTasks()
    {
        var userId = userContext.MustGetUserId(); 
        var tasks = await taskService.GetAllAsync(t => t.AssigneeId == userId);
        return Ok(tasks);
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateTaskStatus([FromQuery] int TaskId, [FromQuery] int StatusId)
    {
        var userId = userContext.MustGetUserId(); 
        var task = await taskService.GetByIdAsync(TaskId);
        if (task != null)
        {
            task.Status = (Status)(StatusId);
            await taskService.UpdateAsync(task);
        }
        return Ok("Status successfully updated.");
    }
}