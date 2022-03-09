using AutoMapper;
using CashGen.Entities;
using CashGen.Models;


namespace CashGen.API.Profiles
{
    public class ChatsProfile : Profile
    {
        public ChatsProfile()
        {
            this.CreateMap<Chat, ChatDto>();
            this.CreateMap<Chat, ChatListDto>();
            this.CreateMap<ChatForCreationDto, Chat>();
        }
    }
}
