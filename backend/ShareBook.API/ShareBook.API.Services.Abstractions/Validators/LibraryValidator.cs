using System;
using FluentValidation;
using ShareBook.API.Contracts;

namespace ShareBook.API.Services.Abstractions.Validators;

public class LibraryValidator : AbstractValidator<LibraryDto>
{
    public LibraryValidator()
    {
        RuleFor(l => l.Name)
            .NotEmpty()
            .WithMessage("Library name is required.")
            .MaximumLength(100)
            .WithMessage("Library name must not exceed 100 characters.");

        RuleFor(l => l.Address)
            .MaximumLength(200)
            .WithMessage("Library address must not exceed 200 characters.");
    }
}
