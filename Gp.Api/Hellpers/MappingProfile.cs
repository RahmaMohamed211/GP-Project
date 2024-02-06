using AutoMapper;
using Gp.Api.Dtos;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Repository.Data.Migrations;
namespace Gp.Api.Hellpers
{
    public class MappingProfile:Profile
    {
        private readonly INameToIdResolver nameToIdResolver;
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


            CreateMap<Shipment, ShipmentToDto>()
               .ForMember(pd => pd.FromCityName, o => o.MapFrom(T => T.FromCity.NameOfCity))
                 .ForMember(pd => pd.ToCityName, o => o.MapFrom(T => T.ToCity.NameOfCity))
                 .ForMember(pd => pd.CountryIdFrom, o => o.MapFrom(T => T.FromCity.CountryId))
                  .ForMember(pd => pd.CountryIdTo, o => o.MapFrom(T => T.ToCity.CountryId))
                  .ForMember(pd => pd.CountryNameFrom, o => o.MapFrom(T => T.FromCity.Country.NameCountry))
          .ForMember(pd => pd.CountryNameTo, o => o.MapFrom(T => T.ToCity.Country.NameCountry))
   .ForMember(pd => pd.ProductId, o => o.MapFrom(T => T.Products.Select(p => p.Id).FirstOrDefault()))
    .ForMember(pd => pd.ProductName, o => o.MapFrom(T => T.Products.Select(p => p.ProductName).FirstOrDefault()))
    .ForMember(pd => pd.ProductPrice, o => o.MapFrom(T => T.Products.Select(p => p.ProductPrice).FirstOrDefault()))
    .ForMember(pd => pd.ProductWeight, o => o.MapFrom(T => T.Products.Select(p => p.ProductWeight).FirstOrDefault()))
    .ForMember(pd => pd.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>())
    .ForMember(pd => pd.CategoryId, o => o.MapFrom(T => T.Products.Select(p => p.CategoryId).FirstOrDefault()))
//.ForMember(pd => pd.CategoryName, o => o.MapFrom(T => T.Products.Select(p => p.Category.TypeName).DefaultIfEmpty("DefaultValue").FirstOrDefault()))

              .ReverseMap();

            CreateMap<Category, CategoriesToDto>()
                .ForMember(c=>c.CategoryName, o => o.MapFrom(T => T.TypeName));

            CreateMap<City, CityToDto>()
             .ForMember(c => c.CityName, o => o.MapFrom(T => T.NameOfCity))
             .ForMember(c => c.CountryId, o => o.MapFrom(T => T.CountryId))
             .ForMember(c=>c.CountryName,o=>o.MapFrom(T=>T.Country.NameCountry));
              

             

        }
        // public MappingProfile(INameToIdResolver nameToIdResolver)
        // {
        //     this.nameToIdResolver = nameToIdResolver;
        //     CreateMap<CreateTripDto, Trip>()
        //   .ForPath(dest => dest.FromCity.NameOfCity, opt => opt.MapFrom(src => src.FromCityName))
        //    .ForPath(dest => dest.ToCity.NameOfCity, opt => opt.MapFrom(src => src.ToCityName))
        //     .ForPath(dest => dest.FromCity.Country.NameCountry, opt => opt.MapFrom(src => src.CountryNameFrom))
        //     .ForPath(dest => dest.ToCity.Country.NameCountry, opt => opt.MapFrom(src => src.CountryNameTo))
        //     .ForMember(dest => dest.FromCityID, opt => opt.MapFrom(src => nameToIdResolver.ResolveCityIdByNameAsync(src.FromCityName)))
        //.ForMember(dest => dest.ToCityId, opt => opt.MapFrom(src => nameToIdResolver.ResolveCityIdByNameAsync(src.ToCityName)))
        //.ForMember(dest => dest.FromCity.Country.NameCountry, opt => opt.MapFrom(src => nameToIdResolver.ResolveCountryIdByNameAsync(src.CountryNameFrom)))
        //.ForMember(dest => dest.ToCity.Country.NameCountry, opt => opt.MapFrom(src => nameToIdResolver.ResolveCountryIdByNameAsync(src.CountryNameTo)));


        // }

      


    }
}
