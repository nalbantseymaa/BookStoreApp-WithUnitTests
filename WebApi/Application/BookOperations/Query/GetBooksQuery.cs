using WebApi.DBOperations;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;

namespace WebApi.BookOperations.Query
{
    public class GetBooksQuery
    {
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBooksQuery(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public List<BookDetailViewModel> Handle()
        {
            var bookList = _dbContext.Books
                .Include(x => x.Genre)
                .Include(x => x.Author)
                .OrderBy(x => x.Id)
                .ToList();

            List<BookDetailViewModel> vm = _mapper.Map<List<BookDetailViewModel>>(bookList);

            // new List<BooksViewModel>();

            // foreach (var book in bookList)
            // {
            //     vm.Add(new BooksViewModel()
            //     {
            //         Title = book.Title,
            //         Genre = ((GenreEnum)book.GenreId).ToString(),
            //         PageCount = book.PageCount,
            //         PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy")
            //     });
            // }

            return vm;
        }
    }


}