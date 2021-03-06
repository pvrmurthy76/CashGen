using AutoMapper;
using CashGen.Entities;
using CashGen.Models;

namespace CashGen.API.Profiles
{
    public class FeaturesProfile : Profile
    {
        public FeaturesProfile()
        {
            this.CreateMap<Feature, FeatureDto>();
            this.CreateMap<FeatureForCreationDto, Feature>();
        }
    }
}
