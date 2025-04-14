using System;
using FluentValidation;
using WebApi.BookOperations.Command;

namespace WebApi.BookOperations.Validation
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(command => command.BookId)
            .GreaterThan(0).WithMessage("Geçersiz kitap ID'si.");

            RuleFor(command => command.Model.Title)
                .NotEmpty().WithMessage("Kitap adı boş olamaz.")
                .MinimumLength(4).WithMessage("Kitap adı en az 4 karakter olmalıdır.");

            RuleFor(command => command.Model.GenreId)
                .GreaterThan(0).WithMessage("Geçersiz tür ID'si.");


            RuleFor(command => command.Model.PageCount)
                .GreaterThan(0).WithMessage("Sayfa sayısı 0'dan büyük olmalıdır.")
                .When(x => x.Model.PageCount.HasValue);


            RuleFor(command => command.Model.PublishDate)
                .NotNull().WithMessage("Yayın tarihi girilmelidir.")
                .LessThan(DateTime.Today).WithMessage("Yayın tarihi bugünden önce olmalıdır.")
                .When(x => x.Model.PublishDate.HasValue);


            RuleFor(command => command.Model.AuthorId)
                .GreaterThan(0).WithMessage("Geçersiz yazar ID'si.")
                .When(x => x.Model.AuthorId.HasValue);


        }
    }
}