using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Core.Entities
{
    public class Shipment : BaseEntity
    {

        public decimal Reward { get; set; }

        public decimal Weight { get; set; }

        public DateTime Dateofcreation { get; set; } = DateTime.Now;


        public int FromCityID { get; set; }
        public City FromCity { get; set; }

       

        public int ToCityId { get; set; }
        public City ToCity { get; set; }

        public string Address { get; set; }

        public DateTime DateOfRecieving { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
