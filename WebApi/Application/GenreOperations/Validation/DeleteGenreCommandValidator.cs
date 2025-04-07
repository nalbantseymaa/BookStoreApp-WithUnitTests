
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.GenreOperations.Validation
{
    public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
    {
        public DeleteGenreCommandValidator()
        {
            RuleFor(command => command.GenreId).GreaterThan(0);
        }
    }
}