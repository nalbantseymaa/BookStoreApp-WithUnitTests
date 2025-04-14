namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.DBOperations;
    using AutoMapper;
    using WebApi.BookOperations.Query;
    using WebApi.Entities;

    public class GetBooksQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBooksQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenBooksExist_Books_ShouldBeReturnedWithAuthorAndGenre()
        {
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

            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var result = query.Handle();

            result.Should().NotBeNull();
            result.Count.Should().BeGreaterThan(0);


            result.First().AuthorName.Should().NotBeNullOrEmpty();
            result.First().Genre.Should().NotBeNullOrEmpty();
            result.First().Title.Should().NotBeNullOrEmpty();
            result.First().PageCount.Should().BeGreaterThan(0);
            result.First().PublishDate.Should().NotBeNullOrEmpty();
        }
    }

}
