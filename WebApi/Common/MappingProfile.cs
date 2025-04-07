using AutoMapper;
using WebApi.Application.GenreOperations.Query;
using WebApi.BookOperations.Query;
using WebApi.Entities;
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

            CreateMap<Book, BookDetailViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<Book, BooksViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

            CreateMap<Genre, GenresViewModel>();
            CreateMap<Genre, GenreDetailViewModel>();





        }
    }
}