using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using GP.core.Entities.identity;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using Gp.Api.Hellpers;
using Microsoft.Extensions.Configuration;

namespace Gp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequsetController : ApiBaseController
    {
        private readonly IConfiguration configuration;
        private readonly IRequestRepository requestRepository;
        private readonly IGenericRepositroy<Shipment> shipmentRepo;
        private readonly IGenericRepositroy<Trip> tripRepo;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
     

        public RequsetController(IConfiguration configuration,IRequestRepository requestRepository, IGenericRepositroy<Shipment> ShipmentRepo, IGenericRepositroy<Trip> tripRepo, IMapper mapper, UserManager<AppUser> userManager) 
        {
            this.configuration = configuration;
            this.requestRepository = requestRepository;
            shipmentRepo = ShipmentRepo;
            this.tripRepo = tripRepo;
            this.mapper = mapper;
            this.userManager = userManager;
        }
       
        [HttpGet]

         public async Task<ActionResult<RequestDto>> GetRequestShipemtWTrip(int Id)
        {
         
            var request = await requestRepository.GetRequestAsync(Id);

            if (request == null)
            {
                new Request(Id);
                    
             }
           
            // جلب بيانات الشحنة
            var specShipment = new ShipmentSpecification(request.ShipmentId);
            var shipment = await shipmentRepo.GetByIdwithSpecAsyn(specShipment);

            var spec = new TripSpecifications(request.TripId);
            var trip = await tripRepo.GetByIdwithSpecAsyn(spec);
            var productPictureUrlResolver = new ProductPictureUrlResolver(configuration);
            // تحديث كائن الطلب ليتضمن بيانات الشحنة والرحلة
            request.Shipment = shipment;
            request.Trip = trip;

            // إنشاء كائن RequestDto من بيانات الطلب والشحنة والرحلة
            var user = await userManager.FindByIdAsync(shipment.IdentityUserId);
            var userTrip = await userManager.FindByIdAsync(trip.UserId);
            var requestDto = new RequestDto
            {
                Id = Id,
                ShipmentToDto = new ShipmentToDto
                {
                    Id= shipment.Id,
                    Reward = shipment.Reward,
                    Weight = shipment.Weight,
                    FromCityID = shipment.FromCityID,
                    FromCityName=shipment.FromCity.NameOfCity,
                    CountryIdFrom=shipment.FromCity.Country.Id,
                    CountryNameFrom=shipment.FromCity.Country.NameCountry,
                    ToCityId = shipment.ToCityId,
                    ToCityName = shipment.ToCity.NameOfCity,
                    CountryIdTo = shipment.ToCity.Country.Id,
                    CountryNameTo = shipment.ToCity.Country.NameCountry,
                    DateOfRecieving =shipment.DateOfRecieving,
                    Address = shipment.Address,
                    ProductId=shipment.Products.Select(t=>t.Id).FirstOrDefault(),
                    ProductName=shipment.Products.Select(t => t.ProductName).FirstOrDefault(),
                    ProductPrice= shipment.Products.Select(t => t.ProductPrice).FirstOrDefault(),
                    ProductWeight= shipment.Products.Select(t => t.ProductWeight).FirstOrDefault(),
                    PictureUrl = productPictureUrlResolver.Resolve(shipment, null, null, null),
                    CategoryId=shipment.Products.Select(p=>p.Category.Id).FirstOrDefault(),
                    CategoryName=shipment.Products.Select(p=>p.Category.TypeName).FirstOrDefault(),
                    UserId=shipment.IdentityUserId,
                    UserName = user.UserName,



                },
                TripToDto = new TripToDto
                {
                   Id= trip.Id,
                   FromCityID= trip.FromCityID,
                   FromCityName = trip.FromCity.NameOfCity,
                   CountryIdFrom=trip.FromCity.Country.Id,
                   CountryNameFrom=trip.FromCity.Country.NameCountry,
                   availableKg=trip.availableKg,
                   ToCityId=trip.ToCity.Id,
                   ToCityName=trip.ToCity.NameOfCity,
                   CountryIdTo=trip.ToCity.Country.Id,
                   CountryNameTo=trip.ToCity.Country.NameCountry,
                   arrivalTime=trip.arrivalTime,
                   dateofCreation=trip.DateofCreation,
                   UserId=trip.UserId,
                   UserName=userTrip.UserName,
                }
            };

            return Ok(requestDto); // أو يمكنك استخدام Ok إذا كان الطلب ناجحًا


        }
        [Authorize]
        [HttpPost]//post:api/request
        public async Task<ActionResult<Request>> UpdateRequest(RequestDto request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);


            var existingUser = await userManager.FindByEmailAsync(email);
            if (request.ShipmentToDto.UserId == existingUser.Id || request.TripToDto.UserId == existingUser.Id)
            {
                if (request.ShipmentToDto.UserId != request.TripToDto.UserId)
                {



                    var mappedRequest = mapper.Map<RequestDto, Request>(request);
                    mappedRequest.UserId = existingUser?.Id;
                    // تنسيق TripId في mappedRequest
                    mappedRequest.TripId = request.TripToDto.Id;
                    mappedRequest.ShipmentId = request.ShipmentToDto.Id;
                    var CreatedOrUpdatedRequest = await requestRepository.UpdateRequestAsync(mappedRequest);
                    if (CreatedOrUpdatedRequest is null) return BadRequest(new ApiResponse(400));

                    return Ok(CreatedOrUpdatedRequest);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Cannot create request for same user's trip or shipment."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Cannot create request for another user's trip or shipment."));
            }


        }
        [HttpDelete]//delete :api/request

        public async Task<ActionResult<bool>> DeleteRequest(int id)
        {
            return await requestRepository.DeleteRequestAsync(id);
        }






    }
}
