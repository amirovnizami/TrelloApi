using Trello.Domain.Enums;

namespace Trello.Application.DTOs.Task;

public class TaskUpdateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime EndDate { get; set; }
    public int? AssigneeId { get; set; }
}