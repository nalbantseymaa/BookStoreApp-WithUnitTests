namespace Application.AuthorOperations.Query
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.DBOperations;
    using AutoMapper;
    using WebApi.Entities;
    using WebApi.Application.GenreOperations.Query;

    public class GetAuthorQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAuthorsAreRequested_ThenAllAuthorsShouldBeReturned()
        {
            // arrange
            var initialCount = _context.Authors.Count();

            Author author1 = new Author { Name = "Author1", Surname = "Surname1", Birthday = new DateTime(1980, 1, 1) };
            Author author2 = new Author { Name = "Author2", Surname = "Surname2", Birthday = new DateTime(1990, 1, 1) };

            _context.Authors.AddRange(author1, author2);
            _context.SaveChanges();

            author1.Books.Add(new Book { Title = "Book1", GenreId = 1, PageCount = 100, PublishDate = new DateTime(2000, 1, 1) });
            author2.Books.Add(new Book { Title = "Book2", GenreId = 2, PageCount = 200, PublishDate = new DateTime(2010, 1, 1) });

            _context.SaveChanges();

            var query = new GetAuthorsQuery(_context, _mapper);

            // act
            var result = query.Handle();

            // assert
            result.Should().NotBeNull();
            //result.Count.Should().Be(initialCount + 2);

            var author1Result = result.FirstOrDefault(x => x.Name == "Author1");
            var author2Result = result.FirstOrDefault(x => x.Name == "Author2");

            author1Result.Should().NotBeNull();
            author2Result.Should().NotBeNull();

            author1Result.Books.Should().ContainSingle(x => x.Title == "Book1");
            author2Result.Books.Should().ContainSingle(x => x.Title == "Book2");
        }


    }
}
