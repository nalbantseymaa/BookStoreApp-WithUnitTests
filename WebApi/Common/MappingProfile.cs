using AutoMapper;
using WebApi.BookOperations.Query;
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

            CreateMap<Book, BookDetailViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()));

            CreateMap<Book, BooksViewModel>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ((GenreEnum)src.GenreId).ToString()));




        }
    }
}