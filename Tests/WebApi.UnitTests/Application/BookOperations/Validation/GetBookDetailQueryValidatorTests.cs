using FluentAssertions;
using TestSetup;
using WebApi.BookOperations.Query;
using WebApi.BookOperations.Validation;

namespace Application.BookOperations.Validation
{
    public class GetBookDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Fact]
        public void WhenInvalidBookIdIsGiven_Validator_ShouldBeReturnError()
        {
            GetBookDetailQuery command = new GetBookDetailQuery(null!);
            command.BookId = 0;

            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Validator_ShouldNotReturnError()
        {
            GetBookDetailQuery command = new GetBookDetailQuery(null!);
            command.BookId = 1;

            GetBookDetailQueryValidator validator = new GetBookDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }

    }
}
