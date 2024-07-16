using System.ComponentModel.DataAnnotations;

namespace EvilCorp2000Products.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? ProductDescription { get; set; }
        public string? ProductPicture { get; set; }

        [Required]
        public int AmountOnStock { get; set; }
        public int? Rating { get; set; }
        //public int? ProductClassId { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        [MaxLength(20)]
        [Required]
        public string ProductClass { get; set; } = string.Empty;
    }
}
