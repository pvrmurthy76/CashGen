using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class FilterOptionsProfile : Profile
    {
        public FilterOptionsProfile()
        {
            this.CreateMap<FilterOption, FilterOptionDto>();
            this.CreateMap<FilterOptionForCreationDto, FilterOption>();
        }
    }
}
