using System;
using FluentValidation;
using WebApi.Application.GenreOperations.Command;

namespace WebApi.AuthorOperations.Validation
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator()
        {
            RuleFor(x => x.Model.Name)
             .NotEmpty().WithMessage("İsim boş olamaz.")
             .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("İsim sadece boşluk olamaz.")
             .Length(2, 100).WithMessage("İsim 2 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.Model.Surname)
                .NotEmpty().WithMessage("Soyisim boş olamaz.")
                .Must(name => !string.IsNullOrWhiteSpace(name)).WithMessage("Soyisim sadece boşluk olamaz.")
                .Length(2, 100).WithMessage("Soyisim 2 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.Model.Birthday)
                .LessThan(DateTime.Today).WithMessage("Doğum tarihi geçmişte olmalıdır.")
                .When(x => x.Model.Birthday.HasValue);
        }
    }
}