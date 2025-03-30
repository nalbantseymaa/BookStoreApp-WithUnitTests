using System;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.BookOperations.Command
{
    public class DeleteBookCommand
    {

        private readonly BookStoreDbContext _dbContext;
        public int BookId { get; set; }

        public DeleteBookCommand(BookStoreDbContext context)
        {
            _dbContext = context;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
            if (book == null)
                throw new InvalidOperationException("Silincek kitap Bulunamadı.");

            _dbContext.Books.Remove(book);
            _dbContext.SaveChanges();

        }
    }
}