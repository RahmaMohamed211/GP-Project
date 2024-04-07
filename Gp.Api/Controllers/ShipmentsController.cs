using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using Gp.Api.Hellpers;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GP.core.Entities.identity;
using GP.core.Sepecifitction;

namespace Gp.Api.Controllers
{
  
    public class ShipmentsController : ApiBaseController
    {
        private readonly IGenericRepositroy<Shipment> shipmentRepo;
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly ICityRepository cityRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IGenericRepositroy<Product> productRepo;
        private readonly UserManager<AppUser> userManager;

        public ShipmentsController(IGenericRepositroy<Shipment> ShipmentRepo,IMapper mapper, ICountryRepository countryRepository, ICityRepository cityRepository,ICategoryRepository categoryRepository,IGenericRepositroy<Product> productRepo, UserManager<AppUser> userManager)
        {
            shipmentRepo = ShipmentRepo;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.cityRepository = cityRepository;
            this.categoryRepository = categoryRepository;
            this.productRepo = productRepo;
            this.userManager = userManager;
        }
        [ProducesResponseType(typeof(ShipmentToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<ShipmentToDto>>> GetShipments([FromQuery] TripwShSpecParams tripwShSpec)
        {
            //var trip= await tripRepo.GetAllAsync();

            var spec = new ShipmentSpecification(tripwShSpec);
            var shipments = await shipmentRepo.GetAllWithSpecAsyn(spec);

            if (shipments is null) return NotFound(new ApiResponse(404));
            var shipmentDtos = new List<ShipmentToDto>();
            foreach (var shipment in shipments)
            {
                var shipmentDto = mapper.Map<Shipment, ShipmentToDto>(shipment);

                // استخدم UserManager للبحث عن اسم المستخدم باستخدام معرّف المستخدم
                var user = await userManager.FindByIdAsync(shipment.IdentityUserId);
                if (user != null)
                {
                    shipmentDto.UserName = user.DisplayName; // افترضت هنا أن DisplayName هو الخاصية التي تحمل اسم المستخدم
                }
                else
                {
                    shipmentDto.UserName = "Unknown"; // إذا لم يتم العثور على المستخدم
                }

                shipmentDtos.Add(shipmentDto);
            }
            //var data = mapper.Map<IEnumerable<Shipment>, IEnumerable<ShipmentToDto>>(shipments);
            var countSpec = new shipmentsWithFilterForCountSpecification(tripwShSpec);
            var Count = await shipmentRepo.GetCountWithSpecAsync(countSpec);
            return Ok(new Pagination<ShipmentToDto>(tripwShSpec.PageIndex, tripwShSpec.PageSize, Count, shipmentDtos));
            
        }
        [ProducesResponseType(typeof(ShipmentToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<ShipmentToDto>> GetShipment(int id)
        {
            var spec = new ShipmentSpecification(id);
            var shipment = await shipmentRepo.GetByIdwithSpecAsyn(spec);

            if (shipment is null) return NotFound(new ApiResponse(404));
            var mappedshipment = mapper.Map<Shipment, ShipmentToDto>(shipment);
            var user = await userManager.FindByIdAsync(shipment.IdentityUserId);
            if (user != null)
            {
                mappedshipment.UserName = user.DisplayName; // افترضت هنا أن DisplayName هو الخاصية التي تحمل اسم المستخدم
            }
            return Ok(mappedshipment);
        }
        [Authorize]
        [HttpPost("CreateShipment")]
        public async Task<ActionResult<Shipment>> CreateShipment ([FromForm]ShipmentToDto shipmentCreateDto, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var email = User.FindFirstValue(ClaimTypes.Email);

           
                var existingUser = await userManager.FindByEmailAsync(email);

              
                var fromCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.FromCityName);
                var fromCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameFrom);
                var toCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.ToCityName);
                var toCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameTo);
                var category2 = await categoryRepository.GetCategoryByNameAsync(shipmentCreateDto.CategoryName);
                
                if (fromCity != null && fromCountry != null && toCity != null && toCountry != null && category2 != null&& existingUser!=null)
                {
                   
                    var mappedTrip = mapper.Map<ShipmentToDto, Shipment>(shipmentCreateDto);

                    mappedTrip.FromCityID = fromCity.Id;
                    mappedTrip.FromCity = fromCity;
                    mappedTrip.FromCity.CountryId = fromCountry.Id;
                    mappedTrip.ToCityId = toCity.Id;
                    mappedTrip.ToCity = toCity;
                    mappedTrip.ToCity.CountryId = toCountry.Id;
                 
                    mappedTrip.CategoryId = category2.Id;
                    mappedTrip.Category = category2;

                   

                       mappedTrip.IdentityUserId = existingUser?.Id;
                    // mappedTrip.User = existingUser;
                   // mappedTrip.IdentityUserId = existingUser.UserName;


                    if (image != null && image.Length > 0)
                    {
                        var productImageUrl = DocumentSetting.UploadImage(image, "products");
                        if (!string.IsNullOrEmpty(productImageUrl))
                        {
                            shipmentCreateDto.PictureUrl = productImageUrl;
                        }


                        var newProduct = new Product
                        {
                            ProductName = shipmentCreateDto.ProductName,
                            ProductPrice = shipmentCreateDto.ProductPrice,
                            ProductWeight = shipmentCreateDto.ProductWeight,
                            PictureUrl = productImageUrl,
                            CategoryId = category2.Id,
                        };

                        mappedTrip.Products.Add(newProduct);

                    }

                    await shipmentRepo.AddAsync(mappedTrip);

                    await shipmentRepo.SaveChangesAsync();

                    return Ok(new { message = "shipment Created Successfully" });


                }
                else
                {
                    return NotFound(new { message = "City or country not found" });
                }
            }

            // Model state is not valid
            return BadRequest(ModelState);
        }



    }
}
