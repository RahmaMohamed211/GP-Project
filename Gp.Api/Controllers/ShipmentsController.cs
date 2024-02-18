using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using Gp.Api.Hellpers;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.core.Sepecifitction;

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

        public ShipmentsController(IGenericRepositroy<Shipment> ShipmentRepo,IMapper mapper, ICountryRepository countryRepository, ICityRepository cityRepository,ICategoryRepository categoryRepository,IGenericRepositroy<Product> productRepo)
        {
            shipmentRepo = ShipmentRepo;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.cityRepository = cityRepository;
            this.categoryRepository = categoryRepository;
            this.productRepo = productRepo;
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
            var data = mapper.Map<IEnumerable<Shipment>, IEnumerable<ShipmentToDto>>(shipments);
            var countSpec = new shipmentsWithFilterForCountSpecification(tripwShSpec);
            var Count = await shipmentRepo.GetCountWithSpecAsync(countSpec);
            return Ok(new Pagination<ShipmentToDto>(tripwShSpec.PageIndex, tripwShSpec.PageSize, Count, data));
            
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
            return Ok(mappedshipment);
        }

        [HttpPost("CreateShipment")]
        public async Task<ActionResult<Shipment>> CreateShipment([FromForm]ShipmentToDto shipmentCreateDto, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                // استرجاع المدينة والبلد للشحنة من المستودعات
                var fromCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.FromCityName);
                var fromCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameFrom);
                var toCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.ToCityName);
                var toCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameTo);
                var category2 = await categoryRepository.GetCategoryByNameAsync(shipmentCreateDto.CategoryName);
                // التحقق من وجود المدينة والبلد
                if (fromCity != null && fromCountry != null && toCity != null && toCountry != null && category2 != null)
                {
                    // تعيين معرفات المدينة والبلد للشحنة
                    var mappedTrip = mapper.Map<ShipmentToDto, Shipment>(shipmentCreateDto);

                    mappedTrip.FromCityID = fromCity.Id;
                    mappedTrip.FromCity = fromCity;
                    mappedTrip.FromCity.CountryId = fromCountry.Id;
                    mappedTrip.ToCityId = toCity.Id;
                    mappedTrip.ToCity = toCity;
                    mappedTrip.ToCity.CountryId = toCountry.Id;
                 
                    mappedTrip.CategoryId = category2.Id;
                    mappedTrip.Category = category2;

                    if (image != null && image.Length > 0)
                    {
                        var productImageUrl = DocumentSetting.UploadImage(image, "products");
                        if (!string.IsNullOrEmpty(productImageUrl))
                        {
                            shipmentCreateDto.PictureUrl = productImageUrl; // تخزين العنوان URL في خاصية PictureUrl
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

                    return Ok("Shipment Created Successfully");
                }
                else
                {
                    return NotFound("City or country not found");
                }
            }

            // Model state is not valid
            return BadRequest(ModelState);
        }



    }
}
