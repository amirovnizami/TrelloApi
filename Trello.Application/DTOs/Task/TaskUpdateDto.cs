using FluentValidation;
using Trello.Domain.Enums;

namespace Trello.Application.DTOs.Task;

public class TaskUpdateDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime EndDate { get; set; }
    public int AssigneeId { get; set; }
}

public class TaskUpdateDtoValidator : AbstractValidator<TaskUpdateDto>
{
    public TaskUpdateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters long.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must be at most 500 characters long.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("Invalid priority value.");
        
        RuleFor(x => x.AssigneeId)
            .GreaterThan(0).WithMessage("Invalid Assignee ID.");
    }
}