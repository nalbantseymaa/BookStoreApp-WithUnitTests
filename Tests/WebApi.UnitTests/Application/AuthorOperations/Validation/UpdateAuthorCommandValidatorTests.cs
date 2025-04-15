using FluentAssertions;
using TestSetup;
using WebApi.Application.GenreOperations.Command;
using WebApi.AuthorOperations.Validation;
using WebApi.DBOperations;

namespace Application.AuthorOperations.Validation
{
    public class UpdateAuthorCommandTestsValidator : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public UpdateAuthorCommandTestsValidator(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData("", "Yılmaz")]
        [InlineData(" ", "Yılmaz")]
        [InlineData("A", "Yılmaz")]
        [InlineData("Ali", "")]
        [InlineData("Ali", " ")]
        [InlineData("Ali", "B")]
        [InlineData(" ", " ")]
        [InlineData("", "")]
        [InlineData("A", "B")]
        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string name, string surname)
        {
            var command = new UpdateAuthorCommand(null);
            command.Model = new UpdateAuthorModel() { Name = name, Surname = surname };
            var validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenInvalidDateIsGiven_Validator_ShouldReturnError()
        {
            var command = new UpdateAuthorCommand(null);
            command.Model = new UpdateAuthorModel()
            {
                Name = "Alice",
                Surname = "Taylor",
                Birthday = DateTime.Now.Date
            };
            var validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
        {
            var command = new UpdateAuthorCommand(null);
            command.Model = new UpdateAuthorModel() { Name = "Yuri", Surname = "Parker", Birthday = DateTime.Now.Date.AddYears(-1) };
            var validator = new UpdateAuthorCommandValidator();
            var result = validator.Validate(command);
            result.Errors.Count.Should().Be(0);
        }
    }
}