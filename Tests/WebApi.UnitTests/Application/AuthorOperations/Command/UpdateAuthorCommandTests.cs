namespace Application.AuthorOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using Xunit;
    using global::WebApi.DBOperations;
    using global::WebApi.Entities;
    using global::WebApi.Application.GenreOperations.Command;

    public class UpdateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidAuthorIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = 0;
            command.Model = new UpdateAuthorModel() { Name = "Test", Surname = "TestSurname" };

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar Bulunamdı!");
        }
        [Fact]
        public void WhenAlreadyExistAuthorNameAndSurnameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var author = new Author() { Name = "Test", Surname = "TestSurname" };
            _context.Authors.Add(author);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = author.Id;
            command.Model = new UpdateAuthorModel() { Name = author.Name, Surname = author.Surname };

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Bu isimde yazar zaten mevcut!");
        }

        [Fact]
        public void WhenValidAuthorIdIsGiven_Author_ShouldBeUpdated()
        {
            // Arrange
            var author = new Author() { Name = "Test", Surname = "TestSurname" };
            _context.Authors.Add(author);
            _context.SaveChanges();

            UpdateAuthorCommand command = new UpdateAuthorCommand(_context);
            command.AuthorId = author.Id;
            command.Model = new UpdateAuthorModel() { Name = "UpdatedName", Surname = "UpdatedSurname", Birthday = DateTime.Now.AddYears(-3) };

            // Act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Assert
            var updatedAuthor = _context.Authors.SingleOrDefault(x => x.Id == author.Id);
            updatedAuthor.Should().NotBeNull();
            updatedAuthor.Name.Should().Be(command.Model.Name);
            updatedAuthor.Surname.Should().Be(command.Model.Surname);
            updatedAuthor.Birthday.Should().Be(command.Model.Birthday.Value);
        }
    }
}