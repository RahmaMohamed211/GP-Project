using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using GP.Repository;
using GP.Repository.Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gp.Api.Controllers
{
  
    public class ShipmentsController : ApiBaseController
    {
        private readonly IGenericRepositroy<Shipment> shipmentRepo;
        private readonly IMapper mapper;
        private readonly ICountryRepository countryRepository;
        private readonly ICityRepository cityRepository;
        private readonly IGenericRepositroy<Product> productRepo;

        public ShipmentsController(IGenericRepositroy<Shipment> ShipmentRepo,IMapper mapper, ICountryRepository countryRepository, ICityRepository cityRepository,IGenericRepositroy<Product> productRepo)
        {
            shipmentRepo = ShipmentRepo;
            this.mapper = mapper;
            this.countryRepository = countryRepository;
            this.cityRepository = cityRepository;
            this.productRepo = productRepo;
        }
        [ProducesResponseType(typeof(ShipmentToDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
      
        public async Task<ActionResult<IEnumerable<ShipmentToDto>>> GetShipments()
        {
            //var trip= await tripRepo.GetAllAsync();

            var spec = new ShipmentSpecification();
            var shipments = await shipmentRepo.GetAllWithSpecAsyn(spec);

            if (shipments is null) return NotFound(new ApiResponse(404));
            var mappedshipments = mapper.Map<IEnumerable<Shipment>, IEnumerable<ShipmentToDto>>(shipments);

            return Ok(mappedshipments);
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
        public async Task<ActionResult<Shipment>> CreateShipment(ShipmentToDto shipmentCreateDto)
        {
            if (ModelState.IsValid)
            {
                // استرجاع المدينة والبلد للشحنة من المستودعات
                var fromCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.FromCityName);
                var fromCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameFrom);
                var toCity = await cityRepository.GetCityByNameAsync(shipmentCreateDto.ToCityName);
                var toCountry = await countryRepository.GetCountryByNameAsync(shipmentCreateDto.CountryNameTo);

                // التحقق من وجود المدينة والبلد
                if (fromCity != null && fromCountry != null && toCity != null && toCountry != null)
                {
                    // تعيين معرفات المدينة والبلد للشحنة
                    var mappedTrip = mapper.Map<ShipmentToDto, Shipment>(shipmentCreateDto);

                    mappedTrip.FromCityID = fromCity.Id;
                    mappedTrip.FromCity = fromCity;
                    mappedTrip.FromCity.CountryId = fromCountry.Id;
                    mappedTrip.ToCityId = toCity.Id;
                    mappedTrip.ToCity = toCity;
                    mappedTrip.ToCity.CountryId = toCountry.Id;

                    //// إضافة المنتجات إلى الشحنة
                    //foreach (var productDto in shipmentCreateDto.Products)
                    //{
                    //    // استرجاع المنتج من قاعدة البيانات
                    //    //var product = await productRepo.GetByIdAsync(productId);
                    //    //if (product != null)
                    //    //{
                    //    //    mappedTrip.Products.Add(product);
                    //    //    product.shipments.Add(mappedTrip);
                    //    //}
                    //    var product = new Product
                    //    {
                    //        ProductName = productDto.ProductName,
                    //        ProductPrice = productDto.ProductPrice,
                    //        ProductWeight = productDto.ProductWeight,
                    //        PictureUrl = productDto.PictureUrl,

                    //        // اضف المزيد من البيانات حسب الحاجة
                    //    };
                    //    mappedTrip.Products.Add(product);
                    //}

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
