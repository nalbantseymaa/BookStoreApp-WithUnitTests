namespace Tests.WebApi.UnitTests.Application.GenreOperations.Command
{
    using FluentAssertions;
    using TestSetup;
    using Xunit;
    using global::WebApi.DBOperations;
    using global::WebApi.Entities;
    using global::WebApi.Application.GenreOperations.Command;

    public class DeleteGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public DeleteGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenGenreIdIsNotFound_InvalidOperationException_ShouldBeThrown()
        {
            DeleteGenreCommand command = new DeleteGenreCommand(_context) { GenreId = 999 };

            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Silincek Kitap Türü Bulunamadı!");
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genre_ShouldBeDeleted()
        {
            Genre genre = new Genre { Name = "Detective ", IsActive = true };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            DeleteGenreCommand command = new DeleteGenreCommand(_context) { GenreId = genre.Id };

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var deletedGenre = _context.Genres.SingleOrDefault(x => x.Id == genre.Id);
            deletedGenre.Should().BeNull();
        }
    }
}


