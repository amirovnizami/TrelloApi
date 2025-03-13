using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs;
using Trello.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Trello.Application.Abstract;

public interface IUserService
{
    Task<Result<User>> RegisterAsync(User user);
    Task<IActionResult> LoginAsync(LoginDto loginDto);
    Task<Result<User>> Update(int ?id,User user);
    Task<Result<User>> Remove(int id);
    Task<List<User>> GetAll();
    Task<User> GetByIdAsync(int id);
}