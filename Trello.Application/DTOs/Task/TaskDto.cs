using FluentValidation;
using Trello.Domain.Enums;

namespace Trello.Application.DTOs.Task;

public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public Status Status { get; set; }

    public Priority Priority { get; set; }
    
    public int CreatorId { get; set; }

    public int AssigneeId { get; set; }


}

public class TaskDtoValidator : AbstractValidator<TaskDto>
{
    // public TaskDtoValidator()
    // {
    //     RuleFor(x => x.Title)
    //         .NotEmpty().WithMessage("Title is required.")
    //         .MaximumLength(100).WithMessage("Title must be at most 100 characters long.");
    //
    //     RuleFor(x => x.Description)
    //         .MaximumLength(500).WithMessage("Description must be at most 500 characters long.");
    //
    //     RuleFor(x => x.Priority)
    //         .IsInEnum().WithMessage("Invalid priority value.");
    //
    //     RuleFor(x => x.AssigneeId)
    //         
    //         .GreaterThan(0).WithMessage("Invalid Assignee ID.")
    //         .Must(AssigneeExists).WithMessage("Assignee ID does not exist in Users table.");
    // }
}