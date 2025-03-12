namespace Trello.Application.DTOs.Task;

public class TaskStatisticsDto
{
    public int TotalTasks { get; set; }
    public int NewTasks { get; set; }
    public int InProgressTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int CancelledTasks { get; set; }

    public int LowPriorityTasks { get; set; }
    public int MediumPriorityTasks { get; set; }
    public int HighPriorityTasks { get; set; }

    public List<AssigneeTaskCountDto> TasksPerAssignee { get; set; }


    public class AssigneeTaskCountDto
    {
        public int? AssigneeId { get; set; }
        public int TaskCount { get; set; }
    }
}