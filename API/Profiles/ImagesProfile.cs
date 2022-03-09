using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class ImagesProfile : Profile
    {
        public ImagesProfile()
        {
            this.CreateMap<Image, ImageDto>();
            this.CreateMap<ImageForCreationDto, Image>();
        }
    }
}
