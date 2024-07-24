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



        public async Task<(IEnumerable<Product>, PaginationMetaData)> GetProducts(string? productClass, string? searchquery, int currentPageNumber, int pageSize)
        {
            
            IQueryable<Product> productCollection = _productsContext.Products;

            //Query zusammenbauen:
            //falls nach Produktnamen gefiltert
            if (!string.IsNullOrWhiteSpace(productClass))
            {
                productClass = productClass.Trim();
                productCollection = productCollection.Where(p => p.ProductName == productClass);
            }

            //falls nach etwas gesucht
            if (!string.IsNullOrWhiteSpace(searchquery))
            {
                searchquery = searchquery.Trim();
                productCollection = productCollection
                    .Where(p => p.ProductName.Contains(searchquery)
                        || (p.ProductDescription != null && p.ProductDescription.Contains(searchquery)));
            }

            var totalItemCount = await productCollection.CountAsync();

            var paginationMetaData = new PaginationMetaData(currentPageNumber, pageSize, totalItemCount);

            var products = await productCollection
                .Skip((currentPageNumber-1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, paginationMetaData);
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
