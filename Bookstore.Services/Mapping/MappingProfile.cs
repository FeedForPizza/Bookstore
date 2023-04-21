using AutoMapper;
using Bookstore.Entities;
using Bookstore.Services.DTO.Authors;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.CreateBook;
using Bookstore.Services.DTO.Customer;
using Bookstore.Services.DTO.Genres;
using Bookstore.Services.DTO.Languages;
using Bookstore.Services.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bookstore.Entities.Book, BookDTO>().ReverseMap();
            CreateMap<Entities.Book, BookSummaryDTO>();
            CreateMap<Author, AuthorsDTO>().ForMember(x => x.FullName, x =>
           x.MapFrom(y => $"{y.FirstName} {y.LastName}"));
            CreateMap<Genre, GenresDTO>();
            CreateMap<Language, LanguageDTO>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateBookDto,Entities.Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }
    }
}
