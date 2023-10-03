using AutoMapper;
using OnlineStore.API.DTOs;
using OnlineStore.Domain.Entites;

namespace OnlineStore.API.AutoMapper;
public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Product, ProductDTO>().ReverseMap();
        CreateMap<Order, OrderDTO>().ReverseMap();
    }
}
