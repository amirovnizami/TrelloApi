using Microsoft.AspNetCore.Mvc;
using Trello.Application.DTOs;
using Trello.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Trello.Application.Abstract;

public interface IUserService
{
    Task RegisterAsync(User user);
    Task<IActionResult> LoginAsync(LoginDto loginDto);
    void Update(User user);
    Task Remove(int id);
    Task<List<User>> GetAll();
    Task<User> GetByIdAsync(int id);
}