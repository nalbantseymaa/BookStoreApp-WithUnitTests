using System;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.BookOperations.Command
{
    public class UpdateBookCommand
    {
        public int BookId { get; set; }
        public UpdateBookModel Model { get; set; }

        private readonly IBookStoreDbContext _dbContext;
        public UpdateBookCommand(IBookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
            if (book is null)
            {
                throw new InvalidOperationException("Güncellenecek kitap bulunamadı.");
            }

            var author = _dbContext.Authors.SingleOrDefault(x => x.Id == Model.AuthorId);
            if (author is null)
            {
                throw new InvalidOperationException("Güncellenecek yazar bilgisi mevcut değil.");
            }

            var genre = _dbContext.Genres.SingleOrDefault(x => x.Id == Model.GenreId);
            if (genre is null)
            {
                throw new InvalidOperationException("Güncellenecek tür bilgisi mevcut değil.");
            }

            book.Title = !string.IsNullOrWhiteSpace(Model.Title) ? Model.Title : book.Title;
            book.GenreId = Model.GenreId ?? book.GenreId;
            book.PageCount = (int)(Model.PageCount > 0 ? Model.PageCount : book.PageCount);
            book.PublishDate = Model.PublishDate ?? book.PublishDate;
            book.AuthorId = Model.AuthorId ?? book.AuthorId;

            _dbContext.SaveChanges();
        }

        public class UpdateBookModel
        {
            public string? Title { get; set; }
            public int? GenreId { get; set; }
            public int? PageCount { get; set; }
            public DateTime? PublishDate { get; set; } // Nullable DateTime
            public int? AuthorId { get; set; }

        }

    }
}