namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.DBOperations;
    using AutoMapper;
    using WebApi.BookOperations.Query;
    using WebApi.Entities;
    using WebApi.Common;

    public class GetBookDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetBookDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenBookNotFound_InvalidOperationException_ShouldBeReturn()
        {
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = 0;

            FluentActions
                .Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>().And.Message.Should().Be("Kitap bulunamadı.");
        }

        [Fact]
        public void WhenBookExist_Books_ShouldBeReturnedWithAuthorAndGenre()
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

            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);
            query.BookId = book.Id;
            var result = query.Handle();

            result.Should().NotBeNull();
            result.Should().BeOfType<BookDetailViewModel>();
            result.Title.Should().Be(book.Title);
            result.Genre.Should().Be(book.Genre.Name);
            result.AuthorName.Should().Be(book.Author.Name + " " + book.Author.Surname);
            result.PageCount.Should().Be(book.PageCount);
            DateTime.Parse(result.PublishDate).Date.Should().Be(book.PublishDate.Date);
        }
    }

}
