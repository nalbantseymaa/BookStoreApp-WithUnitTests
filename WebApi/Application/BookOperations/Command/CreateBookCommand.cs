using WebApi.DBOperations;
using System.Linq;
using System;
using AutoMapper;
using WebApi.Entities;

namespace WebApi.BookOperations.Command
{
    public class CreateBookCommand
    {
        public CreateBookModel Model { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        //sadece constructor üzerinden set ediliyor.

        private readonly IMapper _mapper;
        public CreateBookCommand(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(x => x.Title == Model.Title);
            if (book != null)
            {
                throw new InvalidOperationException("Kitap zaten mevcut.");
            }

            var author = _dbContext.Authors.SingleOrDefault(x => x.Id == Model.AuthorId);
            if (author is null)
            {
                throw new InvalidOperationException("Yazar mevcut olmadığından kitap eklenemez.");
            }
            book = _mapper.Map<Book>(Model);
            //Model ile gelen veri book objesine convert edilir 

            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();

        }

        public class CreateBookModel
        {
            public string Title { get; set; }
            public int GenreId { get; set; }
            public int PageCount { get; set; }
            public DateTime PublishDate { get; set; }
            public int AuthorId { get; set; }
        }
    }
}