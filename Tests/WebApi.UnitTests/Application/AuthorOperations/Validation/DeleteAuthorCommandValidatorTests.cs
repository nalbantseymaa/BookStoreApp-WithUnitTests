using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Command;
using WebApi.DBOperations;
using WebApi.GenreOperations.Validation;


namespace Application.AuthorOperations.Validation
{
    public class GetAuthorDetailQueryTestsValidator : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public GetAuthorDetailQueryTestsValidator(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidAuthorIdIsGiven_Validator_ShouldBeReturnError()
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = 0;

            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Validator_ShouldNotReturnError()
        {
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = 1;

            DeleteAuthorCommandValidator validator = new DeleteAuthorCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
