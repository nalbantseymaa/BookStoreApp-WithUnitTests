using FluentValidation;
using WebApi.BookOperations.Command;

namespace WebApi.BookOperations.Validation
{
    public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookCommandValidator()
        {
            RuleFor(command => command.BookId).GreaterThan(0);
        }
    }

}