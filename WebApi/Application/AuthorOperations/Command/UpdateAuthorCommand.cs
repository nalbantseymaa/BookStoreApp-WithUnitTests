using System;
using System.Linq;
using WebApi.DBOperations;

namespace WebApi.Application.GenreOperations.Command
{
    public class UpdateAuthorCommand
    {
        public IBookStoreDbContext _context { get; set; }
        public int AuthorId { get; set; }
        public UpdateAuthorModel Model { get; set; }

        public UpdateAuthorCommand(IBookStoreDbContext context)
        {
            _context = context;
        }

        public UpdateAuthorCommand(BookStoreDbContext context)
        {
            _context = context;
        }

        public void Handle()
        {
            var author = _context.Authors.SingleOrDefault(x => x.Id == AuthorId);
            if (author is null)
                throw new InvalidOperationException("Yazar Bulunamdı!");

            if (_context.Authors.Any(x => x.Name.ToLower() == Model.Name.ToLower() && x.Surname.ToLower() == Model.Surname.ToLower()))
                throw new InvalidOperationException("Bu isimde yazar zaten mevcut!");

            author.Name = string.IsNullOrWhiteSpace(Model.Name) ? author.Name : Model.Name;
            author.Surname = string.IsNullOrWhiteSpace(Model.Surname) ? author.Surname : Model.Surname;
            author.Birthday = Model.Birthday.HasValue ? Model.Birthday.Value : author.Birthday;

            _context.SaveChanges();
        }
    }

    public class UpdateAuthorModel
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime? Birthday { get; set; }
    }
}