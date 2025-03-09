using Microsoft.EntityFrameworkCore;
using Trello.Application.Abstract;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Entities;
using Trello.Repository.Common.Exception;

namespace Trello.Application.Concrete;

public class UserService(IUserDal userDal, TrelloDbContext context) : IUserService
{
    private readonly IUserDal _userDal = userDal;

    public async Task RegsiterAsync(User user)
    {
        userDal.Add(user);
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

    public List<User> GetAll()
    {
        var users = _userDal.GetList();
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await (context.Users.FirstOrDefaultAsync(u => u.Id == id)!);
    }
}