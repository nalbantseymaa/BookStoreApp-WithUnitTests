namespace Application.GenreOperations.Query
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.DBOperations;
    using WebApi.Entities;
    using WebApi.Application.GenreOperations.Query;
    using AutoMapper;

    public class GetGenreDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenreDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenInvalidGenreIdIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = 999;

            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Aradığınız kitap türü bulunamadı");
        }

        [Fact]
        public void WhenValidGenreIdIsGiven_Genre_ShouldBeReturned()
        {
            var genre = new Genre
            {
                Name = "Test Kitap Türü",
                IsActive = true
            };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            GetGenreDetailQuery query = new GetGenreDetailQuery(_context, _mapper);
            query.GenreId = genre.Id;

            var result = query.Handle();

            result.Should().NotBeNull();
            result.Name.Should().Be(genre.Name);
        }
    }

}
