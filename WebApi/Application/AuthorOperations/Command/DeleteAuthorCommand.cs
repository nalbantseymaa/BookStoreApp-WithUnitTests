using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;


namespace WebApi.Application.GenreOperations.Command
{
    public class DeleteAuthorCommand
    {
        private readonly IBookStoreDbContext _context;

        public int AuthorId { get; set; }
        public DeleteAuthorCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            // Yazar bilgisi kitapları ile birlikte çekiliyor
            var author = _context.Authors
                                 .Include(x => x.Books)
                                 .SingleOrDefault(x => x.Id == AuthorId);

            if (author is null)
                throw new InvalidOperationException("Yazar bulunamadı.");

            if (author.Books.Any())
                throw new InvalidOperationException("Yazara ait kitaplar silinmeden yazar silinemez!");

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }


    }
}