using AlHafiz.AppDbContext;
using AlHafiz.Models;
using AlHafiz.Services.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AlHafiz.Services.Repository
{
    public class CustomerItemRateRepository : ICustomerItemRateRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerItemRateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerItemRate>> GetAllRatesAsync()
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .OrderBy(r => r.Customer.Name)
                .ThenBy(r => r.Item.Name)
                .ToListAsync();
        }

        public async Task<CustomerItemRate> GetRateByIdAsync(int id)
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<CustomerItemRate> GetRateByCustomerAndItemAsync(int customerId, int itemId)
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);
        }

        public async Task<IEnumerable<CustomerItemRate>> GetRatesByCustomerAsync(int customerId)
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .Where(r => r.CustomerId == customerId)
                .OrderBy(r => r.Item.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerItemRate>> GetRatesByItemAsync(int itemId)
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .Where(r => r.ItemId == itemId)
                .OrderBy(r => r.Customer.Name)
                .ToListAsync();
        }

        public async Task<CustomerItemRate> AddRateAsync(CustomerItemRate rate)
        {
            rate.CreatedAt = DateTime.Now;
            _context.CustomerItemRates.Add(rate);
            await _context.SaveChangesAsync();

            // Load the navigation properties
            return await GetRateByIdAsync(rate.Id);
        }

        public async Task<CustomerItemRate> UpdateRateAsync(CustomerItemRate rate)
        {
            rate.UpdatedAt = DateTime.Now;
            _context.CustomerItemRates.Update(rate);
            await _context.SaveChangesAsync();

            // Load the navigation properties
            return await GetRateByIdAsync(rate.Id);
        }

        public async Task<bool> DeleteRateAsync(int id)
        {
            var rate = await _context.CustomerItemRates.FindAsync(id);
            if (rate == null)
                return false;

            _context.CustomerItemRates.Remove(rate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRateByCustomerAndItemAsync(int customerId, int itemId)
        {
            var rate = await _context.CustomerItemRates
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);

            if (rate == null)
                return false;

            _context.CustomerItemRates.Remove(rate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<decimal?> GetCustomerItemRateAsync(int customerId, int itemId)
        {
            var rate = await _context.CustomerItemRates
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);

            return rate?.Rate;
        }

        public async Task<bool> SetCustomerItemRateAsync(int customerId, int itemId, decimal rate)
        {
            var existingRate = await _context.CustomerItemRates
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.ItemId == itemId);

            if (existingRate != null)
            {
                // Update existing rate
                existingRate.Rate = rate;
                existingRate.UpdatedAt = DateTime.Now;
                _context.CustomerItemRates.Update(existingRate);
            }
            else
            {
                // Create new rate
                var newRate = new CustomerItemRate
                {
                    CustomerId = customerId,
                    ItemId = itemId,
                    Rate = rate,
                    CreatedAt = DateTime.Now
                };
                _context.CustomerItemRates.Add(newRate);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RateExistsAsync(int customerId, int itemId)
        {
            return await _context.CustomerItemRates
                .AnyAsync(r => r.CustomerId == customerId && r.ItemId == itemId);
        }

        public async Task<IEnumerable<CustomerItemRate>> GetRatesMatrixAsync()
        {
            return await _context.CustomerItemRates
                .Include(r => r.Customer)
                .Include(r => r.Item)
                .ToListAsync();
        }
    }
}