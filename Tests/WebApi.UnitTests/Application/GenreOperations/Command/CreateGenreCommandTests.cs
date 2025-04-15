namespace Application.GenreOperations.Command
{
    using FluentAssertions;
    using WebApi.DBOperations;
    using WebApi.Entities;
    using Xunit;
    using TestSetup;
    using WebApi.Application.GenreOperations.Command;


    public class CreateGenreCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;

        public CreateGenreCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
        }

        [Fact]
        public void WhenInvalidGenreIsGiven_InvalidOperationException_ShouldBeThrow()
        {
            var genre = new Genre
            {
                Name = "Horror",
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            CreateGenreModel model = new CreateGenreModel
            {
                Name = genre.Name
            };

            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Should()
            .Throw<InvalidOperationException>().And.Message.Should().Be("Kitap türü zaten mevcut.");
        }

        [Fact]
        public void WhenValidGenreIsGiven_Genre_ShouldBeCreated()
        {
            CreateGenreModel model = new CreateGenreModel
            {
                Name = "Crime"
            };

            CreateGenreCommand command = new CreateGenreCommand(_context);
            command.Model = model;

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var genre = _context.Genres.SingleOrDefault(x => x.Name == model.Name);
            genre.Should().NotBeNull();
            genre.Name.Should().Be(model.Name);
        }
    }
}
