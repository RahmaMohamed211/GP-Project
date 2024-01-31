using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GP.Core.Entities
{
    public class Trip :BaseEntity
    {
       
        public int FromCityID { get; set; }
        public City FromCity { get; set; }

       
        public int ToCityId { get; set; }
        public City ToCity { get; set; }

        public decimal availableKg { get; set; }

        public DateTime arrivalTime { get; set; }

        public DateTime DateofCreation { get; set; } = DateTime.Now;
    }
}
