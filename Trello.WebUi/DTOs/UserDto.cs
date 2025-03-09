using FluentValidation;

namespace Trello.WebUi.DTOs;

public class UserDto
{
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Password { get; set; }

    public int RoleId { get; set; }

}

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Full Name cannot be empty.")
            .MinimumLength(3).WithMessage("Full Name must be at least 3 characters.")
            .Matches(@"^[a-zA-Z]+$").WithMessage("You can only enter string values.");
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        RuleFor(x=>x.RoleId).GreaterThan(0).WithMessage("Invalid role ID.");
    }

}