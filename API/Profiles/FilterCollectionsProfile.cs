using AutoMapper;
using CashGen.Entities;
using CashGen.Models;


namespace CashGen.API.Profiles
{
    public class FilterCollectionsProfile : Profile
    {
        public FilterCollectionsProfile()
        {
            this.CreateMap<FilterCollection, FilterCollectionDto>();
            this.CreateMap<FilterCollectionForCreationDto, FilterCollection>();
        }
    }
}
