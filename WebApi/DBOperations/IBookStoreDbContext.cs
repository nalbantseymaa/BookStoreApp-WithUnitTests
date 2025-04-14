namespace WebApi.DBOperations
{
    using Microsoft.EntityFrameworkCore;
    using WebApi.Entities;

    //3.Db operasyonları için kullanılacak olan DB Context'i yaratılması

    public interface IBookStoreDbContext
    {
        DbSet<Book> Books { get; set; }
        DbSet<Genre> Genres { get; set; }
        DbSet<Author> Authors { get; set; }
        int SaveChanges();
    }
}