using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trello.Application.Abstract;
using Trello.Application.DTOs.Task;
using Trello.Application.Security;
using Trello.Domain.Enums;
using Trello.WebUi.Security;
using Task = Trello.Domain.Entities.Task;

namespace Trello.WebUi.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class TaskController(IMapper mapper, ITaskService taskService,IUserContext userContext,IUserService userService) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;
    private readonly IMapper _mapper = mapper;
    private readonly IUserContext _userContext = userContext;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskCreateDto)
    {
        var currentUser = _userContext.MustGetUserId();
        var task = _mapper.Map<Task>(taskCreateDto);
        var result = _taskService.CreateAsync(task, currentUser);
        return Ok();
    }

    [HttpPut]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> UpdateTask( [FromQuery] int id,[FromBody] TaskUpdateDto taskUpdateDto)
    {
        var exisitingTask = await _taskService.GetByIdAsync(id);
        if (exisitingTask == null)
        {
            return NotFound("Task not found");
        }
        var task = _mapper.Map<Task>(taskUpdateDto);
        await _taskService.UpdateAsync(task);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> DeleteTask(int id)
    {
         var task = await _taskService.GetByIdAsync(id);
         if (task == null)
         {
             return NotFound("Task not found");
         }
         await _taskService.DeleteAsync(id);
         return Ok();
    }
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> AssingTask([FromQuery] int TaskId, [FromQuery] int AssignedToId)
    {
        var user = await userService.GetByIdAsync(AssignedToId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        var task = await taskService.GetByIdAsync(TaskId);
        if (task == null)
        {
            return NotFound("Task not found");
        }

        task.AssigneeId = user.Id;

        await taskService.UpdateAsync(task);
        return Ok("Task successfully assigned.");
    }

    [HttpPost]
    [Authorize(Roles = "Assignee")]
    public async Task<ActionResult> GetMyTasks()
    {
        var userId = userContext.MustGetUserId();
        var tasks = await taskService.GetAllAsync(t => t.AssigneeId == userId);
        var result = mapper.Map<IList<TaskDto>>(tasks);

        return Ok(result);
    }
    [HttpGet]
    [Authorize(Roles = "Assignee")]
    public async Task<ActionResult> GetTasksByPriority([FromQuery] int priorityId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.Priority == (Priority)priorityId;
        var tasks =await _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }
    [HttpPut]
    [Authorize(Roles = "Assignee")]
    public async Task<ActionResult> UpdateTaskStatus([FromQuery] int TaskId, [FromQuery] int StatusId)
    {
        // var userId = userContext.MustGetUserId();
        var task = await taskService.GetByIdAsync(TaskId);
        if (task != null)
        {
            task.Status = (Status)(StatusId);
            await taskService.UpdateAsync(task);
            return Ok("Status successfully updated.");
        }

        return NotFound("Task not found");
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<TaskStatisticsDto>> GetTaskStatistics()
    {
        var statistics = await taskService.GetTaskStatisticsAsync();
        var dto = mapper.Map<TaskStatisticsDto>(statistics);
        return Ok(statistics);
    }
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> GetTasksByStatus([FromQuery] int statusId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.Status == (Status)statusId;
        var tasks =await  _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }
    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> GetTasksByAssignee([FromQuery] int assigneeId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.AssigneeId == assigneeId;
        var tasks = await _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }

    [HttpGet]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult> SortTask([FromQuery] string param)
    {
        if (DateTime.TryParse(param, out DateTime date))
        {
            var result = _taskService.SortTask(date);
            return Ok(result);
        }
        else if (Enum.TryParse<Priority>(param, out Priority priority))
        {
            var result = _taskService.SortTask(priority);
            return Ok(result);
        }
        else
        {
            return BadRequest("Invalid parameter. Use a valid DateTime or Priority value.");
        }
    }
    
}