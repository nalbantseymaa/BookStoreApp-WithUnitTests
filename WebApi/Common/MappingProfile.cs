using AutoMapper;
using WebApi.Application.GenreOperations.Query;
using WebApi.BookOperations.Command;
using WebApi.BookOperations.Query;
using WebApi.Entities;
using static WebApi.Application.GenreOperations.Query.GetAuthorsQuery;
using static WebApi.BookOperations.Command.CreateBookCommand;

namespace WebApi.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //createbook model book nesnesine maplenebilir
            //SOURCE---->TARGET
            CreateMap<CreateBookModel, Book>();

            CreateMap<Book, BookDetailViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.Name} {src.Author.Surname}"));

            CreateMap<Book, BookViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<Genre, GenresViewModel>();

            CreateMap<Genre, GenreDetailViewModel>();

            CreateMap<CreateAuthorModel, Author>();

            CreateMap<Author, AuthorsViewModel>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Author, AuthorDetailViewModel>()
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));



            // CreateMap<Book, BooksViewModel>()
            //     .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name))
            //     .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => $"{src.Author.Name} {src.Author.Surname}"));

            // CreateMap<AuthorsViewModel, Author>()
            //     .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));










        }
    }
}