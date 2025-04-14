using WebApi.Entities;
using WebApi.DBOperations;
namespace TestSetup
{
    public static class Books
    {
        public static void AddBooks(this BookStoreDbContext context)//extension metohoda haline getirilir
        {
            context.Books.AddRange(
            new Book() { Title = ".NET Core ile Web API Geliştirme", GenreId = 4, PageCount = 320, PublishDate = new DateTime(2023, 11, 15), AuthorId = 1 },
            new Book() { Title = "1984", GenreId = 2, PageCount = 328, PublishDate = new DateTime(1949, 06, 08), AuthorId = 2 },
            new Book() { Title = "Pride and Prejudice", GenreId = 3, PageCount = 432, PublishDate = new DateTime(1813, 01, 28), AuthorId = 3 });
        }
    }
}