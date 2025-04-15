namespace Application.GenreOperations.Validation
{
    using Xunit;
    using TestSetup;
    using WebApi.GenreOperations.Validation;
    using WebApi.Application.GenreOperations.Command;
    using FluentAssertions;

    public class CreateGenreCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        [InlineData("abc")]
        [InlineData("")]
        public void WhenInvalidNameIsGiven_Validator_ShouldBeReturnErrors(string name)
        {
            CreateGenreCommand command = new CreateGenreCommand(null!);
            command.Model = new CreateGenreModel();
            command.Model.Name = name;

            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();

            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }


        [Theory]
        [InlineData("Valid Genre Name")]
        public void WhenValidNameIsGiven_Validator_ShouldNotReturnErrors(string name)
        {
            CreateGenreCommand command = new CreateGenreCommand(null!);
            command.Model = new CreateGenreModel();
            command.Model.Name = name;

            CreateGenreCommandValidator validator = new CreateGenreCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}