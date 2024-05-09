using App.ViewModels;
using AutoMapper;
using Business.Models;
using System.Net;

namespace App.AutoMapper
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<OrderItem, OrderItemViewModel>().ReverseMap();
            CreateMap<Order, OrderViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();  
            CreateMap<Post, PostViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
        }
    }
}
