using AutoMapper;
using Gp.Api.Dtos;
using Gp.Api.Errors;
using GP.Core.Entities;
using GP.Core.Repositories;
using GP.Core.Specificatios;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gp.Api.Controllers
{
  
    public class ShipmentsController : ApiBaseController
    {
        private readonly IGenericRepositroy<Shipment> shipmentRepo;
        private readonly IMapper mapper;

        public ShipmentsController(IGenericRepositroy<Shipment> ShipmentRepo,IMapper mapper)
        {
            shipmentRepo = ShipmentRepo;
            this.mapper = mapper;
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

    }
}
