namespace Application.GenreOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using Xunit;
    using global::WebApi.DBOperations;
    using global::WebApi.BookOperations.Command;
    using global::WebApi.Entities;
    using static global::WebApi.BookOperations.Command.UpdateBookCommand;
    using global::WebApi.Application.GenreOperations.Command;

    public class UpdateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public UpdateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = 987;
            command.Model = new UpdateGenreModel() { Name = "TestGenre" };

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap Türü Bulunmadı!");
        }

        [Fact]
        public void WhenAlreadyExistGenreNameIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var genre = new Genre() { Name = "Travel ", IsActive = true };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genre.Id;
            command.Model = new UpdateGenreModel() { Name = genre.Name, IsActive = false };

            // Act & Assert
            FluentActions
                .Invoking(() => command.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Bu isimde kitap türü zaten mevcut !");
        }

        [Fact]
        public void WhenValidIdIsGiven_Genre_ShouldBeUpdated()
        {
            // Arrange
            var genre = new Genre() { Name = "TestGenre", IsActive = true };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            UpdateGenreCommand command = new UpdateGenreCommand(_context);
            command.GenreId = genre.Id;
            command.Model = new UpdateGenreModel() { Name = "UpdatedGenre", IsActive = false };

            // Act
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Assert
            var updatedGenre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);
            updatedGenre.Should().NotBeNull();
            updatedGenre.Name.Should().Be("UpdatedGenre");
            updatedGenre.IsActive.Should().BeFalse();
        }
    }
}


