using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace bsStoreApp.Utilities.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookDtoForUpdate, Book>().ReverseMap();
        CreateMap<Book, BookDto>();
        CreateMap<BookDtoForInsertion, Book>();
        CreateMap<BookDto, BookDtoForUpdate>();
        CreateMap<UserForRegistrationDto, User>();
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryDtoForInsertion, Category>();
        CreateMap<CategoryDtoForUpdate, Category>();
    }
}
