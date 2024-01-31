using GP.Core.Entities;
using GP.Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gp.Api.Controllers
{
  
    public class ShipmentsController : ApiBaseController
    {
        private readonly IGenericRepositroy<Shipment> shipmentRepo;

        public ShipmentsController(IGenericRepositroy<Shipment> ShipmentRepo)
        {
            shipmentRepo = ShipmentRepo;
        }
        
    }
}
