using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.GenreOperations.Validation
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(command => command.Model.Name).MinimumLength(4).When(x => x.Model.Name.Trim() != string.Empty);
        }
    }
}