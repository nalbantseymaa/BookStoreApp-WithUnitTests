namespace Application.AuthorOperations.Query
{
    using FluentAssertions;
    using Xunit;
    using WebApi.DBOperations;
    using AutoMapper;
    using WebApi.BookOperations.Query;
    using TestSetup;
    using WebApi.Entities;

    public class GetAuthorDetailQueryTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public GetAuthorDetailQueryTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenAuthorNotFound_InvalidOperationException_ShouldBeThrown()
        {
            // Arrange
            var query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = 9999; // Böyle bir ID yok

            // Act & Assert
            FluentActions.Invoking(() => query.Handle())
                .Should().Throw<InvalidOperationException>()
                .WithMessage("Yazar bulunamadı.");
        }

        [Fact]
        public void WhenAuthorExists_AuthorDetailViewModel_ShouldBeReturnedWithBooks()
        {
            // Arrange
            var author = new Author
            {
                Name = "Test",
                Surname = "Yazar",
                Birthday = new DateTime(1985, 5, 5),
                Books = new List<Book>
        {
            new Book
            {
                Title = "Test Kitap 1",
                PageCount = 200,
                PublishDate = new DateTime(2010, 1, 1),
                Genre = new Genre { Name = "Roman" }
            }
        }
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var query = new GetAuthorDetailQuery(_context, _mapper);
            query.AuthorId = author.Id;

            // Act
            var result = query.Handle();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<AuthorDetailViewModel>();
            result.Name.Should().Be(author.Name);
            result.Surname.Should().Be(author.Surname);
            result.Birthday.Should().Be(author.Birthday); // DateTime olarak doğrudan kıyaslıyoruz
            result.Books.Should().HaveCount(1);
            result.Books.First().Title.Should().Be("Test Kitap 1");
        }

    }
}