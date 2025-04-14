using FluentAssertions;
using TestSetup;
using WebApi.BookOperations.Command;
using WebApi.BookOperations.Validation;
using WebApi.DBOperations;
using static WebApi.BookOperations.Command.UpdateBookCommand;

namespace Application.BookOperations.Validation
{
    public class UpdateBookCommandTestsValidator : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public UpdateBookCommandTestsValidator(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Theory]
        [InlineData("", 0, 0, 0)]               // Tüm alanlar hatalı
        [InlineData("A", 1, 1, 1)]              // Title kısa
        [InlineData("Valid Title", 0, 1, 100)]  // GenreId hatalı
        [InlineData("Valid Title", 1, 0, 100)]  // AuthorId hatalı
        [InlineData("Valid Title", 1, 1, 0)]    // PageCount hatalı
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string title, int genreId, int authorId, int pageCount)
        {
            UpdateBookCommand command = new UpdateBookCommand(null);
            command.BookId = 0; // BookId da hatalı
            command.Model = new UpdateBookModel()
            {
                Title = title,
                GenreId = genreId,
                AuthorId = authorId,
                PageCount = pageCount,
                PublishDate = DateTime.Now.AddDays(1)
            };

            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }


        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldReturnError()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.Model = new UpdateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date,
            };

            var validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void WhenValidInputsIdIsGiven_Validator_ShouldNotReturnErrors()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);
            command.BookId = 1;
            command.Model = new UpdateBookModel() { Title = "Valid Title", GenreId = 1, PageCount = 100, PublishDate = DateTime.Now.Date.AddDays(-1) };

            UpdateBookCommandValidator validator = new UpdateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0);
        }
    }
}
