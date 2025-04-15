using WebApi.DBOperations;
using System.Linq;
using System;
using AutoMapper;
using WebApi.Entities;

namespace WebApi.BookOperations.Command
{
    public class CreateAuthorCommand
    {
        public CreateAuthorModel Model { get; set; }
        private readonly IBookStoreDbContext _dbContext;
        private readonly IMapper _mapper;
        public CreateAuthorCommand(IBookStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void Handle()
        {
            var author = _dbContext.Authors.SingleOrDefault(x => x.Name == Model.Name && x.Surname == Model.Surname);
            if (author != null)
            {
                throw new InvalidOperationException("Yazar zaten mevcut.");
            }
            //_mapper ile mapleme işlemi yapıyoruz. Model sınıfını Author sınıfına dönüştürüyoruz. 
            author = _mapper.Map<Author>(Model);

            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();
        }
    }


    public class CreateAuthorModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
    }
}
