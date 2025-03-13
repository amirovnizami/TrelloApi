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

public class UserService(
    IUserDal userDal,
    IConfiguration configuration,
    TrelloDbContext context,
    ITokenService tokenService) : IUserService
{
    private readonly IUserDal _userDal = userDal;

    public async Task<Result<User>> RegisterAsync(User user)
    {
        if (user.RoleId > 0 && user.RoleId <= 3)
        {
            userDal.Add(user);
            var response = new Result<User>() { Data = user, Errors = [], IsSuccess = true };
            return response;
        }

        return new Result<User>() { Data = null, Errors = ["Occured an error"], IsSuccess = false };
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

    public async Task<Result<User>> Update(int ?id,User user)
    {
        var exisitingUser  = await _userDal.GetByIdAsync(id);
        if (exisitingUser == null)
        {
            return new Result<User>() { Data = null, Errors = ["User not found"], IsSuccess = false };
        }
        _userDal.Update(user);
        return new Result<User>() { Data = null, Errors = [], IsSuccess = true };
    }

    public async Task<Result<User>> Remove(int id)
    {
        var currentUser = await _userDal.GetByIdAsync(id);
        if (currentUser == null)
        {
            return new Result<User>() { Data = null, Errors = ["User does not exists!"], IsSuccess = false };
        }

        _userDal.Delete(currentUser);
        return new Result<User>() { Data = currentUser, IsSuccess = true };
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