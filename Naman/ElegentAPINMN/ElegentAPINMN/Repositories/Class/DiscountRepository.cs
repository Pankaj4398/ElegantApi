using ElegentAPINMN.Data;
using ElegentAPINMN.Models.Domain;
using ElegentAPINMN.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace ElegentAPINMN.Repositories.Class
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DiscountRepository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public async Task<Discount> CreateAsync(Discount discount)
        {
            await _dbContext.Discounts.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount> DeleteAsync(Guid Id)
        {
            var discountRes = await _dbContext.Discounts.FirstOrDefaultAsync(x => x.Id == Id);
            if(discountRes != null)
            {
                _dbContext.Discounts.Remove(discountRes);
                await _dbContext.SaveChangesAsync();
                return discountRes;
            }
            return null;
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            var discountsList = await _dbContext.Discounts.ToListAsync();
            return discountsList;
        }

        public async Task<Discount?> GetByIdAsync(Guid id)
        {
            var Disc = await _dbContext.Discounts.FirstOrDefaultAsync(x=>x.Id == id);
            if(Disc != null)
            {
                return Disc;
            }
            return null;
        }

        public async Task<Discount?> UpdateAsync(Guid Id, Discount discount)
        {
            var toUpdateDisc = await _dbContext.Discounts.FirstOrDefaultAsync(x=>x.Id==Id);
            if(toUpdateDisc != null)
            {
                toUpdateDisc.Name = discount.Name;
                toUpdateDisc.Description = discount.Description;
                toUpdateDisc.Discount_Percent = discount.Discount_Percent;
                toUpdateDisc.Created_At = discount.Created_At;
                toUpdateDisc.Modified_At = discount.Modified_At;
                await _dbContext.SaveChangesAsync();
                return toUpdateDisc;
            }
            return null;

        }
    }
}
