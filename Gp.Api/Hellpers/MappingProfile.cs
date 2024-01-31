using AutoMapper;
using Gp.Api.Dtos;
using GP.Core.Entities;
using GP.Repository.Data.Migrations;
namespace Gp.Api.Hellpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Trip, TripToDto>()
                 .ForMember(pd => pd.FromCityName, o => o.MapFrom(T => T.FromCity.NameOfCity))
                   .ForMember(pd => pd.ToCityName, o => o.MapFrom(T => T.ToCity.NameOfCity))
                   .ForMember(pd => pd.CountryIdFrom, o => o.MapFrom(T => T.FromCity.CountryId))
                    .ForMember(pd => pd.CountryIdTo, o => o.MapFrom(T => T.ToCity.CountryId))
                    .ForMember(pd => pd.CountryNameFrom, o => o.MapFrom(T => T.FromCity.Country.NameCountry))
            .ForMember(pd => pd.CountryNameTo, o => o.MapFrom(T => T.ToCity.Country.NameCountry))
                //.ForMember(pd=>pd.CountryNameFrom,o=>o.MapFrom(T=>T.FromCity.CountryName))
                //.ForMember(pd => pd.CountryNameTo, o => o.MapFrom(T => T.ToCity.CountryName))

                .ReverseMap();

            CreateMap<CreateTripDto, Trip>().ForMember(dest => dest.FromCity, opt => opt.MapFrom(src => src.FromCityName))
            .ForMember(dest => dest.ToCity, opt => opt.MapFrom(src => src.ToCityName));
                  
           
        }
    }
}
