using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Trello.Application.Abstract;
using Trello.Application.DTOs;
using Trello.Application.DTOs.Task;
using Trello.DAL.SqlServer.Abstract;
using Trello.DAL.SqlServer.Context;
using Trello.Domain.Enums;

namespace Trello.Application.Concrete;

public class TaskService(ITaskDal taskDal, TrelloDbContext context) : ITaskService
{
    private readonly TrelloDbContext _context = context;
    private readonly ITaskDal _taskDal = taskDal;

    public async Task CreateAsync(Domain.Entities.Task task, int currentUserId)
    {
        task.CreatorId = currentUserId;
        task.Status = Status.New;
        _taskDal.Add(task);
    }


    public async Task UpdateAsync(Domain.Entities.Task task)
    {
        _taskDal.Update(task);
    }

    public async Task DeleteAsync(int TaskId)
    {
        var task = _taskDal.Get(t => t.Id == TaskId);
        _taskDal.Delete(task);
    }


    public async Task<List<Domain.Entities.Task>> GetAllAsync(
        Expression<Func<Domain.Entities.Task, bool>> filter = null)
    {
        var tasks = _taskDal.GetListAsync(filter);
        return await await Task.FromResult(tasks);
    }

    public async Task<Domain.Entities.Task?> GetByIdAsync(int id)
    {
        var exisitingTask = await context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
        return exisitingTask;
    }

    public async Task<TaskStatisticsDto> GetTaskStatisticsAsync()
    {
        var statistics = new TaskStatisticsDto();

        statistics.TotalTasks = await _context.Tasks.CountAsync();
        statistics.NewTasks = await _context.Tasks.CountAsync(t => t.Status == Status.New);
        statistics.InProgressTasks = await _context.Tasks.CountAsync(t => t.Status == Status.InProgress);
        statistics.CompletedTasks = await _context.Tasks.CountAsync(t => t.Status == Status.Completed);
        statistics.CancelledTasks = await _context.Tasks.CountAsync(t => t.Status == Status.Canceled);

        statistics.LowPriorityTasks = await _context.Tasks.CountAsync(t => t.Priority == Priority.Low);
        statistics.MediumPriorityTasks = await _context.Tasks.CountAsync(t => t.Priority == Priority.Medium);
        statistics.HighPriorityTasks = await _context.Tasks.CountAsync(t => t.Priority == Priority.High);

        statistics.TasksPerAssignee = await _context.Tasks
            .GroupBy(t => t.AssigneeId)
            .Select(g => new TaskStatisticsDto.AssigneeTaskCountDto { AssigneeId = g.Key, TaskCount = g.Count() })
            .ToListAsync();

        return statistics;
    }

    public async Task<List<Domain.Entities.Task>> SortByDate()
    {
        return _context.Tasks.ToList().OrderBy(t => t.StartDate).ToList();
    }


    public async Task<List<Domain.Entities.Task>> SortByPriority(int priorityId)
    {
        return await _context.Tasks
            .Where(t => t.Priority == (Priority)priorityId)
            .OrderBy(t => t.Priority)
            .ToListAsync();
    }
    
}