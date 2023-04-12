using FirstAPI.Dtos.CategoryDtos;
using FirstAPI.Dtos.ProductDtos;
using FirstAPI.Models;

namespace FirstAPI.Profile
{
    public class MapProfile : AutoMapper.Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryReturnDto>()
                .ForMember(c => c.ImageUrl, map => map.MapFrom(c => "https://localhost:7232/img/" + c.ImageUrl));

            CreateMap<Product, ProductReturnDto>()
                .ForMember(p=>p.Profit,map=>map.MapFrom(p=>p.SalePrice-p.Price));
            CreateMap<Category, CategoryForProductReturnDto>();
            CreateMap<Category, CategoryInProductListItemDto>();

            CreateMap<Product, ProductListItemDto>();
        }
    }
}
