using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.GenreOperations.Validation
{
    public class UpdateGenreCommandValidator : AbstractValidator<UpdateGenreCommand>
    {
        public UpdateGenreCommandValidator()
        {
            RuleFor(command => command.GenreId).GreaterThan(0).WithMessage("Kitap Türü Id'si 0'dan büyük olmalıdır.");
            RuleFor(command => command.Model.Name)
             .NotEmpty().WithMessage("Genre adı boş olamaz.")
             .Must(name => !string.IsNullOrWhiteSpace(name))
                 .WithMessage("Genre adı sadece boşluklardan oluşamaz.")
             .MinimumLength(4)
                 .WithMessage("Genre adı en az 4 karakter olmalıdır.");



        }
    }
}