using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gp.Api.Controllers
{
   
    public class TripController : ApiBaseController
    {

    


        private readonly IGenericRepositroy<Trip> tripRepo;
        private readonly IMapper mapper;
        private readonly INameToIdResolver nameToIdResolver;
        private readonly ICountryRepository countryRepository;
        private readonly ICityRepository cityRepository;

        public TripController(IGenericRepositroy<Trip> TripRepo, IMapper mapper,INameToIdResolver nameToIdResolver,ICountryRepository countryRepository,ICityRepository cityRepository)
        {
            tripRepo = TripRepo;
            this.mapper = mapper;
            this.nameToIdResolver = nameToIdResolver;
            this.countryRepository = countryRepository;
            this.cityRepository = cityRepository;
        }
        [ProducesResponseType(typeof(TripToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripToDto>>> GetTrips()
        {
            

            var spec = new TripSpecifications();
            var Trips = await tripRepo.GetAllWithSpecAsyn(spec);

            if (Trips is null) return NotFound(new ApiResponse(404));
            var mappedTrip = mapper.Map<IEnumerable<Trip>, IEnumerable<TripToDto>>(Trips);

            return Ok(mappedTrip);
        }
        [ProducesResponseType(typeof(TripToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<TripToDto>> GetTrip(int id)
        {
            var spec = new TripSpecifications(id);
            var trip = await tripRepo.GetByIdwithSpecAsyn(spec);
            if (trip is null) return NotFound(new ApiResponse(404));
            var mappedTrip = mapper.Map<Trip, TripToDto>(trip);
            return Ok(mappedTrip);
        }
       // [HttpPost]
       //public async Task<ActionResult<TripToDto>> CreateTrip(TripToDto createTripDto)
       // {
       //     if(ModelState.IsValid)
                
       //     {
       //         var mappedTrip = mapper.Map<TripToDto, Trip>(createTripDto);
       //         await tripRepo.AddAsyn(mappedTrip);
       //     }
       //     return
       // }
        [HttpPost("CreateTrip")]
        public async Task<ActionResult<Trip>> CreateTrip( TripToDto tripCreateDto)
        {
            if (ModelState.IsValid)
            {
                // Check if the FromCity exists in the database
                var fromCity = await cityRepository.GetCityByNameAsync(tripCreateDto.FromCityName);

                if (fromCity == null)
                {
                    // If not exists, create and add it to the database
                    fromCity = new City { NameOfCity = tripCreateDto.FromCityName };
                    await cityRepository.AddAsync(fromCity);
                }

                // Repeat the same process for ToCity
                var toCity = await cityRepository.GetCityByNameAsync(tripCreateDto.ToCityName);
                if (toCity == null)
                {
                    toCity = new City { NameOfCity = tripCreateDto.ToCityName };
                    await cityRepository.AddAsync(toCity);
                }

                // Check if the FromCountry exists in the database
                var fromCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameFrom);
                if (fromCountry == null)
                {
                    fromCountry = new Country { NameCountry = tripCreateDto.CountryNameFrom };
                    await countryRepository.AddAsync(fromCountry);
                }

                // Repeat the same process for ToCountry
                var toCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameTo);
                if (toCountry == null)
                {
                    toCountry = new Country { NameCountry = tripCreateDto.CountryNameTo };
                    await countryRepository.AddAsync(toCountry);
                }

                var mappedTrip = mapper.Map<TripToDto, Trip>(tripCreateDto);
                await tripRepo.AddAsync(mappedTrip);
               // var tripId = mappedTrip.Id;
                return Ok("Trip Created Successfully");
            }

            // Model state is not valid
            return BadRequest(ModelState);
        }

    }
}
