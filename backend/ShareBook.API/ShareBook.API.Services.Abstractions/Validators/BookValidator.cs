using System;
using FluentValidation;
using ShareBook.API.Contracts;

namespace ShareBook.API.Services.Abstractions.Validators;

public class BookValidator : AbstractValidator<BookDto>
{
    public BookValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty()
            .WithMessage("Book title is required.")
            .MaximumLength(200)
            .WithMessage("Book title must not exceed 200 characters.");

        RuleFor(b => b.Publisher)
            .MaximumLength(100)
            .WithMessage("Publisher name must not exceed 100 characters.");

        RuleFor(b => b.DatePublished)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publication date cannot be in the future.");

        RuleFor(b => b.Isbn13)
            .Matches(@"^(978|979)\d{10}$")
            .WithMessage("ISBN must be a valid 13-digit number starting with 978 or 979.");

        RuleFor(b => b.Isbn10)
            .Matches(@"^\d{10}$")
            .WithMessage("ISBN must be a valid 10-digit number.");
    }
}
