using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using Gp.Api.Hellpers;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using GP.core.Entities.identity;
using GP.core.Sepecifitction;

namespace Gp.Api.Controllers
{
   
    public class TripController : ApiBaseController
    {

    


        private readonly IGenericRepositroy<Trip> tripRepo;
        private readonly IMapper mapper;
        private readonly INameToIdResolver nameToIdResolver;
        private readonly ICountryRepository countryRepository;
        private readonly ICityRepository cityRepository;
        private readonly UserManager<AppUser> userManager;

        public TripController(IGenericRepositroy<Trip> TripRepo, IMapper mapper,INameToIdResolver nameToIdResolver,ICountryRepository countryRepository,ICityRepository cityRepository, UserManager<AppUser> userManager)
        {
            tripRepo = TripRepo;
            this.mapper = mapper;
            this.nameToIdResolver = nameToIdResolver;
            this.countryRepository = countryRepository;
            this.cityRepository = cityRepository;
            this.userManager = userManager;
        }
        [Authorize]
        [ProducesResponseType(typeof(TripToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<Pagination<TripToDto>>> GetTrips([FromQuery]TripwShSpecParams tripwShSpec)
         {
            

            var spec = new TripSpecifications(tripwShSpec);
            var Trips = await tripRepo.GetAllWithSpecAsyn(spec);

            if (Trips is null) return NotFound(new ApiResponse(404));
            var tripDtos = new List<TripToDto>();
            foreach (var trip in Trips)
            {
               
                var tripDto = mapper.Map<Trip, TripToDto>(trip);

                // استخدم UserManager للبحث عن اسم المستخدم باستخدام معرّف المستخدم
                var user = await userManager.FindByIdAsync(trip.UserId);
                if (user != null)
                {
                    tripDto.UserName = user.DisplayName; // افترضت هنا أن DisplayName هو الخاصية التي تحمل اسم المستخدم
                }
                else
                {
                    tripDto.UserName = "Unknown"; // إذا لم يتم العثور على المستخدم
                }

                tripDtos.Add(tripDto);
            }
            
            var countSpec = new TripWithFilterForCountSpecification(tripwShSpec);
            var Count =await tripRepo.GetCountWithSpecAsync(countSpec); 
            return Ok(new Pagination<TripToDto>(tripwShSpec.PageIndex,tripwShSpec.PageSize,Count, tripDtos));
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
            var user = await userManager.FindByIdAsync(trip.UserId);
            if (user != null)
            {
                mappedTrip.UserName = user.DisplayName; // افترضت هنا أن DisplayName هو الخاصية التي تحمل اسم المستخدم
            }
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
        [Authorize]
        [HttpPost("CreateTrip")]
        public async Task<ActionResult<Trip>> CreateTrip(TripToDto tripCreateDto)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);


                var existingUser = await userManager.FindByEmailAsync(email);


                // استرجاع كائن المدينة المطابق لاسم المدينة المدخل
                var fromCity = await cityRepository.GetCityByNameAsync(tripCreateDto.FromCityName);
                // استرجاع كائن البلد المطابق لاسم البلد المدخل
                var fromCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameFrom);
                // استرجاع كائن المدينة المطابق لاسم المدينة المدخل
                var toCity = await cityRepository.GetCityByNameAsync(tripCreateDto.ToCityName);
                // استرجاع كائن البلد المطابق لاسم البلد المدخل
                var toCountry = await countryRepository.GetCountryByNameAsync(tripCreateDto.CountryNameTo);

                // تحقق مما إذا كانت البيانات المطابقة موجودة
                if (fromCity != null && fromCountry != null && toCity != null && toCountry != null&& existingUser!=null)
                {
                  
                    var mappedTrip = mapper.Map<TripToDto, Trip>(tripCreateDto);
                    mappedTrip.FromCityID = fromCity.Id;
                    mappedTrip.FromCity = fromCity;
                    mappedTrip.FromCity.CountryId = fromCountry.Id;

                    mappedTrip.ToCityId = toCity.Id;
                    mappedTrip.ToCity = toCity;
                    mappedTrip.ToCity.CountryId = toCountry.Id;

                   

                    mappedTrip.UserId = existingUser?.Id;

                    await tripRepo.AddAsync(mappedTrip);

                    await tripRepo.SaveChangesAsync();
                    return Ok(new { message = "Trip Created Successfully" });
                }
                else
                {
                    return NotFound(new { message = "City or country not found" });
                }
            }

            return BadRequest(ModelState);
        }


    }
}
