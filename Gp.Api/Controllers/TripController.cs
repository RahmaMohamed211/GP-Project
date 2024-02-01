using AutoMapper;
using Gp.Api.Dtos;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TripToDto>>> GetTrips()
        {
            //var trip= await tripRepo.GetAllAsync();

            var spec = new TripSpecifications();
            var Trips = await tripRepo.GetAllWithSpecAsyn(spec);

            var mappedTrip = mapper.Map<IEnumerable<Trip>, IEnumerable<TripToDto>>(Trips);

            return Ok(mappedTrip);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<TripToDto>> GetTrip(int id)
        {
            var spec = new TripSpecifications(id);
            var trip = await tripRepo.GetByIdwithSpecAsyn(spec);
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
        public async Task<ActionResult> CreateTrip(CreateTripDto createTripDto)
        {
            if (ModelState.IsValid)
            {

                var mappedTrip = mapper.Map<CreateTripDto, Trip>(createTripDto);
                await tripRepo.AddAsyn(mappedTrip);

                var tripToDto = mapper.Map<Trip, TripToDto>(mappedTrip);

                return CreatedAtAction(nameof(GetTrips), new { id=tripToDto.Id } ,tripToDto);
            }

            // Model state is not valid
            return BadRequest(ModelState);
        }

    }
}
