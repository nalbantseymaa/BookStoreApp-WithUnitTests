using WebApi.Entities;
using WebApi.DBOperations;
namespace TestSetup
{
    public static class Authors
    {
        public static void AddAuthors(this BookStoreDbContext context)
        {
            context.Authors.AddRange(
             new Author { Name = "Şeyma", Surname = "Nalbant", Birthday = new DateTime(2002, 07, 12) },
             new Author { Name = "George", Surname = "Orwell", Birthday = new DateTime(1903, 06, 25) },
             new Author { Name = "Jane", Surname = "Austen", Birthday = new DateTime(1775, 12, 16) });
        }
    }
}