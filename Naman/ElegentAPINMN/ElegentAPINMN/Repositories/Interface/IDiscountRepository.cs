using ElegentAPINMN.Models.Domain;
using System.Security.Cryptography.Xml;

namespace ElegentAPINMN.Repositories.Interface
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(Guid id);
        Task<Discount> CreateAsync(Discount discount);
        Task<Discount?> UpdateAsync(Guid Id, Discount discount);
        Task<Discount> DeleteAsync(Guid Id);
    }
}
