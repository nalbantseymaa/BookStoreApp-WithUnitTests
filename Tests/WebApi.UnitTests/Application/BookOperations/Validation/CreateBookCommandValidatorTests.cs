namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.BookOperations.Command;
    using static WebApi.BookOperations.Command.CreateBookCommand;
    using WebApi.BookOperations.Validation;

    public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        //bir test metohudunun birden fazla veri için/birden fazla kez çalıştırılmasını sağlamak için kullanılır.theory kullanılır.
        //değişen veriye göre test yazmamak için 
        [Theory]
        [InlineData("Lord of the rings", 0, 0)]
        [InlineData("Lord of the rings", 0, 1)]
        [InlineData("Lord of the rings", 100, 0)]
        [InlineData("", 0, 0)]
        [InlineData("", 100, 1)]
        [InlineData("", 0, 1)]
        [InlineData("Lord", 0, 1)]
        [InlineData("Lord", 100, 0)]

        public void WhenInvalidInputsAreGiven_Validator_ShouldBeReturnErrors(string title, int genreId, int pageCount)
        {
            // Arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = title,
                GenreId = genreId,
                PageCount = pageCount,
                PublishDate = DateTime.Now.Date.AddDays(-1),
            };

            // Act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // Assert
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        //datetime için datetime.now gibi tanımlama yapamayaız.bu her test sırasında bu casein değişbeliceği anlamına gelir.bu yüzden de testle doğru çalışamayabilir.
        //datetime bugünde küçük olmalı

        //bir test içiinde yanlnıza bir case cover edilmeli
        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldBeReturnError()
        {
            // Arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date,
            };

            // Act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // Assert-testCase
            result.Errors.Count.Should().BeGreaterThan(0);
        }

        //happypath-her koşulun doğru çalıştığı case
        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotBeReturnError()
        {
            // Arrange
            CreateBookCommand command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date.AddYears(-2),
            };

            // Act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            // Assert-testCase
            result.Errors.Count.Should().Be(0);
        }
    }
}
