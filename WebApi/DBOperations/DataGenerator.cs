using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Entities;

namespace WebApi.DBOperations
{
    //4.Initial Data için bir Data Generator'ın yazılması
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return;
                }


                context.Genres.AddRange(
                new Genre() { Name = "Personal Growth" },
                new Genre() { Name = "Science Fiction" },
                new Genre() { Name = "Romance" },
                new Genre() { Name = "Software Development" }
 );

                context.Authors.AddRange(
                    new Author()
                    {
                        Name = "Şeyma",
                        Surname = "Nalbant",
                        Birthday = new DateTime(2002, 07, 12)
                    },
                    new Author()
                    {
                        Name = "George",
                        Surname = "Orwell",
                        Birthday = new DateTime(1903, 06, 25)
                    },
                    new Author()
                    {
                        Name = "Jane",
                        Surname = "Austen",
                        Birthday = new DateTime(1775, 12, 16)
                    }
                );

                context.Books.AddRange(
                    new Book()
                    {
                        Title = ".NET Core ile Web API Geliştirme",
                        GenreId = 4,
                        PageCount = 320,
                        PublishDate = new DateTime(2023, 11, 15),
                        AuthorId = 1
                    },
                    new Book()
                    {
                        Title = "1984",
                        GenreId = 2,
                        PageCount = 328,
                        PublishDate = new DateTime(1949, 06, 08),
                        AuthorId = 2
                    },
                    new Book()
                    {
                        Title = "Pride and Prejudice",
                        GenreId = 3,
                        PageCount = 432,
                        PublishDate = new DateTime(1813, 01, 28),
                        AuthorId = 3
                    }
                );
                context.SaveChanges();
            }
        }
    }
}