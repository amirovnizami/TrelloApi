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
public class TaskController(IMapper mapper, ITaskService taskService,IUserContext userContext) : ControllerBase
{
    private readonly ITaskService _taskService = taskService;
    private readonly IMapper _mapper = mapper;
    private readonly IUserContext _userContext = userContext;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto taskCreateDto)
    {
        var currentUser = _userContext.MustGetUserId();
        var task = _mapper.Map<Task>(taskCreateDto);
        var result = _taskService.CreateAsync(task, currentUser);
        return Ok();
    }

    [HttpPut]
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
    public async Task<ActionResult> GetTasksByPriority([FromQuery] int priorityId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.Priority == (Priority)priorityId;
        var tasks =await _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }
    [HttpGet]
    public async Task<ActionResult> GetTasksByStatus([FromQuery] int statusId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.Status == (Status)statusId;
        var tasks =await  _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }
    [HttpGet]
    public async Task<ActionResult> GetTasksByAssigne([FromQuery] int assigneeId)
    {
        Expression<Func<Task, bool>> filter = task =>  task.AssigneeId == assigneeId;
        var tasks = await _taskService.GetAllAsync(filter);
        return Ok(tasks);
    }

    [HttpGet]
    public async Task<ActionResult> SortTaskByDate()
    {
        var resutlt = await _taskService.SortByDate();
        return Ok(resutlt);
    }
    [HttpGet]
    public async Task<ActionResult> SortTaskByPriority()
    {
        var result = await _taskService.SortByDate();
        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult> SortByPriority(int PriorityId)
    {
        var result = await _taskService.SortByPriority(PriorityId);
        return Ok(result);
    }
    
}