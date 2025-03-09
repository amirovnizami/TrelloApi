using Microsoft.EntityFrameworkCore;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Entities;
using Trello.Repository.DataAccess.EntityFrameworkAccess;

namespace Trello.DAL.SqlServer.Implementation.EFEntityFramework;

public class EFUserDal: EfEntityRepositoryBase.EFEntityRepositoryBase<User,TrelloDbContext>
{
    async Task<User> GetByIdAsync(int id)
    {
        var context = new TrelloDbContext();
        
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}