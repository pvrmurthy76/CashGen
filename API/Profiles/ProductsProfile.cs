using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            this.CreateMap<Product, ProductDto>();
            this.CreateMap<Product, ProductListDto>();
            this.CreateMap<ProductForCreationDto, Product>();
            this.CreateMap<ProductForUpdateDto, Product>();
        }
    }
}
