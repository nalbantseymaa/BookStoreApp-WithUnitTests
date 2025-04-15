namespace Application.AuthorOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using WebApi.Application.GenreOperations.Command;
    using WebApi.DBOperations;
    using WebApi.Entities;
    public class DeleteAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        public DeleteAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }
        [Fact]
        public void WhenAuthorIdDoesNotExist_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = 456;

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar bulunamadı.");
        }

        [Fact]
        public void WhenAuthorIdExists_BooksCountIsNotZero_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var author = new Author() { Name = "Test Author", Surname = "Test Surname", Birthday = new DateTime(1990, 1, 1) };
            author.Books.Add(new Book() { Title = "Test Book", GenreId = 1, PageCount = 100, PublishDate = new DateTime(2020, 1, 1) });
            _context.Authors.Add(author);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = author.Id;

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazara ait kitaplar silinmeden yazar silinemez!");
        }

        [Fact]
        public void WhenAuthorIdExists_Author_ShouldBeDeleted()
        {
            // Arrange
            var author = new Author() { Name = "Test Author", Surname = "Test Surname", Birthday = new DateTime(1990, 1, 1) };
            _context.Authors.Add(author);
            _context.SaveChanges();

            DeleteAuthorCommand command = new DeleteAuthorCommand(_context);
            command.AuthorId = author.Id;

            // Act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Assert
            var deletedAuthor = _context.Authors.SingleOrDefault(x => x.Id == author.Id);
            deletedAuthor.Should().BeNull();
        }
    }
}