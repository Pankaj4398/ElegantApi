using ElegentAPINMN.Models.Domain;

namespace ElegentAPINMN.Repositories.Interface
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetAllAsync();
        Task<CartItem?> GetByIdAsync(Guid id);
        Task<CartItem> CreateAsync(CartItem discount);
        Task<CartItem?> UpdateAsync(Guid Id, CartItem discount);
        Task<CartItem> DeleteAsync(Guid Id);
    }
}
