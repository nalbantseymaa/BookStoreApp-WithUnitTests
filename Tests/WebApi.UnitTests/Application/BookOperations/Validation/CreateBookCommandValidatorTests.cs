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
        [InlineData("", 0, 0, 0)]
        [InlineData("A", 1, 1, 1)]
        [InlineData("Valid Title", 0, 1, 100)]
        [InlineData("Valid Title", 1, 0, 100)]
        [InlineData("Valid Title", 1, 1, 0)]
        public void WhenInvalidInputsAreGiven_Validator_ShouldReturnErrors(string title, int genreId, int authorId, int pageCount)
        {
            var command = new CreateBookCommand(null, null);
            command.Model = new CreateBookModel()
            {
                Title = title,
                GenreId = genreId,
                AuthorId = authorId,
                PageCount = pageCount,
                PublishDate = DateTime.Now.Date.AddDays(-1), // geçmiş bir tarih veriliyor
            };

            var validator = new CreateBookCommandValidator();
            var result = validator.Validate(command);

            result.Errors.Count.Should().BeGreaterThan(0); // en az bir hata bekleniyor
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
