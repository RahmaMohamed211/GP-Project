namespace Gp.Api.Dtos
{
    public class ProductsDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductWeight { get; set; }

        public string? PictureUrl { get; set; }
    }
}
