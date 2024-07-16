using System.ComponentModel.DataAnnotations;

namespace EvilCorp2000Products.Models
{
    public class ProductForCreationDto
    {
        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;
        public string? ProductDescription { get; set; }
        public string? ProductPicture { get; set; }
        [Required]
        public int AmountOnStock { get; set; }
        [Required]
        public decimal ProductPrice { get; set; }
        [Required]
        [MaxLength(20)]
        public string ProductClass { get; set; } = string.Empty;
    }
}
