using EvilCorp2000Products.Models;

namespace EvilCorp2000Products
{
    public class MockProducts
    {
        public List<ProductDTO> Products { get; set; }

        public static MockProducts CurrentProducts { get; } = new MockProducts();

        

        public MockProducts() 
        {
            Products = new List<ProductDTO>()
            //warum funktioniert das nicht?
            //List<ProductDTO> Products =
            {
                //[
                    new ProductDTO()
                    {
                        ProductId = 1,
                        ProductName = "Haifischbecken",
                        ProductDescription = "Have fun and wach enemies suffer primal fear",
                        ProductClassId = 0,
                        ProductClass = "Facilities",
                        ProductPicture = null,
                        ProductPrice = 20000.00m,
                        AmountOnStock = 0,
                        Rating = 0,
                    },
                    new ProductDTO()
                    {
                        ProductId = 2,
                        ProductName = "Guilloutine",
                        ProductDescription = "The classic",
                        ProductClassId = 0,
                        ProductClass = "Furniture",
                        ProductPicture = null,
                        ProductPrice = 5000.00m,
                        AmountOnStock = 0,
                        Rating = 0,
                    },
                    new ProductDTO()
                    {
                        ProductId = 5,
                        ProductName = "Narcotics - set of 10 syringes",
                        ProductDescription = "Convinient, silent, works every time",
                        ProductClassId = 0,
                        ProductClass = "Consumable",
                        ProductPicture = null,
                        ProductPrice = 100.00m,
                        AmountOnStock = 0,
                        Rating = 0,
                    }
                //];
            };
        }
    }
}
