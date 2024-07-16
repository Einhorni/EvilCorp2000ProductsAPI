using EvilCorp2000Products.DbContexts;
using EvilCorp2000Products.Entities;
using Microsoft.EntityFrameworkCore;

namespace EvilCorp2000Products.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext _productsContext;

        public ProductRepository(ProductsContext productsContext)
        {
            this._productsContext = productsContext ?? throw new ArgumentNullException(nameof(productsContext));
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _productsContext.Products.ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _productsContext.Products.Where(p => p.ProductId == id).FirstOrDefaultAsync();
        }

        public void CreateNewProduct(Product product)
        {
            _productsContext.Products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _productsContext.Products.Remove(product);
        }

        public async Task<bool> SaveChangesAsync()
        { 
            return (await _productsContext.SaveChangesAsync() >= 0);
        }
    }
}
