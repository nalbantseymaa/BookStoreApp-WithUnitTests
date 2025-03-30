using FluentValidation;
using WebApi.BookOperations.Query;

namespace WebApi.BookOperations.Validation
{
    public class GetBookDetailQueryValidator : AbstractValidator<GetBookDetailQuery>
    {
        public GetBookDetailQueryValidator()
        {
            RuleFor(query => query.BookId).GreaterThan(0);
        }
    }
}