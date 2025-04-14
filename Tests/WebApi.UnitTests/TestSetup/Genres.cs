using WebApi.Entities;
using WebApi.DBOperations;
namespace TestSetup
{
    public static class Genres
    {
        public static void AddGenres(this BookStoreDbContext context)
        {
            context.Genres.AddRange(
                new Genre() { Name = "Personal Growth" },
                new Genre() { Name = "Science Fiction" },
                new Genre() { Name = "Romance" },
                new Genre() { Name = "Software Development" }
            );
        }
    }
}