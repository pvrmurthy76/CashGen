using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class ProductFiltersProfile : Profile
    {
        public ProductFiltersProfile()
        {
            this.CreateMap<ProductFilter, ProductFilterDto>();
            this.CreateMap<ProductFilterForCreationDto, ProductFilter>();
        }
    }
}
