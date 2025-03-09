using Trello.Domain.Entities;

namespace Trello.Application.Abstract;

public interface IUserService
{
    Task RegsiterAsync(User user);
    void Update(User user);
    void Remove(User user);
    List<User> GetAll();
    Task<User> GetByIdAsync(int id);
}