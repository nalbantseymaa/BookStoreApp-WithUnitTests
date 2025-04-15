using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Command;
using WebApi.GenreOperations.Validation;

namespace Application.GenreOperations.Validation
{
    public class DeleteGenreCommandTestsValidator : IClassFixture<CommonTestFixture>
    {

        [Fact]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors()
        {
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = 0; // GenreId hatalı

            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
        {
            DeleteGenreCommand command = new DeleteGenreCommand(null);
            command.GenreId = 1; // GenreId geçerli

            DeleteGenreCommandValidator validator = new DeleteGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
