namespace Application.AuthorOperations.Validation
{
    using FluentAssertions;
    using TestSetup;
    using WebApi.BookOperations.Query;
    using WebApi.BookOperations.Validation;

    public class GetAuthorDetailQueryValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Fact]
        public void WhenInvalidAuthorIdIsGiven_Validator_ShouldBeReturnError()
        {
            GetAuthorDetailQuery command = new GetAuthorDetailQuery(null!, null!);
            command.AuthorId = 0;

            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validator_ShouldNotReturnError()
        {
            GetAuthorDetailQuery command = new GetAuthorDetailQuery(null!, null!);
            command.AuthorId = 1;

            GetAuthorDetailQueryValidator validator = new GetAuthorDetailQueryValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
