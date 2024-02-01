using AutoMapper;
using Gp.Api.Dtos;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShipmentToDto>>> GetShipments()
        {
            //var trip= await tripRepo.GetAllAsync();

            var spec = new ShipmentSpecification();
            var shipments = await shipmentRepo.GetAllWithSpecAsyn(spec);

            var mappedshipments = mapper.Map<IEnumerable<Shipment>, IEnumerable<ShipmentToDto>>(shipments);

            return Ok(mappedshipments);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ShipmentToDto>> GetShipment(int id)
        {
            var spec = new ShipmentSpecification(id);
            var shipment = await shipmentRepo.GetByIdwithSpecAsyn(spec);
            var mappedshipment = mapper.Map<Shipment, ShipmentToDto>(shipment);
            return Ok(mappedshipment);
        }

    }
}
