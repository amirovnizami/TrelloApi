using Microsoft.AspNetCore.Mvc;
using Trello.Application.Abstract;
using Trello.Domain.Entities;
using Trello.WebUi.DTOs;
using Trello.WebUi.Models;

namespace Trello.WebUi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public IActionResult Register( UserDto user)
    {
        var newUser = _userService.RegsiterAsync(user);
        return Ok(newUser);
    }
}