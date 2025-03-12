using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Trello.Application.Abstract;
using Trello.Application.DTOs;
using Trello.Application.Services;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Trello.Application.Concrete;

public class UserService(IUserDal userDal, IConfiguration configuration, TrelloDbContext context,ITokenService tokenService) : IUserService
{
    private readonly IUserDal _userDal = userDal;

    public async Task RegisterAsync(User user)
    {
        userDal.Add(user);
    }

    public async Task<IActionResult> LoginAsync(LoginDto loginDto)
    {
        var user = await _userDal.GetByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new BadRequestObjectResult("Invalid email");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            return new BadRequestObjectResult("Invalid password");
        }

        var token = tokenService.GenerateJWT(user);
        return new OkObjectResult(token);
    }

    public async void Update(User user)
    {
        _userDal.Update(user);
    }

    public async Task Remove(int id)
    {
        var currentUser = await _userDal.GetByIdAsync(id);
        _userDal.Delete(currentUser);
    }

    public Task<List<User>> GetAll()
    {
        var users = _userDal.GetListAsync();
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _userDal.GetByIdAsync(id);
    }
}