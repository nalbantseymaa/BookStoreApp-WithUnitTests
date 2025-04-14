namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using WebApi.DBOperations;
    using WebApi.Entities;
    using Xunit;
    using AutoMapper;
    using TestSetup;
    using WebApi.BookOperations.Command;
    using static WebApi.BookOperations.Command.CreateBookCommand;

    // ❗ CreateBookCommandTests sınıfı, gerçek projedeki CreateBookCommand sınıfını test etmek için yazılmıştır.
    // Testlerde kullanılacak veritabanı (DbContext) ve nesne dönüştürücü (AutoMapper), CommonTestFixture üzerinden sağlanır.
    // Bu sınıf, dışarıdan biri CreateBook komutunu çağırmış gibi davranarak sistemi sınar.
    //Test sınıfına dışarıdan hazır ortam sağlar. 
    // EN: This class tests the CreateBookCommand by simulating real use cases.

    public class CreateBookCommandTests : IClassFixture<CommonTestFixture>
    {
        private readonly BookStoreDbContext _context;
        private readonly IMapper _mapper;

        public CreateBookCommandTests(CommonTestFixture testFixture)
        {
            _context = testFixture.Context;
            _mapper = testFixture.Mapper;
        }

        // ❗ Test 1: Aynı başlığa sahip bir kitap zaten veritabanında varsa,
        // CreateBookCommand içindeki Handle() metodu InvalidOperationException fırlatmalı.
        // EN: This test ensures that trying to add a book with an existing title throws an exception.

        [Fact]
        public void WhenAlreadyExistBookTitleIsGiven_InvalidOperationException_ShouldBeReturn()
        {
            // Arrange – Test için gerekli ortam hazırlanır
            var book = new Book
            {
                Title = "Existing Book",
                GenreId = 1,
                PageCount = 200,
                PublishDate = new DateTime(2023, 1, 1),
                AuthorId = 1
            };
            _context.Books.Add(book);
            _context.SaveChanges();

            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            command.Model = new CreateBookCommand.CreateBookModel
            {
                Title = book.Title, // Aynı başlığa sahip model set edilir
            };

            // Act & Assert – Metodun hata fırlatıp fırlatmadığı kontrol edilir
            //Bir işlemin hata fırlatıp fırlatmadığını kontrol eder.
            FluentActions.Invoking(() => command.Handle())
                         .Should()
                         .Throw<InvalidOperationException>()
                         .And.Message
                         .Should().Be("Kitap zaten mevcut.");
        }

        // ❗ Test 2: Geçerli bilgiler verildiğinde, kitap başarıyla veritabanına eklenmeli.
        // Bu senaryo “happy path” olarak adlandırılır (her şeyin doğru çalıştığı durum).
        // EN: This test ensures that a valid book is created successfully.

        [Fact]
        public void WhenValidInputsAreGiven_Book_ShouldBeCreated()
        {
            // Arrange – Komut ve model hazırlanır
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);

            CreateBookModel model = new CreateBookModel()
            {
                Title = "Hobbit",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date.AddYears(-2),
            };
            command.Model = model;

            // Act – Komut çalıştırılır
            FluentActions.Invoking(() => command.Handle()).Invoke();

            // Assert – Kitap gerçekten veritabanına eklendi mi kontrol edilir
            var book = _context.Books.SingleOrDefault(b => b.Title == model.Title);

            book.Should().NotBeNull();                      // Kitap var mı
            book.GenreId.Should().Be(model.GenreId);        // Tür doğru mu
            book.AuthorId.Should().Be(model.AuthorId);      // Yazar doğru mu
            book.PageCount.Should().Be(model.PageCount);    // Sayfa sayısı doğru mu
            book.PublishDate.Should().Be(model.PublishDate);// Yayın tarihi doğru mu
        }
    }
}
