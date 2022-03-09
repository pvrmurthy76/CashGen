using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class TransactionsProfile : Profile
    {
        public TransactionsProfile() => this.CreateMap<TransactionForCreationDto, Transaction>();
    }
}
