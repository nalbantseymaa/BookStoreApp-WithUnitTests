using FluentAssertions;
using TestSetup;
using WebApi.BookOperations.Command;
using WebApi.BookOperations.Validation;
using WebApi.DBOperations;

namespace Application.BookOperations.Validation
{
    public class GetBookDetailQueryTestsValidator : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public GetBookDetailQueryTestsValidator(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidBookIdIsGiven_Validator_ShouldBeReturnError()
        {
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = 0;

            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidBookIdIsGiven_Validator_ShouldNotReturnError()
        {
            DeleteBookCommand command = new DeleteBookCommand(_context);
            command.BookId = 1;

            DeleteBookCommandValidator validator = new DeleteBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
