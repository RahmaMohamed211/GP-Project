using AutoMapper;
using Gp.Api.Dtos;
using GP.Core.Entities;

namespace Gp.Api.Hellpers
{
    public class ProductPictureUrlResolver : IValueResolver<Shipment, ShipmentToDto, string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Shipment source, ShipmentToDto destination, string destMember, ResolutionContext context)
        {
            if (source.Products != null && source.Products.Any())
            {
                
                var firstProduct = source.Products.First();
                if (!string.IsNullOrEmpty(firstProduct.PictureUrl))
                {
                    return $"{configuration["ApiBaseUrl"]}{firstProduct.PictureUrl}";
                }
            }
            return string.Empty;




        }
    }
}
