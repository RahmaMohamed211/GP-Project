﻿using GP.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections;
using GP.core.Sepecifitction;


namespace GP.Core.Specificatios
{
    public class ShipmentSpecification :BaseSpecification<Shipment>
    {
        public ShipmentSpecification(TripwShSpecParams tripwShSpec) //get
        {
            includes.Add(sh => sh.FromCity);
            includes.Add(sh => sh.ToCity);
            includes.Add(sh => sh.ToCity.Country);
            includes.Add(sh => sh.FromCity.Country);
            includes.Add(Sh => Sh.Products);
            includes.Add(sh => sh.Category);
          

            if (!string.IsNullOrEmpty(tripwShSpec.Sort))
            {
                switch (tripwShSpec.Sort)
                {
                    case "RewardAsc":
                        AddOrderBy(T => T.Reward);
                        break;
                    case "RewardDesc":
                        AddOrderByDescending(T => T.Reward);
                        break;

                    default:
                        AddOrderBy(T => T.ToCity.Country.NameCountry);
                        break;
                }
            }
            //totalTrips=50;
            //pageSize=10;
            //pageIndex=3;
            ApplyPagination(tripwShSpec.PageSize * (tripwShSpec.PageIndex - 1), tripwShSpec.PageSize);
        





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
