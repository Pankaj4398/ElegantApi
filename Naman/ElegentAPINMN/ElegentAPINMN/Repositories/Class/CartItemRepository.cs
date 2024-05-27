using ElegentAPINMN.Data;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Repositories.Class
{
    public class CartItemRepository:ICartItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CartItemRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<CartItem> CreateAsync(CartItem cartItem)
        {
            await _dbContext.CartItems.AddAsync(cartItem);
            await _dbContext.SaveChangesAsync();
            return cartItem;
        }

        public async Task<CartItem?> DeleteAsync(Guid Id)
        {
            //get the cart item based on the id provided
            var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(x=>x.Id == Id);
            
            //if it is present then remove the cartitem
            if(cartItem != null)
            {
                _dbContext.CartItems.Remove(cartItem);
                await _dbContext.SaveChangesAsync();
                return cartItem;
            }
            return null;
        }

        public async Task<List<CartItem>> GetAllAsync()
        {
            var result = await _dbContext.CartItems.ToListAsync();
            return result;
        }

        public async Task<CartItem?> GetByIdAsync(Guid id)
        {
            //get the data based on id
            var res = await _dbContext.CartItems.FirstOrDefaultAsync(x=>x.Id.Equals(id));
            return res;
        }

        public async Task<CartItem?> UpdateAsync(Guid Id, CartItem cartItem)
        {
            //get the cart item based on id
            var cartItemToBeEdited = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.Id == Id);

            //update the cart item based on the cartItem given
            if(cartItemToBeEdited != null )
            {
                cartItemToBeEdited.Quantity = cartItem.Quantity;
                cartItemToBeEdited.Created_At = cartItem.Created_At;
                cartItemToBeEdited.Modified_At = cartItem.Modified_At;
                cartItemToBeEdited.ShoppingSessionId = cartItem.ShoppingSessionId;
                cartItemToBeEdited.Product_Id = cartItem.Product_Id;
                await _dbContext.SaveChangesAsync();
            }

            //return the updated cartitem
            return cartItemToBeEdited;
        }
    }
}
