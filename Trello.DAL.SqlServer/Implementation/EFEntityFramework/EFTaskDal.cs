using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Repository.DataAccess.EntityFrameworkAccess;
using Task = Trello.Domain.Entities.Task;

namespace Trello.DAL.SqlServer.Implementation.EFEntityFramework;

public class EFTaskDal: EfEntityRepositoryBase.EFEntityRepositoryBase<Task,TrelloDbContext>,ITaskDal
{
    
}