using ElegentAPINMN.Models.Domain;

namespace ElegentAPINMN.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> CreateAsync(Product product);
        Task<Product?> UpdateAsync(Guid Id, Product product);
        Task<Product> DeleteAsync(Guid Id);
    }
}
