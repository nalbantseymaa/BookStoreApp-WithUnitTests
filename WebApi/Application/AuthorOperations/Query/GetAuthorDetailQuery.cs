
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Common;
using WebApi.DBOperations;


namespace WebApi.BookOperations.Query
{
    public class GetAuthorDetailQuery
    {
        public int AuthorId { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;


        public GetAuthorDetailQuery(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public AuthorDetailViewModel Handle()
        {
            var author = _dbContext.Authors
                .Include(x => x.Books)
                .ThenInclude(b => b.Genre)
                .SingleOrDefault(a => a.Id == AuthorId);

            if (author is null)
            {
                throw new InvalidOperationException("Yazar bulunamadı.");
            }

            AuthorDetailViewModel vm = _mapper.Map<AuthorDetailViewModel>(author);
            return vm;
        }

    }

    public class AuthorDetailViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        // Kullanıcıya yazar bilgileriyle birlikte kitaplarını da göstermek istiyoruz.
        // Bu yüzden kitaplar Include ile veritabanından çekilecek ve bu liste BookDetailViewModel'e maplenecek.
        public List<BookViewModel> Books { get; set; }


    }
}