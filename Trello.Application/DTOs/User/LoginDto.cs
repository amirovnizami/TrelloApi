using FluentValidation;

namespace Trello.Application.DTOs;

public class LoginDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
    }
}