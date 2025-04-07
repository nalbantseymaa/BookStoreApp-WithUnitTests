
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.GenreOperations.Validation
{
    public class CreateGenreCommandValidator : AbstractValidator<CreateGenreCommand>
    {
        public CreateGenreCommandValidator()
        {
            RuleFor(command => command.Model.Name)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(30)
                .WithMessage("Genre name must be between 4 and 30 characters long.");
        }
    }
}