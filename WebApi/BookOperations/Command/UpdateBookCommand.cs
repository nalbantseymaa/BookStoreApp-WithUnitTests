using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.Command
{
    public class UpdateBookCommand
    {
        public int BookId { get; set; }
        public UpdateBookModel Model { get; set; }

        private readonly BookStoreDbContext _dbContext;
        public UpdateBookCommand(BookStoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle()
        {

            var book = _dbContext.Books.SingleOrDefault(x => x.Id == BookId);
            if (book is null)
                throw new InvalidOperationException("Güncellenecek Kitap bulunamadı.");

            book.Title = Model.Title != default ? Model.Title : book.Title;
            book.GenreId = Model.GenreId != null ? Model.GenreId : book.GenreId;

            // ((GenreEnum)Model.GenreId).ToString();
            // book.PageCount = Model.PageCount;
            // book.PublishDate = Model.PublishDate;

            _dbContext.SaveChanges();


        }



        public class UpdateBookModel
        {
            public string Title { get; set; }
            public int GenreId { get; set; }

            // public int PageCount { get; set; }
            // public string PublishDate { get; set; }
        }

    }
}