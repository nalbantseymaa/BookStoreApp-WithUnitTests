using Microsoft.EntityFrameworkCore;

namespace WebApi.DBOperations
{
    //3.Db operasyonları için kullanılacak olan DB Context'i yaratılması

    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }

    }
}