using Trello.Repository.DataAccess;
using Task = Trello.Domain.Entities.Task;

namespace Trello.DAL.SqlServer.Abstract;

public interface ITaskDal:IEntityRepository<Task>
{
    
}