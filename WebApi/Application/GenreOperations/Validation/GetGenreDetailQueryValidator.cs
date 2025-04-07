using FluentValidation;
using WebApi.Application.GenreOperations.Query;

namespace WebApi.GenreOperations.Validation
{
    public class GetGenreDetailQueryValidator : AbstractValidator<GetGenreDetailQuery>
    {
        public GetGenreDetailQueryValidator()
        {
            RuleFor(query => query.GenreId).GreaterThan(0);
        }
    }
}