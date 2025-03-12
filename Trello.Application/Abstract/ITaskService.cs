using System.Linq.Expressions;
using Trello.Application.DTOs;
using Trello.Application.DTOs.Task;

namespace Trello.Application.Abstract;

public interface ITaskService
{
    Task CreateAsync(Domain.Entities.Task taskDto,int currentUserId);
    Task UpdateAsync(Domain.Entities.Task task);
    Task DeleteAsync(int TaskId);
    Task<List<Domain.Entities.Task>> GetAllAsync(Expression<Func<Domain.Entities.Task, bool>> predicate);
    public Task<Domain.Entities.Task?> GetByIdAsync(int id);
    Task<TaskStatisticsDto> GetTaskStatisticsAsync();
}