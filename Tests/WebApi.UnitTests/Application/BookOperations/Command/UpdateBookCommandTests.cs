namespace Tests.WebApi.UnitTests.Application.BookOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using Xunit;
    using global::WebApi.DBOperations;
    using global::WebApi.BookOperations.Command;
    using global::WebApi.Entities;
    using static global::WebApi.BookOperations.Command.UpdateBookCommand;

    public class UpdateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenBookIdIsNotFound_InvalidOperationException_ShouldBeThrow()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context) { BookId = 999 };
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Güncellenecek kitap bulunamadı.");
        }


        [Fact]
        public void WhenAuthorIdIsNotFound_InvalidOperationException_ShouldBeThrow()
        {
            var book = new Book
            {
                Title = "Test Kitabı",
                GenreId = 1,
                AuthorId = 1,
                PageCount = 250,
                PublishDate = new DateTime(2010, 05, 10)
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            UpdateBookCommand command = new UpdateBookCommand(_context)
            {
                BookId = book.Id,
                Model = new UpdateBookModel
                {
                    Title = "Updated Title",
                    GenreId = 1,
                    AuthorId = 999, // geçersiz yazar ID'si
                    PageCount = 300,
                    PublishDate = new DateTime(2020, 01, 01)
                }
            };

            FluentActions
                .Invoking(() => command.Handle()).Should()
                .Throw<InvalidOperationException>().And.Message.Should().Be("Güncellenecek yazar bilgisi mevcut değil.");
        }

        [Fact]
        public void WhenGenreIdIsNotFound_InvalidOperationException_ShouldBeThrow()
        {
            var book = new Book
            {
                Title = "Tür Test Kitabı",
                GenreId = 1,
                AuthorId = 1,
                PageCount = 220,
                PublishDate = new DateTime(2015, 01, 01)
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            UpdateBookCommand command = new UpdateBookCommand(_context)
            {
                BookId = book.Id,
                Model = new UpdateBookModel
                {
                    Title = "Yeni Başlık",
                    GenreId = 999, // Geçersiz tür ID'si
                    AuthorId = 1,
                    PageCount = 230,
                    PublishDate = new DateTime(2016, 01, 01)
                }
            };

            FluentActions
               .Invoking(() => command.Handle()).Should()
               .Throw<InvalidOperationException>().And.Message.Should().Be("Güncellenecek tür bilgisi mevcut değil.");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeUpdated()
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);

            var book = new Book
            {
                Title = "Test Kitabı",
                GenreId = 1,
                AuthorId = 1,
                PageCount = 300,
                PublishDate = new DateTime(2020, 1, 1)
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            command.BookId = book.Id;
            UpdateBookModel model = new UpdateBookModel
            {
                Title = "Updated Test Kitabı",
                GenreId = 2,
                AuthorId = 2,
                PageCount = 400,
                PublishDate = new DateTime(2021, 1, 1)
            };
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var updatedBook = _context.Books.SingleOrDefault(b => b.Id == book.Id);
            updatedBook.Should().NotBeNull();
            updatedBook.Title.Should().Be(model.Title);
            updatedBook.GenreId.Should().Be(model.GenreId);
            updatedBook.AuthorId.Should().Be(model.AuthorId);
            updatedBook.PageCount.Should().Be(model.PageCount);
            updatedBook.PublishDate.Should().Be(model.PublishDate);
        }

    }
}


