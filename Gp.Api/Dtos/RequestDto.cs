using System.ComponentModel.DataAnnotations;

namespace Gp.Api.Dtos
{
    public class RequestDto
    {
        
     public int? Id { get; set; }

    public ShipmentToDto ShipmentToDto { get; set; }

     public TripToDto TripToDto { get; set; }




    }
}
