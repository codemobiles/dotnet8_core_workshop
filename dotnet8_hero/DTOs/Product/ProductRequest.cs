namespace dotnet8_hero.DTOs.Product
{
    public class ProductRequest
    {
        public string Name { get; set; } = null!;
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? FormFiles { get; set; }
    }

}