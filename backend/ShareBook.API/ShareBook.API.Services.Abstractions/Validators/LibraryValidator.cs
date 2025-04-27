using FluentValidation;
using ShareBook.API.Contracts;

namespace ShareBook.API.Services.Abstractions.Validators;

public class LibraryValidator : AbstractValidator<LibraryDto>
{
    public LibraryValidator()
    {
        RuleFor(l => l.Id)
            .NotEmpty()
            .WithMessage("Library ID is required.")
            .MaximumLength(16)
            .Matches(@"^[a-zA-Z0-9\-]+$")
            .WithMessage("Library ID must not exceed 16 characters.");

        RuleFor(l => l.Name)
            .NotEmpty()
            .WithMessage("Library name is required.")
            .MaximumLength(512)
            .WithMessage("Library name must not exceed 512 characters.");

        RuleFor(l => l.Address)
            .MaximumLength(256)
            .WithMessage("Library address must not exceed 256 characters.");
    }
}
