using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;

namespace TestSetup
{
    // Bu sınıf, testlerde tekrar tekrar kullanılacak ortak nesneleri (veritabanı, mapper) oluşturur.
    // EN: This class provides shared objects (like DB context and mapper) for all test classes.

    public class CommonTestFixture
    {
        public BookStoreDbContext Context { get; set; }
        public IMapper Mapper { get; set; }

        public CommonTestFixture()
        {
            var options = new DbContextOptionsBuilder<BookStoreDbContext>()
                .UseInMemoryDatabase(databaseName: "BookStoreTestDB")
                .Options;

            Context = new BookStoreDbContext(options);

            Context.Database.EnsureCreated();

            // Test ortamı için örnek veri eklenir
            Context.AddBooks();
            Context.AddGenres();
            Context.AddAuthors();
            Context.SaveChanges();

            // MappingProfile sınıfı üzerinden bir AutoMapper konfigürasyonu oluşturuluyor
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper();
        }
    }
}
