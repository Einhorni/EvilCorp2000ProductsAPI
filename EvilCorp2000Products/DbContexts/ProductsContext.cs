using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace EvilCorp2000Products.DbContexts
{
    public class ProductsContext : DbContext
    {
        public DbSet<Entities.Product> Products { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options) { }
    }
}