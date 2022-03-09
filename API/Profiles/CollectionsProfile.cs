using AutoMapper;
using CashGen.Entities;
using CashGen.Models;


namespace CashGen.API.Profiles
{
    public class CollectionsProfile : Profile
    {
        public CollectionsProfile() => this.CreateMap<Collection, CollectionListDto>();
    }
}
