using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile()
        {
            this.CreateMap<User, UserDto>();
            this.CreateMap<User, UserListDto>();
            this.CreateMap<UserForCreationDto, User>();
            this.CreateMap<UserForUpdateDto, User>();
            this.CreateMap<UserPasswordDto, User>();
            this.CreateMap<UserResetDto, User>();
        }
    }
}
