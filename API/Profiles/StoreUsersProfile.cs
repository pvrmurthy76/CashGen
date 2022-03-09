using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class StoreUsersProfile : Profile
    {
        public StoreUsersProfile() => this.CreateMap<StoreUserForCreationDto, StoreUser>();
    }
}
