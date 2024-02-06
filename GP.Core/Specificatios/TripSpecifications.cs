using GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GP.Core.Specificatios
{
    public class TripSpecifications :BaseSpecification<Trip>
    {
        public TripSpecifications() //get
        {
            includes.Add(T => T.FromCity);
            includes.Add(T => T.ToCity);
            includes.Add(T => T.ToCity.Country);
            includes.Add(T => T.FromCity.Country);

        }

        public TripSpecifications(int id):base(T=>T.Id==id) //getById
        {
            includes.Add(T => T.FromCity);
            includes.Add(T => T.ToCity);
            includes.Add(T => T.ToCity.Country);
            includes.Add(T => T.FromCity.Country);
            //includes.Add(T => T.FromCity.CountryId);
            //includes.Add(T => T.ToCity.CountryId);
        }

     

    }
}
