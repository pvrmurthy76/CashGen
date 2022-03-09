using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class StoresProfile : Profile
    {
        public StoresProfile()
        {
            this.CreateMap<Store, StoreDto>();
            this.CreateMap<Store, StoreListDto>();
            this.CreateMap<Store, AccountListDto>();
            this.CreateMap<StoreForCreationDto, Store>();
            this.CreateMap<StoreForUpdateDto, Store>();
        }
    }
}
