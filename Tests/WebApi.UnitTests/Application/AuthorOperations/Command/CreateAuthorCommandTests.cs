namespace Application.AuthorOperations.Command
{
    using FluentAssertions;
    using WebApi.DBOperations;
    using WebApi.Entities;
    using Xunit;
    using AutoMapper;
    using TestSetup;
    using WebApi.BookOperations.Command;

    public class CreateAuthorCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateAuthorCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        [Fact]
        public void WhenValidInputsAreGiven_Author_ShouldBeCreated()
        {
            var command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel()
            {
                Name = "NewAuthor",
                Surname = "Author",
                Birthday = DateTime.Now.Date.AddYears(-2),
            };

            FluentActions.Invoking(() => command.Handle()).Invoke();

            var author = _context.Authors.SingleOrDefault(x => x.Name == command.Model.Name && x.Surname == command.Model.Surname);
            author.Should().NotBeNull();
            author.Birthday.Should().Be(command.Model.Birthday);
        }

        [Fact]
        public void WhenAlreadyExistingAuthorIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange
            var author = new Author()
            {
                Name = "Test",
                Surname = "Author",
                Birthday = new DateTime(1990, 1, 1)
            };
            _context.Authors.Add(author);
            _context.SaveChanges();

            var command = new CreateAuthorCommand(_context, _mapper);
            command.Model = new CreateAuthorModel()
            {
                Name = author.Name,
                Surname = author.Surname,
                Birthday = author.Birthday.Date
            };

            // Act & Assert
            FluentActions.Invoking(() => command.Handle()).Should().Throw<InvalidOperationException>().And.Message.Should().Be("Yazar zaten mevcut.");
        }
    }
}
