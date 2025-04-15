using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Command;
using WebApi.GenreOperations.Validation;

namespace Application.GenreOperations.Validation
{
    public class UpdateGenreCommandTestsValidator : IClassFixture<CommonTestFixture>
    {

        [Theory]
        [InlineData(0, "ValidName")]     // GenreId geçersiz
        [InlineData(-1, "ValidName")]    // GenreId geçersiz
        [InlineData(1, "a")]             // Name geçersiz
        [InlineData(1, "abc")]           // Name geçersiz
        [InlineData(1, "   ")]           // Name geçersiz (boşluk)
        [InlineData(0, "ab")]            // Her ikisi de geçersiz
        [InlineData(-5, "")]             // Her ikisi de geçersiz
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(int genreId, string name)
        {
            // Arrange
            var command = new UpdateGenreCommand(null!);
            command.GenreId = genreId;
            command.Model = new UpdateGenreModel { Name = name };

            // Act
            var validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }



        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
        {
            var command = new UpdateGenreCommand(null!);
            command.GenreId = 1;
            command.Model = new UpdateGenreModel { Name = "ValidName" };
            var validator = new UpdateGenreCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().Be(0);
        }
    }
}