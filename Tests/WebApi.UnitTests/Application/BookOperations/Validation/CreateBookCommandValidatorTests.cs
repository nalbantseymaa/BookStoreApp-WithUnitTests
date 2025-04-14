namespace Application.BookOperations.Command
{
    using FluentAssertions;
    using Xunit;
    using TestSetup;
    using WebApi.BookOperations.Command;
    using static WebApi.BookOperations.Command.CreateBookCommand;
    using WebApi.BookOperations.Validation;

    public class CreateBookCommandValidatorTests : IClassFixture<CommonTestFixture>
    {
        // Bir test metodunun farklı veri setleriyle tekrar tekrar çalıştırılmasını sağlamak için [Theory] ve [InlineData] kullanılır.
        // Bu test, geçersiz girişler verildiğinde validatordan hata dönmesini bekler.

        [Theory]
        [InlineData("", 1, 100, 1)]          // Title boş, hata bekleniyor
        [InlineData("A", 1, 100, 1)]         // Title çok kısa (<4 karakter), hata bekleniyor
        [InlineData("Abc", 1, 100, 1)]       // Title yine < 4 karakter, hata bekleniyor
        [InlineData("Valid Title", 0, 100, 1)]   // GenreId 0, hata bekleniyor
        [InlineData("Valid Title", 1, 100, 0)]   // AuthorId 0, hata bekleniyor
        [InlineData("Valid Title", 1, 100, -1)]  // AuthorId negatif, hata bekleniyor
        [InlineData("Valid Title", -1, 100, 1)]  // PageCount negatif, hata bekleniyor
        [InlineData("Valid Title", 1, 0, 1)]     // PageCount 0, hata bekleniyor
        [InlineData("Valid Title", 1, 100, 0)]   // AuthorId 0, hata bekleniyor
        [InlineData("", 0, 0, 0)]               // Hepsi geçersiz, hata bekleniyor
        public void WhenInvalidInputAreGiven_Validator_ShouldBeReturnErrors(string title, int genreId, int authorId, int pageCount)
        {
            //arrange
            CreateBookCommand command = new CreateBookCommand(null!, null!);
            command.Model = new CreateBookModel() { Title = title, PageCount = pageCount, PublishDate = DateTime.Now.Date.AddYears(-1), GenreId = genreId, AuthorId = authorId };

            //act
            CreateBookCommandValidator validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            //assert
            result.Errors.Count.Should().BeGreaterThan(0);

        }

        // PublishDate, bugünden küçük olmalı. DateTime.Now verildiğinde test her çalıştırıldığında farklı zamanlar oluşur ve hatalı sonuçlar dönebilir.

        [Fact]
        public void WhenDateTimeEqualNowIsGiven_Validator_ShouldReturnError()
        {
            var command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date, // bugünün tarihi
            };

            var validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0); // bugünün tarihi geçersiz kabul edilir
        }

        // Happy Path: Tüm değerler geçerli. Hiçbir validation hatası beklenmiyor.

        [Fact]
        public void WhenValidInputsAreGiven_Validator_ShouldNotReturnError()
        {
            var command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = "Lord of the Rings",
                GenreId = 1,
                AuthorId = 2,
                PageCount = 100,
                PublishDate = DateTime.Now.Date.AddYears(-2), // 2 yıl öncesi
            };

            var validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().Be(0); // hata dönmemeli
        }
    }
}
