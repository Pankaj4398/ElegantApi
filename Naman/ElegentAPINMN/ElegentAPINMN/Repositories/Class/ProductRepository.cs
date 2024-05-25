using ElegentAPINMN.Data;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Repositories.Class
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
        public async Task<Product> UpdateAsync(Guid Id, Product product)
        {
            var existingProduct =  await _dbContext.Products.FirstOrDefaultAsync(x=>x.Id == Id);
            if(existingProduct != null)
            {
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Category = product.Category;
                existingProduct.sku = product.sku;
                existingProduct.Modified_At = product.Modified_At;

                await _dbContext.SaveChangesAsync();
                return existingProduct;
            }

            return null;
        }

        public async Task<Product> DeleteAsync(Guid Id)
        {
            var deletedProduct = await _dbContext.Products.FirstOrDefaultAsync(x=>x.Id==Id);
            if(deletedProduct != null)
            {
                _dbContext.Products.Remove(deletedProduct);
                await _dbContext.SaveChangesAsync();
                return deletedProduct;
            }
            return null;

        }
    }
}
