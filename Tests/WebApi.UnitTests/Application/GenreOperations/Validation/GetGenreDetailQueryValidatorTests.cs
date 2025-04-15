using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Query;
using WebApi.GenreOperations.Validation;


namespace Application.GenreOperations.Validation
{
    public class GetGenreDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Fact]
        public void WhenInvalidIdIsGiven_Validator_ShouldBeReturnErrors()
        {
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = 0;
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();

            var result = validator.Validate(query);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidIdIsGiven_Validator_ShouldNotBeReturnErrors()
        {
            GetGenreDetailQuery query = new GetGenreDetailQuery(null, null);
            query.GenreId = 1;
            GetGenreDetailQueryValidator validator = new GetGenreDetailQueryValidator();

            var result = validator.Validate(query);

            result.Errors.Count.Should().Be(0);
        }

    }
}
