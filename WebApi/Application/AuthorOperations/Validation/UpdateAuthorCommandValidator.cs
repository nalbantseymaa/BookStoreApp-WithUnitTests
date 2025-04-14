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
           .Length(1, 100).WithMessage("İsim 1 ile 50 karakter arasında olmalıdır.")
           .When(x => !string.IsNullOrWhiteSpace(x.Model.Name));

            RuleFor(x => x.Model.Surname)
                .Length(1, 100).WithMessage("Soyisim 1 ile 50 karakter arasında olmalıdır.")
                .When(x => !string.IsNullOrWhiteSpace(x.Model.Surname));

            RuleFor(x => x.Model.Birthday)
                .LessThan(DateTime.Today).WithMessage("Doğum tarihi geçmişte olmalıdır.")
                .When(x => x.Model.Birthday.HasValue);
        }
    }
}