using App.ViewModels;
using AutoMapper;
using Business.Models;

namespace App.AutoMapper
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig() 
        { 
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
        
        }
    }
}
