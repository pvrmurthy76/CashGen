using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            this.CreateMap<Order, OrderDto>();
            this.CreateMap<Order, OrderListDto>();
            this.CreateMap<OrderForCreationDto, Order>();
            this.CreateMap<OrderForUpdateDto, Order>();
            this.CreateMap<OrderFraudUpdateDto, Order>();
        }
    }
}
