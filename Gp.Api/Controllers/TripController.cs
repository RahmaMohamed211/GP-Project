using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using Gp.Api.Hellpers;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Talabat.core.Sepecifitction;

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
        public async Task<ActionResult<Pagination<TripToDto>>> GetTrips([FromQuery]TripwShSpecParams tripwShSpec)
         {
            

            var spec = new TripSpecifications(tripwShSpec);
            var Trips = await tripRepo.GetAllWithSpecAsyn(spec);

            if (Trips is null) return NotFound(new ApiResponse(404));
            var data = mapper.Map<IEnumerable<Trip>, IEnumerable<TripToDto>>(Trips);
            var countSpec = new TripWithFilterForCountSpecification(tripwShSpec);
            var Count =await tripRepo.GetCountWithSpecAsync(countSpec); 
            return Ok(new Pagination<TripToDto>(tripwShSpec.PageIndex,tripwShSpec.PageSize,Count,data));
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

                // استرجاع كائن المدينة المطابق لاسم المدينة المدخل
                var fromCity = await cityRepository.GetCityByNameAsync(tripCreateDto.FromCityName);
                // استرجاع كائن البلد المطابق لاسم البلد المدخل
                var fromCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameFrom);
                // استرجاع كائن المدينة المطابق لاسم المدينة المدخل
                var toCity = await cityRepository.GetCityByNameAsync(tripCreateDto.ToCityName);
                // استرجاع كائن البلد المطابق لاسم البلد المدخل
                var toCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameTo);

                // تحقق مما إذا كانت البيانات المطابقة موجودة
                if (fromCity != null && fromCountry != null && toCity != null && toCountry != null)
                {
                    // تعيين المعرّفات لكائن الرحلة
                    var mappedTrip = mapper.Map<TripToDto, Trip>(tripCreateDto);
                    mappedTrip.FromCityID = fromCity.Id;
                    mappedTrip.FromCity = fromCity;
                    mappedTrip.FromCity.CountryId = fromCountry.Id;

                    mappedTrip.ToCityId = toCity.Id;
                    mappedTrip.ToCity = toCity;
                    mappedTrip.ToCity.CountryId = toCountry.Id;

                    await tripRepo.AddAsync(mappedTrip);
                    // var tripId = mappedTrip.Id;
                    return Ok("Trip Created Successfully");
                }
                  else
        {
            // إذا لم يتم العثور على المدينة أو البلد المطابقين، يتم إرجاع رسالة خطأ
            return NotFound("City or country not found");
        }
                
            }

            // Model state is not valid
            return BadRequest(ModelState);
        }

    }
}
