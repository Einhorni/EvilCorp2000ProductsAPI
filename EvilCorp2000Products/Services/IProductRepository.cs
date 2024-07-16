using EvilCorp2000Products.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace EvilCorp2000Products.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        //optional
        Task<Product?> GetProductById(int id);
        void CreateNewProduct(Product product);
        void DeleteProduct(Product product);
        Task<bool> SaveChangesAsync();
    }
}
