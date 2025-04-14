
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.GenreOperations.Validation
{
    public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
    {
        public DeleteAuthorCommandValidator()
        {
            RuleFor(command => command.AuthorId).GreaterThan(0);
        }
    }
}