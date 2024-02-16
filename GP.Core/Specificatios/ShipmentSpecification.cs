using GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections;


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
            includes.Add(sh => sh.Category);

            // includes.Add(sh => sh.Products.Select(p => p.Category));

            // includes.Add(sh => sh.Products.Include(p => p.Category));


            //  includes.Add(sh => sh.Products.SelectMany(p => p.Category));

            // includes.Add(sh => Enumerable.Repeat(sh.Products.Select(p => p.Category), 1));


            //includes.Add(sh => sh.Products)
            //    .ThenInclude(c => c.Category);


            // includes.Add(sh => sh.Products).ThenInclude(p => p.Category);


            //includes.Add(sh => sh.Products.Where(p =>p.Id==1).Select(p => p.Category));

            //includes.Add(sh => sh.Products.AsEnumerable().Select(p => p.Category));



            //   AddThenInclude(Sh => Sh.Products.Select(p => p.Category));






            //AddThenInclude(Sh => Sh.Products, p => p.Category);





        }

        public ShipmentSpecification(int id) : base(T => T.Id == id) //getById
        {
            includes.Add(T => T.FromCity);
            includes.Add(T => T.ToCity);
            includes.Add(T => T.ToCity.Country);
            includes.Add(T => T.FromCity.Country);
            includes.Add(Sh => Sh.Products);
            includes.Add(sh => sh.Category);
        }

    }
}
