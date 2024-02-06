using GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GP.Core.Specificatios
{
    public class ShipmentSpecification :BaseSpecification<Shipment>
    {
        public ShipmentSpecification() //get
        {
            includes.Add(sh => sh.FromCity);
            includes.Add(sh => sh.ToCity);
            includes.Add(sh => sh.ToCity.Country);
            includes.Add(sh => sh.FromCity.Country);
            includes.Add(Sh => Sh.Products);
            







        }

        public ShipmentSpecification(int id) : base(T => T.Id == id) //getById
        {
            includes.Add(T => T.FromCity);
            includes.Add(T => T.ToCity);
            includes.Add(T => T.ToCity.Country);
            includes.Add(T => T.FromCity.Country);
            includes.Add(Sh => Sh.Products);
           
        }

    }
}
