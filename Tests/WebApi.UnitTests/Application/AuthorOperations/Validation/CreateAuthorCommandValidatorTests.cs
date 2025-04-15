namespace Application.AuthorOperations.Validation
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.BookOperations.Command;
    using WebApi.BookOperations.Validation;

    public class CreateAuthorCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        [Theory]
        [InlineData("", "Smith")]
        [InlineData("A", "Smith")]
        [InlineData("John", "")]
        [InlineData("John", "B")]
        [InlineData("", "")]
        [InlineData("A", "B")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string name, string surname)
        {
            var command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel() { Name = name, Surname = surname };
            var validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenInvalidDateIsGiven__Validator_ShouldReturnError()
        {
            var command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel()
            {
                Name = "Alice",
                Surname = "Taylor",
                Birthday = DateTime.Now.Date // bugünün tarihi
            };
            var validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().BeGreaterThan(0); // bugünün tarihi geçersiz kabul edilir
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
        {
            var command = new CreateAuthorCommand(null, null);
            command.Model = new CreateAuthorModel() { Name = "Yuri", Surname = "Parker", Birthday = DateTime.Now.Date.AddYears(-1) };
            var validator = new CreateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().Be(0); // Geçerli bir sonuç döner
        }
    }
}