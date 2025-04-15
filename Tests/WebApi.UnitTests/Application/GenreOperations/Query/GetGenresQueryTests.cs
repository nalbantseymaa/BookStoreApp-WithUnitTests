namespace Application.GenreOperations.Query
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.DBOperations;
    using AutoMapper;
    using WebApi.Entities;
    using WebApi.Application.GenreOperations.Query;

    public class GetGenresQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetGenresQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenValidInputsAreGiven_Genres_ShouldBeReturned()
        {
            var genre = new Genre() { Name = "Drama", IsActive = true };
            _context.Genres.Add(genre);
            _context.SaveChanges();

            GetGenresQuery command = new GetGenresQuery(_context, _mapper);

            var result = command.Handle();

            result.Should().NotBeNull();
            result.Count.Should().BeGreaterThan(0);
            result.Should().Contain(g => g.Name == "Drama");

        }
    }
}
