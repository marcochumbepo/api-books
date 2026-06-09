using FluentValidation;
using MsBooks.Application.DTOs;

namespace MsBooks.Application.Validators;

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es obligatorio.");

        RuleFor(x => x.Author)
            .NotEmpty().WithMessage("El autor es obligatorio.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("La descripción es obligatorio.");
    }
}
