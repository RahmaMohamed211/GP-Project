using Gp.Api.Errors;
using Gp.Api.Hellpers;
using GP.Core.Repositories;
using GP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gp.Api.Extensions
{
    public static class ApplicationServicesExtension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            
            services.AddScoped(typeof(IGenericRepositroy<>), typeof(GenericRepositorty<>));


            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped(typeof(IGenericRepositroy<>), typeof(GenericRepositorty<>));
            services.AddScoped(typeof(INameToIdResolver), typeof(NameToIdResolver));
           services.AddScoped(typeof(ICountryRepository), typeof(CountryRepository));
            services.AddScoped(typeof(ICityRepository), typeof(CityRepository));
            services.AddScoped(typeof(ICategoryRepository), typeof(categoryRepository));


            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray();


                    var validationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(validationErrorResponse);
                };
            });
        
            return services;    
        }
    }
}
