using Trello.Application.Abstract;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Entities;
using Trello.Repository.Common.Exception;

namespace Trello.Application.Concrete;

public class UserService(IUserDal userDal,TrelloDbContext context):IUserService
{
    private readonly IUserDal _userDal = userDal;
    public async Task RegsiterAsync(User user)
    {
        _userDal.Add(user);
    }

    public async void Update(User user)
    {
        _userDal.Update(user);
    }
    public void Remove(User user)
    {
        _userDal.Delete(user);
    }

    public List<User> GetAll()
    {
        var users = _userDal.GetList();
        return users;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return (context.Users.FirstOrDefault(u => u.Id == id));
    }
}