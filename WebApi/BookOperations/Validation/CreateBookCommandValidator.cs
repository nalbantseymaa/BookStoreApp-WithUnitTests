using System;
using FluentValidation;
using WebApi.BookOperations.Command;


namespace WebApi.BookOperations.Validation
{
    // Validates CreateBookCommand objects
    public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        // Constructor to define validation rules
        public CreateBookCommandValidator()
        {
            RuleFor(command => command.Model.GenreId).GreaterThan(0);
            RuleFor(command => command.Model.PageCount).GreaterThan(0);
            RuleFor(command => command.Model.PublishDate.Date).NotEmpty().LessThan(DateTime.Now.Date);
            RuleFor(command => command.Model.Title).NotEmpty().MinimumLength(4);

        }
    }
}