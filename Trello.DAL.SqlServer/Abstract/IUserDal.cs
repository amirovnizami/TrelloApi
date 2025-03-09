using Trello.Domain.Entities;
using Trello.Repository.DataAccess;

namespace Trello.DAL.SqlServer.Abstract;

public interface IUserDal:IEntityRepository<User>
{
    Task<User> GetByIdAsync(int id);
}