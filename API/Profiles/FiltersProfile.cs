using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class FiltersProfile : Profile
    {
        public FiltersProfile()
        {
            this.CreateMap<Filter, FilterDto>();
            this.CreateMap<FilterForCreationDto, Filter>();
            this.CreateMap<FilterForUpdateDto, Filter>();
        }
    }
}
