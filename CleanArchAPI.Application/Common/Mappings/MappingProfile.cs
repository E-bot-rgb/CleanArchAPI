using AutoMapper;
using CleanArchAPI.Application.Common.DTOs;
using CleanArchAPI.Domain.Entities;

namespace CleanArchAPI.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name));
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.ProductCount, o => o.MapFrom(s => s.Products.Count));
        CreateMap<CreateCategoryDto, Category>();
    }
}