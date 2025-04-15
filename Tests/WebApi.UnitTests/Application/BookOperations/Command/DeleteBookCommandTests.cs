namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using Xunit;
    using global::WebApi.DBOperations;
    using global::WebApi.BookOperations.Command;
    using global::WebApi.Entities;

    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenBookIdIsNotFound_InvalidOperationException_ShouldBeThrown()
        {
            DeleteBookCommand command = new DeleteBookCommand(_context) { BookId = 999 };
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Silincek Kitap Bulunamadı.");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeDeleted()
        {
            var book = new Book { Title = "Test Kitabı", GenreId = 1, AuthorId = 1, PageCount = 300, PublishDate = new DateTime(2020, 1, 1) };
            _context.Books.Add(book);
            _context.SaveChanges();

            DeleteBookCommand command = new DeleteBookCommand(_context) { BookId = book.Id };

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var deletedBook = _context.Books.SingleOrDefault(x => x.Id == book.Id);
            deletedBook.Should().BeNull();
        }
    }
}


