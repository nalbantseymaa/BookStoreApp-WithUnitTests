using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;

namespace WebApi.BookOperations.Query
{
    public class GetBookDetailQuery
    {
        public int BookId { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetBookDetailQuery(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;

        }

        public BookDetailViewModel Handle()
        {
            var book = _dbContext.Books
                .Include(x => x.Genre)
                .Include(x => x.Author).SingleOrDefault(a => a.Id == BookId);

            if (book is null)
            {
                throw new InvalidOperationException("Kitap bulunamadı.");
            }

            BookDetailViewModel vm = _mapper.Map<BookDetailViewModel>(book);
            return vm;
        }
    }
}