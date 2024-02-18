namespace Gp.Api.Dtos
{
    public class ShipmentToDto
    {
        public int Id { get; set; }

        public decimal Reward { get; set; }

        public decimal Weight { get; set; }

        public DateTime Dateofcreation { get; set; }=DateTime.Now;

        public int? FromCityID { get; set; }
        public string FromCityName { get; set; }

        public int? CountryIdFrom { get; set; }

        public string CountryNameFrom { get; set; }

        public int? ToCityId { get; set; }

        public string ToCityName { get; set; }

        public int? CountryIdTo { get; set; }

        public string CountryNameTo { get; set; }

        public DateTime DateOfRecieving { get; set; }
        public string Address { get; set; }

       // public List<ProductsDto> Products { get; set; }
        
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductWeight { get; set; }

        public string? PictureUrl { get; set; }

       // public IFormFile? Image {  get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
