using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.Abstract;
using Trello.Application.DTOs;
using Trello.Domain.Entities;

namespace Trello.WebUi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService, IMapper mapper) : Controller
{
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    public async Task<IActionResult> Register(UserDto user)
    {
        var newUser = Mapper.Map<User>(user);
        await _userService.RegsiterAsync(newUser);
        return Ok(newUser);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] int id)
    {
        var user = await _userService.GetByIdAsync(id);
        await _userService.Remove(id);
        return NoContent();
    }

    [HttpPost("[action]")]
    public Task<IActionResult> Update(UserDto user)
    {
        var newUser = _mapper.Map<User>(user);
        _userService.Update(newUser);
        return Task.FromResult<IActionResult>(NoContent());
    }

    [HttpGet("[action]")]
    public Task<IActionResult> GetAll()
    {
        _userService.GetAll();
        return Task.FromResult<IActionResult>(NoContent());
    }
}