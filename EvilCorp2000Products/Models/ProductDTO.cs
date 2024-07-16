namespace EvilCorp2000Products.Models
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public string? ProductPicture { get; set; }
        public int AmountOnStock { get; set; }
        public int? Rating { get; set; }
        public int? ProductClassId { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductClass { get; set; } = string.Empty;
    }
}
