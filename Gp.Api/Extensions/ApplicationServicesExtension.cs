using Gp.Api.Errors;
using GP.Core.Repositories;
using GP.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Gp.Api.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            //services.AddScoped<IGenericRepositroy<>, GenericRepositorty<>>();
            services.AddScoped(typeof(IGenericRepositroy<>), typeof(GenericRepositorty<>));


            //services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));

            //services.AddAutoMapper(typeof(MappingProfiles)); // El mafrood mesh commented


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
