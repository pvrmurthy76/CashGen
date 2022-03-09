using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class LineItemsProfile : Profile
    {
        public LineItemsProfile()
        {
            this.CreateMap<LineItem, LineItemDto>();
            this.CreateMap<LineItemForCreationDto, LineItem>();
        }
    }
}
