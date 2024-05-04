using AutoMapper;
using ExampleRestAPI.DTOs;
using NorthwindData.Models;

namespace ExampleRestAPI
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
        }
    }
}
