using Microsoft.EntityFrameworkCore;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Entities;
using Trello.Repository.DataAccess.EntityFrameworkAccess;

namespace Trello.DAL.SqlServer.Implementation.EFEntityFramework;

public class EFUserDal: EfEntityRepositoryBase.EFEntityRepositoryBase<User,TrelloDbContext>,IUserDal
{
    private readonly TrelloDbContext _context;
    
    public EFUserDal(TrelloDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int? id)
    {
        var result = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        return result;
    
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return _context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email);
        
    }
}