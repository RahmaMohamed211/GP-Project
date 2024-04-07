﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP.Core.Repositories
{
    public interface INameToIdResolver
    {
        Task<int> ResolveCityIdByNameAsync(string cityName);
    Task<int> ResolveCountryIdByNameAsync(string countryName);
    }
}
